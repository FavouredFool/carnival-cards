using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardPile _cardPile;

    public void Start()
    {
        _cardPile = null;
    }

    public void Update()
    {
       
    }

    public void AddCardToCardPile(Card cardToAdd)
    {
        // SHOULD ONLY BE CALLED IF A CARD IS PUT ON TOP OF A CARD(PILE), NEVER IF A CARDPILE IS PUT ON A CARD(PILE)


        if (_cardPile == null)
        {
            _cardPile = CreateCardPile();
        }

        _cardPile.AddCard(cardToAdd);

    }

    private CardPile CreateCardPile()
    {
        return new CardPile(this);
    }
}
