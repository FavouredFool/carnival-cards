using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardlikeManager : MonoBehaviour
{
    private CardPileManager _cardPileManager;
    private CardManager _cardManager;

    protected readonly List<Cardlike> _cardlikeList = new();

    protected int counter = 0;

    void Awake()
    {
        _cardPileManager = GetComponent<CardPileManager>();
        _cardManager = GetComponent<CardManager>();
    }

    public virtual void AddCardToCardlike(Card card, Cardlike baseCardlike)
    {
        if (baseCardlike is Card baseCard)
        {
            _cardManager.AddCardToCard(card, baseCard);
        }
        else if (baseCardlike is CardPile baseCardPile)
        {
            _cardPileManager.AddCardToCardPile(card, baseCardPile);
        }
        else
        {
            Debug.LogWarning("FEHLER");
        }
        
    }

    public Card CreateCard()
    {
        Card newCard = _cardManager.CreateCard(counter);

        AddCardlike(newCard);

        counter++;

        return newCard;
    }

    public void AddCardlike(Cardlike cardlike)
    {
        _cardlikeList.Add(cardlike);
    }

    public void RemoveCardlike(Cardlike cardlike)
    {
        _cardlikeList.Remove(cardlike);
    }

    public List<CardPile> GetAllCardPiles()
    {
        return _cardlikeList.Where(e => e is CardPile).Cast<CardPile>().ToList();
    }

    public List<Card> GetAllCards()
    {
        return _cardlikeList.Where(e => e is Card).Cast<Card>().ToList();
    }

    public List<Cardlike> GetAllCardlikes()
    {
        return _cardlikeList;
    }

    public CardManager GetCardManager()
    {
        return _cardManager;
    }

    public CardPileManager GetCardPileManager()
    {
        return _cardPileManager;
    }
}
