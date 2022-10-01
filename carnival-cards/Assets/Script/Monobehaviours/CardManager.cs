using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public TextAsset _jsonText;

    public PileFactory _pileFactory;
    public CardFactory _cardFactory;

    private List<Pile> _pileList = new();

    private JsonReader jsonReader;

    private Card topCard;

    private void Start()
    {
        jsonReader = new JsonReader();
        
        CardContext rootCardContext = jsonReader.ReadJsonForCardContext(_jsonText);

        Card card = CreateAndAddCardsRecursive(null, rootCardContext, null);

        //card.SynchronizeHeight();

        topCard = card;

        MoveCardRandom(card);
    }


    private void Update()
    {
        topCard.SynchronizeHeight();
    }

    private Card CreateAndAddCardsRecursive(Card parentCard, CardContext cardContext, CardContext parentCardContext)
    {
        cardContext.ParentCardContext = parentCardContext;

        Card card = CreateCardAddToCard(parentCard, cardContext);

        List<Card> childCards = new();
        for (int i = 0; i < cardContext.ReferencedCardContexts.Count; i++)
        {
            childCards.Add(CreateAndAddCardsRecursive(card, cardContext.ReferencedCardContexts[i], cardContext));
        }

        card.SetChildCards(childCards);

        return card;
        
    }




    #region Create Stuff
    public Pile CreatePileWithCards(List<Card> cardList)
    {
        Pile newPile = CreatePile();
        newPile.AddCardListAtIndex(cardList, 0);
        return newPile;
    }

    public Card CreateCardAddToPile(Pile pile, CardContext cardContext)
    {
        Card newCard = CreateCard(cardContext);
        pile.AddCardOnTop(newCard);
        return newCard;
    }

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

    public Pile CreatePile()
    {
        Pile newPile = _pileFactory.CreateNewInstance();
        AddPileToList(newPile);
        return newPile;
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

    #region PileList
    public void AddPileToList(Pile pile)
    {
        _pileList.Add(pile);
    }

    public void RemovePileFromList(Pile pile)
    {
        _pileList.Remove(pile);
    }

    public List<Pile> GetAllPiles()
    {
        return _pileList;
    }
    #endregion

    #region SplitOperation
    public Pile SplitPileInHalf(Pile pile)
    {
        return SplitPileAtRange(pile, Mathf.CeilToInt(pile.GetCardList().Count / 2f), Mathf.FloorToInt(pile.GetCardList().Count / 2f));
    }

    public Pile SplitPileAtRange(Pile pile, int index, int cardCount)
    {
        Debug.Assert(cardCount > 0);
        Debug.Assert(0 <= index && index + cardCount - 1 < pile.GetCardList().Count);
        
        List<Card> currentPileCards = pile.GetCardList();
        List<Card> newCards = currentPileCards.GetRange(index, cardCount);

        List<Card> remainingCardList = currentPileCards.Except(newCards).ToList();

        //pile.GetCardList().Clear();
        //pile.AddCardListAtIndex(remainingCardList, pile.GetCardList().Count);
        pile.RemoveCardList(newCards);

        // Create new Pile
        Pile newPile = CreatePileWithCards(newCards);

        return newPile;
    }
    #endregion


    #region Add Piles
    public void AddPileToPileAtIndex(Pile pileToAdd, Pile pileBase, int index)
    {
        pileBase.AddCardListAtIndex(pileToAdd.GetCardList(), index);
        pileToAdd.RemoveAllCards();
    }
    #endregion

}
