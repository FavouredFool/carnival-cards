using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Pile _pile;

    private CardContext _cardContext;


    public void Awake()
    {
        _pile = null;
    }

    public void Init(CardContext cardContext)
    {
        _cardContext = cardContext;
    }

    public void SetPile(Pile pile)
    {
        _pile = pile;
    }

    public Pile GetPile()
    {
        return _pile;
    }

    public string GetCardLabel()
    {
        return _cardContext.GetCardLabel();
    }
}
