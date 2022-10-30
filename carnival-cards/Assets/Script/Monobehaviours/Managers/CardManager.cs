using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static CardTypeManager;

public class CardManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private TextAsset _jsonText;
    [SerializeField]
    private CardFactory _cardFactory;
    [SerializeField]
    private LayoutManager _layoutManager;
    [SerializeField]
    private OnClickManager _onClickManager;

    [Header("Helpers")]
    [SerializeField]
    private Transform _closeUpPosition;

    private List<Context> _topContextList = new();
    private Context _inventoryContext = null;
    private Context _closeUpContext = null;

    private Context _rootContext;

    private JsonReader jsonReader;

    private bool _inventoryIsOpen = false;

    private Context _activeContext;


    private void Start()
    {

        // Deck
        jsonReader = new JsonReader();
        
        _rootContext = jsonReader.ReadJsonForContext(_jsonText);
        _rootContext.InitContextRecursive(null, new List<int>(), 0);

        CreateNewCardDeck();

        SetActiveContext(_rootContext);
        SetLayout(_rootContext);

        // Inventory
        _inventoryContext = new Context("Inventory", CardType.INVENTORY);
        CreateCardAddToCard(null, _inventoryContext);
        _layoutManager.SetInventoryContext(_inventoryContext);
    }

    private void Update()
    {
        foreach (Context topContext in _topContextList)
        {
            topContext.SynchronizeHeight();
        }

        _inventoryContext.SynchronizeHeight();
    }

    public void CloseUpCardback(Context context)
    {
        Debug.Log("No");
        return;

        // Turn card around, zoom in / enhance. Animation or just "jump"?
        //_closeUpContext = context;
        //context.GetCard().transform.parent = _closeUpPosition;
    }

    public Context FindContextFromCard(Card card)
    {
        Context context = _rootContext.FindContextFromCard(card);

        if (context != null)
        {
            return context;
        }

        context = _inventoryContext.FindContextFromCard(card);

        if (context != null)
        {
            return context;
        }

        Debug.LogWarning("ERROR");
        return null;
    }

    private void CreateNewCardDeck()
    {
        _topContextList.Clear();

        CreateAndAddCardsRepeating(null, _rootContext);

        _topContextList.Add(_rootContext);
    }

    private void ResetExistingCardDeck()
    {
        ResetCardRepeating(_rootContext);
        _topContextList.Clear();
        _topContextList.Add(_rootContext);
        _closeUpContext = null;
    }

    private void ResetCardRepeating(Context context)
    {
        Card cardToReset = context.GetCard();
        if (context.GetParentContext() != null)
        {
            cardToReset.SetParentCard(context.GetParentContext().GetCard());
        }
        else
        {
            cardToReset.SetParentCard(null);
        }

        foreach (Context childContext in context.ChildContexts)
        {
            ResetCardRepeating(childContext);
        }
    }

    private Card CreateAndAddCardsRepeating(Card parentCard, Context context)
    {
        Card card = CreateCardAddToCard(parentCard, context);

        List<Card> childCards = new();
        for (int i = 0; i < context.ChildContexts.Count; i++)
        {
            childCards.Add(CreateAndAddCardsRepeating(card, context.ChildContexts[i]));
        }

        return card;
    }

    public void ResetLayout()
    {
        ResetExistingCardDeck();

        // Reset inventory items
        _layoutManager.SetInventoryContext(_inventoryContext);
        ResetInventory();
    }

    public void InitSetLayout(Context context)
    {
        ResetLayout();
        SetLayout(context);
    }

    public void ResetInventory()
    {
        ResetCardRepeating(_inventoryContext);
    }

    public void SetLayout(Context pressedContext)
    {
        switch (pressedContext.Type)
        {
            case CardType.COVER:
                _layoutManager.SetCoverLayout(pressedContext);
                break;
            case CardType.FLAVOR:
                _layoutManager.SetBasicLayout(pressedContext);
                break;
            case CardType.THING:
                _layoutManager.SetBasicLayout(pressedContext);
                break;
            case CardType.PLACE:
                _layoutManager.SetBasicLayout(pressedContext);
                break;
            case CardType.INVESTIGATION:
                _layoutManager.SetBasicLayout(pressedContext);
                break;
            case CardType.LOCK:
                _layoutManager.SetLockLayout(pressedContext, _inventoryContext);
                break;
            case CardType.INVENTORY:
                _layoutManager.SetInventoryLayout(pressedContext);
                break;
        }
    }

    public void InitToggleInventory(bool change)
    {
        ResetLayout();
        ToggleInventory(change);
        SetLayout(_activeContext);
    }

    public void ToggleInventory(bool change)
    {
        if (change)
        {
            _inventoryIsOpen = !_inventoryIsOpen;
        }

        _layoutManager.ToggleInventory(_inventoryContext, _inventoryIsOpen);
    }

    public void PickUp(Context context)
    {
        DetachContext(context);

        context.SetIdentifier(new() { 0, _inventoryContext.ChildContexts.Count });

        AttachContextToContext(context, _inventoryContext);

        ToggleInventory(false);
    }

    public void Unlock(Context context)
    {
        if (context.Type != CardType.ITEM)
        {
            Debug.LogWarning("ERROR");
        }

        // Grundarchitektur �ndern, sodass Infos wie "wurde bereits unlocked"
        // auf Klassenebene sitzen und nicht direkt vom CardManager geh�ndelt werden.

        if (_activeContext.UnlockItemName == context.Name)
        {
            Debug.Log("unlocked");
        }
        else
        {
            Debug.Log("nope");
        }
    }




    #region Create Stuff
    public Card CreateCardAddToCard(Card parentCard, Context context)
    {
        Card newCard = CreateCard(context);

        if (parentCard != null)
        {
            newCard.SetParentCard(parentCard);
        }
        
        return newCard;
    }

    public Card CreateCard(Context context)
    {
        return _cardFactory.CreateNewInstance(context);
    }
    #endregion


    private void DetachContext(Context context)
    {
        Context parentContext = context.GetParentContext();
        List<Context> childContexts = context.ChildContexts;

        if (parentContext == null || childContexts.Count != 0)
        {
            Debug.Log("ERROR: CANT BE REMOVED");
            return;
        }

        parentContext.ChildContexts.Remove(context);
        context.SetParentContext(null);
        _topContextList.Add(context);
    }

    private void AttachContextToContext(Context toAttach, Context basis)
    {
        _topContextList.Remove(toAttach);       
        basis.ChildContexts.Add(toAttach);
        toAttach.SetParentContext(basis);

        toAttach.GetCard().SetParentCard(_inventoryContext.GetCard());
    }

    public Transform GetCloseUpPosition()
    {
        return _closeUpPosition;
    }

    public Context GetCloseUpContext()
    {
        return _closeUpContext;
    }

    public Context GetRootContext()
    {
        return _rootContext;
    }

    public LayoutManager GetLayoutManager()
    {
        return _layoutManager;
    }

    public List<Context> GetTopContextList()
    {
        return _topContextList;
    }

    public Context GetActiveContext()
    {
        return _activeContext;
    }

    public void SetActiveContext(Context activeContext)
    {
        _activeContext = activeContext;
    }

}