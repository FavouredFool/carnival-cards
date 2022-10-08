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

    private List<CardContext> _topCardContextList = new();

    private CardContext _rootCardContext;

    private JsonReader jsonReader;


    private void Start()
    {
        jsonReader = new JsonReader();
        
        _rootCardContext = jsonReader.ReadJsonForCardContext(_jsonText);
        _rootCardContext.InitCardContextRecursive(null, new List<int>(), 0);

        CreateNewCardDeck();
       
        SetLayout(_rootCardContext);
    }

    public CardContext FindCardContextFromCard(Card card)
    {
        return _rootCardContext.FindCardContextFromCard(card);
    }

    private void CreateNewCardDeck()
    {
        _topCardContextList.Clear();

        CreateAndAddCardsRepeating(null, _rootCardContext);

        _topCardContextList.Add(_rootCardContext);
    }

    private void ResetExistingCardDeck()
    {
        // Reference Cardcontext to set cards back in their place
        ResetCardRepeating(_rootCardContext);
        _topCardContextList.Clear();
        _topCardContextList.Add(_rootCardContext);

    }

    private void ResetCardRepeating(CardContext cardContext)
    {
        Card cardToReset = cardContext.GetCard();
        if (cardContext.GetParentContext() != null)
        {
            cardToReset.SetParentCard(cardContext.GetParentContext().GetCard());
        }
        else
        {
            cardToReset.SetParentCard(null);
        }

        foreach (CardContext childCardContext in cardContext.ChildCardContexts)
        {
            ResetCardRepeating(childCardContext);
        }
    }


    private Card CreateAndAddCardsRepeating(Card parentCard, CardContext cardContext)
    {
        Card card = CreateCardAddToCard(parentCard, cardContext);

        List<Card> childCards = new();
        for (int i = 0; i < cardContext.ChildCardContexts.Count; i++)
        {
            childCards.Add(CreateAndAddCardsRepeating(card, cardContext.ChildCardContexts[i]));
        }

        return card;
        
    }

    private void Update()
    {
        foreach (CardContext topCardContext in _topCardContextList)
        {
            topCardContext.SynchronizeHeight();
        }
    }

    public void SetLayout(CardContext pressedCardContext)
    {
        CardContext mainCardContext;
        List<CardContext> subCardContexts;
        CardContext backCardContext;
        CardContext discardCardContext = null;
        

        // Reset everything
        ResetExistingCardDeck();

        // Find cards relative to pressedCard

        //Main
        mainCardContext = pressedCardContext;

        //Sub
        subCardContexts = pressedCardContext.ChildCardContexts;

        //Back
        backCardContext = pressedCardContext.GetParentContext();

        if (_rootCardContext != mainCardContext && _rootCardContext != backCardContext)
        {
            //Discard
            discardCardContext = _rootCardContext;
        }

        // Change their Behaviour

        DetachCard(mainCardContext);
        foreach(CardContext subCardContext in subCardContexts)
        {
            DetachCard(subCardContext);
        }
        if (backCardContext != null)
        {
            DetachCard(backCardContext);
        }
        if (discardCardContext != null)
        {
            DetachCard(discardCardContext);
        }
        
        _layoutManager.SetPlaceLayout(mainCardContext, subCardContexts, backCardContext, discardCardContext);
    }

    #region Attach / Detach
    public void DetachCard(CardContext contextToRemove)
    {
        if (contextToRemove.GetCard().GetIsAttached())
        {
            contextToRemove.GetCard().SetParentCard(null);
            
            _topCardContextList.Add(contextToRemove);

            contextToRemove.GetCard().SynchronizeVisual();
        }
        
    }

    public void AttachCard(CardContext cardContextToAttach, CardContext baseCardContext)
    {
        if (!_topCardContextList.Contains(cardContextToAttach))
        {
            Debug.LogWarning("FEHLER");
        }

        baseCardContext.AttachCardAtEnd(cardContextToAttach);

        _topCardContextList.Remove(cardContextToAttach);
    }

    #endregion


    #region Create Stuff

    public Card CreateCardAddToCard(Card parentCard, CardContext cardContext)
    {
        Card newCard = CreateCard(cardContext);

        if (parentCard != null)
        {
            newCard.SetParentCard(parentCard);
        }
        
        return newCard;
    }

    public Card CreateCard(CardContext cardContext)
    {
        return _cardFactory.CreateNewInstance(cardContext);
    }
    #endregion



}
