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
        for (int i = 0; i < cardContext.ReferencedCardContexts.Count; i++)
        {
            childCards.Add(CreateAndAddCardsRecursive(card, cardContext.ReferencedCardContexts[i]));
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

        foreach (Card activeCard in childCardsCopy)
        {
            activeCard.GetParentCard().GetChildCards().Remove(activeCard);
            activeCard.SetParentCard(null);
            _topCardList.Add(activeCard);
        }

        MoveCardToHighlightPos(card);

        FanOutCardListAtPos(card.GetCardContext().GetListOfReferencedCards());

        if (card.GetCardContext().GetParentContext() != null)
        {
            AddSiblingsToParent(card);

            PutCardInStepOutPos(card.GetCardContext().GetParentContext().GetCard());

            if (card.GetCardContext().GetParentContext().GetParentContext() != null)
            {
                // Add to discard Card
                if (_discardCard == null)
                {
                    _discardCard = card.GetCardContext().GetParentContext().GetParentContext().GetCard();
                }
                else
                {
                    // 1. remove from topCard list
                    // 2. Add newCard parent
                    // 3. Add parentCard child
                    Card addToDiscardCard = card.GetCardContext().GetParentContext().GetParentContext().GetCard();
                    _topCardList.Remove(addToDiscardCard);
                    addToDiscardCard.SetParentCard(_discardCard);
                    _discardCard.GetChildCards().Add(addToDiscardCard);

                }
            }

            //PutUnusedCardOnDiscardPos(MakeDiscardCard(card.GetCardContext().GetParentContext()));
        }

        if (_discardCard != null)
        {
            _discardCard.transform.position = new Vector3(5f, card.transform.position.y, 3f);
        }
        

    }

    public void AddSiblingsToParent(Card card)
    {
        foreach (Card sibling in card.GetCardContext().GetParentContext().GetListOfReferencedCards())
        {
            if (sibling != card)
            {
                sibling.SetParentCard(card.GetCardContext().GetParentContext().GetCard());
                card.GetCardContext().GetParentContext().GetCard().GetChildCards().Add(sibling);
                _topCardList.Remove(sibling);
            }
        }
    }

    public void MoveCardToHighlightPos(Card card)
    {
        card.transform.position = new Vector3(-5f, card.transform.position.y, 0f);
    }

    public void FanOutCardListAtPos(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.position = new Vector3(2f * i, cardList[i].transform.position.y, 0f);
        }
    }

    public void PutCardInStepOutPos(Card card)
    {

        card.transform.position = new Vector3(-5f, card.transform.position.y, 3f);
    }
    
    public Card MakeDiscardCard(CardContext excludeCardsWithDeeperContext)
    {
        List<Card> unusedCards = new();

        foreach (Card card in _topCardList)
        {
            // is this card being used on the field?
            // if not, add it to unusedCards and sort these afterwards

            if (!card.GetCardContext().IsDeeperEqual(excludeCardsWithDeeperContext))
            {
                // Add to List
                unusedCards.Add(card);
            }
        }

        return CombineCardListToCard(unusedCards);
    }

    public void SortCardListByContext(List<Card> cardList)
    {

    }

    public Card CombineCardListToCard(List<Card> cardList)
    {
        SortCardListByContext(cardList);


        for (int i = 1; i < cardList.Count; i++)
        {
            //cardList[0].AddCard(cardList[i]);
        }


        return null;
    }
    

    public void PutUnusedCardOnDiscardPos(Card discardCard)
    {

    }

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
