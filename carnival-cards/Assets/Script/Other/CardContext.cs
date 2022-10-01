using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CardTypeManager;


public class CardContext
{
    private Card _card;

    public CardContext ParentCardContext { get; set; }

    public string Name { get; set; }
    public List<CardContext> ReferencedCardContexts { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CardType Type { get; set; }

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
        return Name;
    }

    public Color GetColor()
    {
        return GetColorFromCardType(Type);
    }

    public CardContext GetRootCardContext()
    {
        CardContext parentContext = ParentCardContext;

        while (parentContext.ParentCardContext != null)
        {
            parentContext = parentContext.ParentCardContext;
        }

        return parentContext;
    }
}
