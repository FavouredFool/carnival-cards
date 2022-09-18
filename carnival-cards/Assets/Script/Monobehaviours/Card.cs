using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Pile _pile;

    private int _cardLabel;


    public void Awake()
    {
        _pile = null;
    }

    public void Init(int cardLabel)
    {
        _cardLabel = cardLabel;
    }

    public int GetCardLabel()
    {
        return _cardLabel;
    }

    public void SetPile(Pile pile)
    {
        _pile = pile;
    }

    public Pile GetPile()
    {
        return _pile;
    }
}
