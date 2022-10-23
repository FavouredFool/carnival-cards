using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardTypeManager
{
    public enum CardType { COVER, FLAVOR, PLACE, THING, ITEM, INVESTIGATION, INVENTORY, LOCK }

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
            case CardType.INVESTIGATION:
                return Color.yellow;
            case CardType.INVENTORY:
                return Color.magenta;
            case CardType.LOCK:
                return Color.cyan;
            default:
                Debug.LogWarning("ERROR");
                break;
        }
        return Color.white;
    }
}
