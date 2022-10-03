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

    private JsonReader jsonReader;


    private void Start()
    {
        jsonReader = new JsonReader();
        
        CardContext rootCardContext = jsonReader.ReadJsonForCardContext(_jsonText);
        rootCardContext.InitCardContextRecursive(null, new List<int>(), 0);
        Card card = CreateAndAddCardsRecursive(null, rootCardContext);

        _topCardList.Add(card);

        card.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPIN));

    }


    private Card CreateAndAddCardsRecursive(Card parentCard, CardContext cardContext)
    {
        Card card = CreateCardAddToCard(parentCard, cardContext);

        List<Card> childCards = new();
        for (int i = 0; i < cardContext.ChildCardContexts.Count; i++)
        {
            childCards.Add(CreateAndAddCardsRecursive(card, cardContext.ChildCardContexts[i]));
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

    public void SetPlaceLayout(Card card)
    {
        List<Card> childCardsCopy = new(card.GetChildCards());
        CardContext greaterCardContext = card.GetCardContext().GetParentContext();

        foreach (Card activeCard in childCardsCopy)
        {
            DetachCard(activeCard);
        }

        if (greaterCardContext != null)
        {
            AddSiblingsToGreaterCard(card, greaterCardContext);
            
            CardContext greaterGreaterCardContext = greaterCardContext.GetParentContext();

            if (greaterGreaterCardContext != null)
            {
                AddToDiscardCard(greaterGreaterCardContext);
            }
        }

        Card mainCard = card;
        List<Card> fanoutCards = card.GetCardContext().GetListOfReferencedCards();
        Card backCard = null;

        if (greaterCardContext != null)
        {
            backCard = greaterCardContext.GetCard();
        }

        _layoutManager.SetPlaceLayout(mainCard, fanoutCards, backCard, _discardCard);
    }

    public void AddToDiscardCard(CardContext greaterGreaterCardContext)
    {
        if (_discardCard == null)
        {
            _discardCard = greaterGreaterCardContext.GetCard();
        }
        else
        {
            AttachCard(greaterGreaterCardContext.GetCard(), _discardCard);
        }
    }

    public void AddSiblingsToGreaterCard(Card baseCard, CardContext greaterCardContext)
    {
        foreach (Card sibling in greaterCardContext.GetListOfReferencedCards())
        {
            if (sibling != baseCard)
            {
                AttachCard(sibling, greaterCardContext.GetCard());
            }
        }
    }




    #region Attach / Detach
    public void DetachCard(Card cardToRemove)
    {
        cardToRemove.GetParentCard().GetChildCards().Remove(cardToRemove);
        cardToRemove.SetParentCard(null);
        _topCardList.Add(cardToRemove);
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
