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

    private List<Card> _topCardList = new();
    private Card _discardCard = null;

    private CardContext _rootCardContext;

    private JsonReader jsonReader;


    private void Start()
    {
        jsonReader = new JsonReader();
        
        _rootCardContext = jsonReader.ReadJsonForCardContext(_jsonText);
        _rootCardContext.InitCardContextRecursive(null, new List<int>(), 0);

        Card card = CreateNewCardDeck();
        
        //_layoutManager.SetPlaceLayout(null, startItem, null, null);

        SetLayout(card);
    }

    private Card CreateNewCardDeck()
    {
        _topCardList.Clear();

        Card card = CreateAndAddCardsRepeating(null, _rootCardContext);

        _topCardList.Add(card);

        return card;
    }

    private Card ResetExistingCardDeck()
    {
        // Reference Cardcontext to set cards back in their place
        return ResetCardRepeating(_rootCardContext);

    }

    private Card ResetCardRepeating(CardContext cardContext)
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

        cardToReset.SetChildCards(cardContext.GetListOfReferencedCards());

        foreach (Card childCard in cardToReset.GetChildCards())
        {
            ResetCardRepeating(childCard.GetCardContext());
        }

        return cardToReset;
    }


    private Card CreateAndAddCardsRepeating(Card parentCard, CardContext cardContext)
    {
        Card card = CreateCardAddToCard(parentCard, cardContext);

        List<Card> childCards = new();
        for (int i = 0; i < cardContext.ChildCardContexts.Count; i++)
        {
            childCards.Add(CreateAndAddCardsRepeating(card, cardContext.ChildCardContexts[i]));
        }

        card.SetChildCards(childCards);

        return card;
        
    }

    private void Update()
    {
        foreach (Card topCard in _topCardList)
        {
            topCard.SynchronizeHeight();
        }
    }

    public void SetLayout(Card pressedCard)
    {
        Card mainCard = null;
        List<Card> subCards = new List<Card>();
        Card backCard = null;
        Card discardCard = null;
        

        CardContext pressedCardContext = pressedCard.GetCardContext();

        // Reset everything
        Card rootCard = ResetExistingCardDeck();

        // Find cards relative to pressedCard

        //Main
        mainCard = pressedCard;

        //Sub
        subCards = pressedCardContext.GetListOfReferencedCards();
        
        CardContext parentCardContext = pressedCardContext.GetParentContext();

        if (parentCardContext != null)
        {
            //Back
            backCard = parentCardContext.GetCard();
        }

        Card root = rootCard;

        if (root != pressedCard && root != parentCardContext.GetCard())
        {
            //Discard
            discardCard = root;
        }

        // Change their Behaviour

        DetachCard(mainCard);
        foreach(Card subCard in subCards)
        {
            DetachCard(subCard);
        }
        if (backCard != null)
        {
            DetachCard(backCard);
        }
        if (discardCard != null)
        {
            DetachCard(discardCard);
        }


        mainCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.NOTHING));
        foreach (Card subCard in subCards)
        {
            subCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPIN));
        }
        if (backCard != null)
        {
            backCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPOUT));
        }
        if (discardCard != null)
        {
            discardCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.NOTHING));
        }
        
        _layoutManager.SetPlaceLayout(mainCard, subCards, backCard, discardCard);
    }

    #region Attach / Detach
    public void DetachCard(Card cardToRemove)
    {
        if (cardToRemove.GetParentCard() != null)
        {
            cardToRemove.GetParentCard().GetChildCards().Remove(cardToRemove);
            cardToRemove.SetParentCard(null);
            
            _topCardList.Add(cardToRemove);

            cardToRemove.SynchronizeVisual();
        }
        
    }

    public void AttachCard(Card cardToAttach, Card baseCard)
    {
        if (!_topCardList.Contains(cardToAttach))
        {
            Debug.LogWarning("FEHLER");
        }

        baseCard.AttachCardAtEnd(cardToAttach);

        _topCardList.Remove(cardToAttach);
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
