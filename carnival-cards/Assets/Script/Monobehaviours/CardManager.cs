using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public TextAsset _jsonText;
    public CardFactory _cardFactory;

    private List<Card> _topCardList = new();
    private Card _discardCard = null;

    private JsonReader jsonReader;

    private Vector2 _highlightPos = new Vector2(-5f, 0f);
    private Vector2 _stepOutPos = new Vector2(-5f, 3f);


    private void Start()
    {
        jsonReader = new JsonReader();
        
        CardContext rootCardContext = jsonReader.ReadJsonForCardContext(_jsonText);
        rootCardContext.InitCardContextRecursive(null, new List<int>(), 0);
        Card card = CreateAndAddCardsRecursive(null, rootCardContext);

        _topCardList.Add(card);

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



    public void DisplayReferencedCards(Card card)
    {
        List<Card> childCardsCopy = new(card.GetChildCards());
        CardContext greaterCardContext = card.GetCardContext().GetParentContext();

        foreach (Card activeCard in childCardsCopy)
        {
            DetachCard(activeCard);
        }

        MoveCard(card, _highlightPos);
        FanOutCardListAtPos(card.GetCardContext().GetListOfReferencedCards());

        if (greaterCardContext != null)
        {
            AddSiblingsToGreaterCard(card, greaterCardContext);
            MoveCard(greaterCardContext.GetCard(), _stepOutPos);

            CardContext greaterGreaterCardContext = greaterCardContext.GetParentContext();

            if (greaterGreaterCardContext != null)
            {
                AddToDiscardCard(greaterGreaterCardContext);
            }

        }

        if (_discardCard != null)
        {
            _discardCard.transform.position = new Vector3(5f, card.transform.position.y, 3f);
        }
    }

    public void AddToDiscardCard(CardContext greaterGreaterCardContext)
    {
        // Add to discard Card
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

    public void FanOutCardListAtPos(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            MoveCard(cardList[i], new Vector2(2f * i, 0f));
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

        // Find correct position in baseCard-Hierachie

        Debug.Log("base: " + baseCard.name);
        Debug.Log("attach: " + cardToAttach.name);

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
        

        // TODO: HEIGHT SETZEN
        //newCard.transform.localPosition = new Vector3(0, 0.05f * index, 0);
        return newCard;
    }

    public Card CreateCard(CardContext cardContext)
    {
        return _cardFactory.CreateNewInstance(cardContext);
    }
    #endregion

    #region Move Card
    public void MoveCard(Card card, Vector2 newPosition)
    {
        card.transform.position = new Vector3(newPosition.x, card.transform.position.y, newPosition.y);
    }

    public void MoveCardRandom(Card card)
    {
        MoveCard(card, Random.insideUnitCircle * 3);
    }
    #endregion


}
