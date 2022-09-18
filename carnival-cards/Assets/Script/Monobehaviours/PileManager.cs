using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PileManager : MonoBehaviour
{
    public PileFactory _pileFactory;

    private CardManager _cardManager;
    
    protected readonly List<Pile> _pileList = new();

    void Awake()
    {
        _cardManager = GetComponent<CardManager>();
    }

    public void AddPileToPile(Pile pileToAdd, Pile pileBase)
    {
        pileBase.AddPile(pileToAdd);
        RemovePileFromList(pileToAdd);
        Destroy(pileToAdd.gameObject);
    }

    public Pile CreateCardAndPile()
    {
        Card newCard = _cardManager.CreateCard();

        return CreatePileWithCard(newCard);
    }

    public void CreateCardAddToPile(Pile pile)
    {
        Card newCard = _cardManager.CreateCard();
        pile.AddCardOnTop(newCard);
    }

    public Pile SplitPileinHalf(Pile pile)
    {
        return SplitPileAtIndex(pile, Mathf.CeilToInt(pile.GetCardList().Count / 2f));
    }

    public Pile SplitPileAtIndex(Pile pile, int index)
    {
        Pile newPile = CreatePile();
        List<Card> pileList = pile.GetCardList();

        for (int i = pileList.Count - 1; i >= index; i--)
        {
            newPile.AddCardOnTop(pileList[i]);
            pileList.RemoveAt(i);
        }
        newPile.ReverseCards();

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

    public Pile CreatePileWithCard(Card firstCard)
    {
        Pile newPile = CreatePile();
        newPile.AddCardOnTop(firstCard);
        return newPile;
    }

    public Pile CreatePile()
    {
        Pile newPile = _pileFactory.CreateNewInstance();
        AddPileToList(newPile);
        return newPile;
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
        return _pileList;
    }
}
