using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    private List<Card> _cardList;

    public void Awake()
    {
        _cardList = new List<Card>();
    }

    public void Update()
    {
        if (_cardList.Count <= 0)
        {
            Debug.LogWarning("ERROR: Pile empty");
        }
    }

    public void AddCard(Card card)
    {
        card.SetPile(this);
        _cardList.Add(card);

        SetCardTransformOnEntry(card);
    }

    public void AddPile(Pile pile)
    {
        List<Card> cardListCopy = new List<Card>(pile.GetCardList());

        foreach (Card card in cardListCopy)
        {
            AddCard(card);
        }
    }

    public void SetCardTransformOnEntry(Card card)
    {
        Debug.Assert(_cardList.Count > 0);

        card.transform.parent = this.transform;

        Vector3 cardLocalPosition = new Vector3(0f, 0.005f, 0f) * (_cardList.Count - 1);
        card.transform.localPosition = cardLocalPosition;
        card.transform.localRotation = Quaternion.identity;
    }

    public List<Card> GetCardList()
    {
        return _cardList;
    }

}
