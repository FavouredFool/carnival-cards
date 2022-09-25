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
        RecalculateTransformForAllCards();

        if (_cardList.Count <= 0)
        {
            Debug.LogWarning("Pile Empty - will be removed");
        }
    }

    public void ClearCardList()
    {
        _cardList.Clear();
    }

    public void AddCardOnTop(Card card)
    {
        _cardList.Add(card);
    }

    public void AddPile(Pile pile)
    {
        List<Card> cardListCopy = new List<Card>(pile.GetCardList());
        AddCardList(cardListCopy);
    }

    public void AddCardList(List<Card> cardList)
    {
        foreach (Card card in cardList)
        {
            AddCardOnTop(card);
        }
    }

    public void ReverseCards()
    {
        GetCardList().Reverse();

        RecalculateTransformForAllCards();
    }

    public void RecalculateTransformForAllCards()
    {
        for (int i = 0; i < _cardList.Count; i++)
        {
            _cardList[i].transform.parent = transform;
            _cardList[i].transform.localPosition = new Vector3(0f, 0.005f, 0f) * (_cardList.IndexOf(_cardList[i]));
            _cardList[i].transform.localRotation = Quaternion.identity;
        }
    }

    public List<Card> GetCardList()
    {
        return _cardList;
    }
}
