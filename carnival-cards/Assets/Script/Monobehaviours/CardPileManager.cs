using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileManager : MonoBehaviour
{
    public GameObject CardPilePrefab;
    public CardlikeManager CardLikeManager;

    public CardPile CreateCardPile(Card firstCard)
    {
        CardPile cardPile = Instantiate(CardPilePrefab, firstCard.transform.position, firstCard.transform.rotation).GetComponent<CardPile>();
        cardPile.Init(firstCard, CardLikeManager);

        CardLikeManager.AddCardlike(cardPile);

        return cardPile;
    }
}
