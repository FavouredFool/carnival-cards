using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public TextAsset _jsonText;
    public CardFactory _cardFactory;
    public LayoutManager _layoutManager;
    public OnClickManager _onClickManager;

    private List<Context> _topContextList = new();

    private Context _rootContext;

    private JsonReader jsonReader;


    private void Start()
    {
        jsonReader = new JsonReader();
        
        _rootContext = jsonReader.ReadJsonForContext(_jsonText);
        _rootContext.InitContextRecursive(null, new List<int>(), 0);

        CreateNewCardDeck();
       
        SetLayout(_rootContext);
    }

    public Context FindContextFromCard(Card card)
    {
        return _rootContext.FindContextFromCard(card);
    }

    private void CreateNewCardDeck()
    {
        _topContextList.Clear();

        CreateAndAddCardsRepeating(null, _rootContext);

        _topContextList.Add(_rootContext);
    }

    private void ResetExistingCardDeck()
    {
        // Reference Cardcontext to set cards back in their place
        ResetCardRepeating(_rootContext);
        _topContextList.Clear();
        _topContextList.Add(_rootContext);

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

    private void Update()
    {
        foreach (Context topContext in _topContextList)
        {
            topContext.SynchronizeHeight();
        }
    }

    public void SetLayout(Context pressedContext)
    {
        Context mainContext;
        List<Context> subContexts;
        Context backContext;
        Context discontext = null;
        
        // Reset everything
        ResetExistingCardDeck();

        // Find cards relative to pressedCard

        //Main
        mainContext = pressedContext;

        //Sub
        subContexts = pressedContext.ChildContexts;

        //Back
        backContext = pressedContext.GetParentContext();

        if (_rootContext != mainContext && _rootContext != backContext)
        {
            //Discard
            discontext = _rootContext;
        }

        // Change their Behaviour

        DetachCard(mainContext);
        foreach(Context subContext in subContexts)
        {
            DetachCard(subContext);
        }
        if (backContext != null)
        {
            DetachCard(backContext);
        }
        if (discontext != null)
        {
            DetachCard(discontext);
        }
        
        _layoutManager.SetPlaceLayout(mainContext, subContexts, backContext, discontext);
    }

    #region Attach / Detach
    public void DetachCard(Context contextToRemove)
    {
        if (contextToRemove.GetCard().GetIsAttached())
        {
            contextToRemove.GetCard().SetParentCard(null);
            
            _topContextList.Add(contextToRemove);

            contextToRemove.GetCard().SynchronizeVisual();
        }
        
    }

    public void AttachCard(Context contextToAttach, Context baseContext)
    {
        if (!_topContextList.Contains(contextToAttach))
        {
            Debug.LogWarning("FEHLER");
        }

        baseContext.AttachCardAtEnd(contextToAttach);

        _topContextList.Remove(contextToAttach);
    }

    #endregion


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



}
