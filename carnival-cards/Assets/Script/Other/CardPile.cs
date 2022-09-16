using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPile
{
    private List<Card> _cardList;
    private Vector3 _position;

    public CardPile(Card card)
    {
        _cardList = new List<Card>() { card };
        _position = card.transform.position;

        SetCardTransformUniform(card);
    }

    public void AddCard(Card card)
    {
        _cardList.Add(card);

        SetCardTransformUniform(card);
    }

    public void SetCardTransformUniform(Card card)
    {
        Debug.Assert(_cardList.Count > 0);

        card.transform.SetPositionAndRotation(_position + new Vector3(0f, 0.005f, 0f) * (_cardList.Count-1), Quaternion.identity);
    }
}
