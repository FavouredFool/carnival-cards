using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject _cardPrefab;
    private CardlikeManager _cardlikeManager;

    public void Awake()
    {
        _cardlikeManager = GetComponent<CardlikeManager>();
    }

    public Card CreateCard(int counter)
    {
        Card newCard = Instantiate(_cardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();
        newCard.Init(counter);

        return newCard;
    }

    public void AddCardToCard(Card cardToAdd, Card baseCard)
    {
        foreach (Card interatedCard in _cardlikeManager.GetAllCards())
        {
            if (interatedCard.Equals(baseCard))
            {
                CardPile newCardPile = _cardlikeManager.GetCardPileManager().CreateCardPile(baseCard);
                newCardPile.AddCard(baseCard);
                newCardPile.AddCard(cardToAdd);

                _cardlikeManager.RemoveCardlike(baseCard);
                _cardlikeManager.RemoveCardlike(cardToAdd);

                return;
            }
        }
    }
}
