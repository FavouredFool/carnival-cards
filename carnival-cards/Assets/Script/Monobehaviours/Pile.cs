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

    public void SynchronizeVisual()
    {
        for (int i = 0; i < _cardList.Count; i++)
        {
            _cardList[i].transform.parent = transform;
            _cardList[i].transform.localPosition = new Vector3(0f, 0.005f, 0f) * (_cardList.IndexOf(_cardList[i]));
            _cardList[i].transform.localRotation = Quaternion.identity;
        }

        if (_cardList.Count <= 0)
        {
            Debug.LogWarning("Pile Empty - will be removed");
        }
    }

    public void AddCardOnTop(Card card)
    {
        AddCardAtIndex(card, GetCardList().Count); 
    }

    public void AddCardAtIndex(Card card, int index)
    {
        if (index == GetCardList().Count)
        {
            _cardList.Add(card);
        } 
        else
        {
            _cardList.Insert(index, card);
        }

        SynchronizeVisual();
    }

    public void RemoveCardAtIndex(Card card, int index)
    {
        _cardList.RemoveAt(index);

        SynchronizeVisual();
    }

    public void RemoveCard(Card card)
    {
        _cardList.Remove(card);
    }

    public void AddPileAtIndex(Pile pile, int index)
    {
        List<Card> cardListCopy = new List<Card>(pile.GetCardList());
        AddCardListAtIndex(cardListCopy, index);
    }

    public void AddCardListAtIndex(List<Card> cardList, int index)
    {
        cardList.Reverse();

        foreach (Card card in cardList)
        {
            AddCardAtIndex(card, index);
        }
    }

    public void RemoveCardList(List<Card> cardList)
    {
        foreach(Card card in cardList)
        {
            RemoveCard(card);
        }
    }

    public List<Card> GetCardList()
    {
        return _cardList;
    }
}
