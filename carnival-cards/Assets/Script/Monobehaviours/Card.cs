using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardContext _cardContext;

    public void Init(CardContext cardContext)
    {
        _cardContext = cardContext;
    }

    public string GetCardLabel()
    {
        return _cardContext.GetCardLabel();
    }
}
