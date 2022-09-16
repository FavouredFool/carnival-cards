using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile : Cardlike
{
    private List<Card> _cardList;

    public void Start()
    {
        
    }
    public void Init(Card card)
    {
        _cardList = new List<Card>() { card };

        SetCardTransformOnEntry(card);
    }

    public void AddCard(Card card)
    {
        _cardList.Add(card);

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
}
