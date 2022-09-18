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

    public void AddCardOnTop(Card card)
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
            AddCardOnTop(card);
        }
    }

    public void ReverseCards()
    {
        // Optional so it looks good in hierachie. You wouldn't see it in game because in game its all about height difference
        for (int i = 0; i < GetCardList().Count; i++)
        {
            transform.GetChild(0).SetSiblingIndex(GetCardList().Count - 1 - i);
        }

        GetCardList().Reverse();

        RecalculateTransformForAllCards();
    }

    public void SetCardTransformOnEntry(Card card)
    {
        Debug.Assert(_cardList.Count > 0);

        card.transform.parent = this.transform;

        Vector3 cardLocalPosition = new Vector3(0f, 0.005f, 0f) * (_cardList.Count - 1);
        card.transform.localPosition = cardLocalPosition;
        card.transform.localRotation = Quaternion.identity;
    }

    public void RecalculateTransformForAllCards()
    {
        // Cards should have a height according to their position in List

        for (int i = 0; i < _cardList.Count; i++)
        {
            _cardList[i].transform.localPosition = new Vector3(0f, 0.005f, 0f) * (_cardList.IndexOf(_cardList[i]));
        }
    }

    public List<Card> GetCardList()
    {
        return _cardList;
    }

}
