using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileManager : MonoBehaviour
{
    public GameObject _cardPilePrefab;
    private CardlikeManager _cardlikeManager;

    public void Awake()
    {
        _cardlikeManager = GetComponent<CardlikeManager>();
    }

    public CardPile CreateCardPile(Card firstCard)
    {
        CardPile newPile = Instantiate(_cardPilePrefab, firstCard.transform.position, firstCard.transform.rotation).GetComponent<CardPile>();
        _cardlikeManager.AddCardlike(newPile);
        return newPile;
    }

    public void AddCardToCardPile(Card card, CardPile baseCardPile)
    {
        foreach (CardPile pile in _cardlikeManager.GetAllCardPiles())
        {
            if (pile.Equals(baseCardPile))
            {
                pile.AddCard(card);
                _cardlikeManager.RemoveCardlike(card);
                return;
            }
        }
    }
}
