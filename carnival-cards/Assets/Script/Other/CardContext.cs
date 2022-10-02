using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CardTypeManager;


public class CardContext
{
    private Card _card;

    private List<int> _identifier;

    private CardContext _parentContext;

    public string Name { get; set; }
    public List<CardContext> ReferencedCardContexts { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CardType Type { get; set; }

    public void InitCardContextRecursive(CardContext parentCardContext, List<int> upperIdentifier, int index)
    {
        SetParentContext(parentCardContext);

        List<int> identifier = new();
        identifier.AddRange(upperIdentifier);
        identifier.Add(index);
        SetIdentifier(identifier);

        for (int i = 0; i < ReferencedCardContexts.Count; i++)
        {
            ReferencedCardContexts[i].InitCardContextRecursive(this, identifier, i);
        }

    }

    public CardContext GetParentContext()
    {
        return _parentContext;
    }

    public void SetParentContext(CardContext parentContext)
    {
        _parentContext = parentContext;
    }

    public void SetIdentifier(List<int> identifier)
    {
        _identifier = identifier;
    }

    public List<int> GetIdentifier()
    {
        return _identifier;
    }

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

    public List<Card> GetListOfReferencedCards()
    {
        List<Card> listOfCards = new();

        foreach (CardContext context in ReferencedCardContexts)
        {
            listOfCards.Add(context.GetCard());
        }

        return listOfCards;
    }

}
