using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : Cardlike
{
    private CardPile _cardPile;

    private CardPileManager _cardPileManager;

    private int _cardLabel;


    public void Awake()
    {
        _cardPile = null;
    }

    public void Init(CardPileManager cardPileManager, int cardLabel)
    {
        _cardPileManager = cardPileManager;
        _cardLabel = cardLabel;
    }

    public override void AddCardToCardPile(Card cardToAdd)
    {
        // SHOULD ONLY BE CALLED IF A CARD IS PUT ON TOP OF A CARD(PILE), NEVER IF A CARDPILE IS PUT ON A CARD(PILE)


        if (_cardPile == null)
        {
            _cardPile = _cardPileManager.CreateCardPile(this);
        }

        _cardPile.AddCard(cardToAdd);

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
