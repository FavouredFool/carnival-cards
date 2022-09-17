using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile : Cardlike
{
    private List<Card> _cardList;

    private CardlikeManager _cardlikeManager;



    public void Init(Card card, CardlikeManager cardlikeManager)
    {
        _cardlikeManager = cardlikeManager;
        _cardList = new List<Card>();
        AddCard(card);

        SetCardTransformOnEntry(card);
    }

    public void AddCard(Card card)
    {
        card.SetCardPile(this);
        _cardList.Add(card);

        // remove card out of CardlikeManager
        _cardlikeManager.RemoveCardlike(card);
        

        SetCardTransformOnEntry(card);
    }

    public void SetCardTransformOnEntry(Card card)
    {
        Debug.Assert(_cardList.Count > 0);

        // Set card as child of cardpile
        card.transform.parent = this.transform;

        Vector3 cardLocalPosition = new Vector3(0f, 0.005f, 0f) * (_cardList.Count - 1);
        card.transform.localPosition = cardLocalPosition;
        card.transform.localRotation = Quaternion.identity;
    }

    public override void AddCardToCardPile(Card cardToAdd)
    {
        // SHOULD ONLY BE CALLED IF A CARD IS PUT ON TOP OF A CARD(PILE), NEVER IF A CARDPILE IS PUT ON A CARD(PILE)

        AddCard(cardToAdd);

    }
}
