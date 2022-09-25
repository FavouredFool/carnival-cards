using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardTypeManager;

public class CardContext
{
    private readonly string _name;
    private readonly int _number;
    private readonly CardType _cardType;

    public CardContext(string name, int number, CardType cardType)
    {
        _name = name;
        _number = number;
        _cardType = cardType;
    }

    public string GetCardLabel()
    {
        return _name + "\n" + _number;
    }

    public CardType GetCardType()
    {
        return _cardType;
    }

    public Color GetColor()
    {
        return GetColorFromCardType(_cardType);
    }
}
