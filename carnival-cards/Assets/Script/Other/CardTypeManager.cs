using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardTypeManager
{
    public enum CardType { COVER, FLAVOR, PLACE, THING, ITEM, PERSON, INFO, SENSE, INVESTIGATION }

    public static Color GetColorFromCardType(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.COVER:
                return Color.black;
            case CardType.FLAVOR:
                return Color.gray;
            case CardType.PLACE:
                return Color.green;
            case CardType.THING:
                return Color.blue;
            case CardType.ITEM:
                return Color.red;
            case CardType.PERSON:
                return Color.red;
            case CardType.INFO:
                return Color.white;
            case CardType.SENSE:
                return Color.magenta;
            case CardType.INVESTIGATION:
                return Color.yellow;
            default:
                Debug.LogWarning("ERROR");
                break;
        }
        return Color.white;
    }
}
