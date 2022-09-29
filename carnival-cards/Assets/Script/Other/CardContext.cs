using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CardTypeManager;


public class CardContext
{
    private Card _card;

    public string name { get; set; }
    public List<CardContext> referencedCards { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CardType type { get; set; }

    public void SetCard(Card card)
    {
        _card = card;
    }

    public Card GetCard()
    {
        return _card;
    }

    public string GetCardLabel()
    {
        return name;
    }

    public Color GetColor()
    {
        return GetColorFromCardType(type);
    }
}
