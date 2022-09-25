using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public PileFactory _pileFactory;
    public CardFactory _cardFactory;

    protected List<Pile> _pileList;

    public void Start()
    {
        _pileList = new();
    }

    public void AddPileToPile(Pile pileToAdd, Pile pileBase)
    {
        pileBase.AddPile(pileToAdd);
        RemovePileFromList(pileToAdd);
        Destroy(pileToAdd.gameObject);
    }

    public Pile CreateCardAndPile()
    {
        Card newCard = CreateCard();

        return CreatePileWithCard(newCard);
    }

    public Pile CreatePileWithCard(Card firstCard)
    {
        Pile newPile = CreatePile();
        newPile.AddCardOnTop(firstCard);
        return newPile;
    }

    public Pile CreatePileWithCards(List<Card> cardList)
    {
        Pile newPile = CreatePile();
        newPile.AddCardList(cardList);
        return newPile;
    }

    public void CreateCardAddToPile(Pile pile)
    {
        Card newCard = CreateCard();
        pile.AddCardOnTop(newCard);
    }

    public Pile CreatePile()
    {
        Pile newPile = _pileFactory.CreateNewInstance();
        AddPileToList(newPile);
        return newPile;
    }

    public Card CreateCard()
    {
        CardContext cardContext = ExampleCardContexts.GetCardContext();
        return _cardFactory.CreateNewInstance(cardContext);
    }

    public Pile SplitPileInHalf(Pile pile)
    {
        return SplitPileAtIndex(pile, Mathf.CeilToInt(pile.GetCardList().Count / 2f));
    }

    public Pile SplitPileAtIndex(Pile pile, int index)
    {
        if (pile.GetCardList().Count < 2)
        {
            return pile;
        }

        Pile newPile = CreatePile();
        List<Card> pileCards = pile.GetCardList();

        for (int i = pileCards.Count - 1; i >= index; i--)
        {
            newPile.AddCardOnTop(pileCards[i]);
            pileCards.RemoveAt(i);
        }

        newPile.GetCardList().Reverse();

        return newPile;
    }

    public Pile SplitPileAtRange(Pile pile, int index, int cardCount)
    {
        Debug.Assert(cardCount > 0);
        Debug.Assert(0 <= index && index + cardCount - 1 < pile.GetCardList().Count);
        
        List<Card> currentPileCards = pile.GetCardList();
        List<Card> newCards = currentPileCards.GetRange(index, cardCount);

        List<Card> remainingCardList = currentPileCards.Except(newCards).ToList();

        pile.GetCardList().Clear();
        pile.AddCardList(remainingCardList);

        // Create new Pile
        Pile newPile = CreatePileWithCards(newCards);

        return newPile;
    }

    public void MovePile(Pile pile, Vector2 newPosition)
    {
        pile.transform.position = new Vector3(newPosition.x, pile.transform.position.y, newPosition.y);
    }

    public void MovePileRandom(Pile pile)
    {
        MovePile(pile, Random.insideUnitCircle * 3);
    }

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
        // Remove Empty Piles when necessary

        List<Pile> pileListCopy = new(_pileList);

        foreach (Pile pile in pileListCopy)
        {
            if (pile.GetCardList().Count <= 0)
            {
                _pileList.Remove(pile);
                Destroy(pile.gameObject);
            }
        }

        return _pileList;
    }
}
