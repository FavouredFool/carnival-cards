using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PileManager : MonoBehaviour
{
    public GameObject _pilePrefab;

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

        return CreatePile(newCard);
    }

    public void CreateCardAddToPile()
    {

    }

    public Pile CreatePile(Card firstCard)
    {
        Pile newPile = Instantiate(_pilePrefab).GetComponent<Pile>();
        newPile.AddCard(firstCard);
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
