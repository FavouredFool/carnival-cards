using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Cardlike
{
    private CardPile _cardPile;

    private int _cardLabel;


    public void Awake()
    {
        _cardPile = null;
    }

    public void Init(int cardLabel)
    {
        _cardLabel = cardLabel;
    }

    public int GetCardLabel()
    {
        return _cardLabel;
    }

    public void SetCardPile(CardPile cardPile)
    {
        _cardPile = cardPile;
    }

    public CardPile GetCardPile()
    {
        return _cardPile;
    }
}
