using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CardTypeManager;


public class CardContext
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CardType Type { get; set; }

    public string Name { get; set; }

    public List<CardContext> ChildCardContexts { get; set; }



    private Card _card;

    private List<int> _identifier;

    private CardContext _parentContext;




    public void InitCardContextRecursive(CardContext parentCardContext, List<int> upperIdentifier, int index)
    {
        SetParentContext(parentCardContext);

        List<int> identifier = new();
        identifier.AddRange(upperIdentifier);
        identifier.Add(index);
        SetIdentifier(identifier);

        for (int i = 0; i < ChildCardContexts.Count; i++)
        {
            ChildCardContexts[i].InitCardContextRecursive(this, identifier, i);
        }

    }

    public bool IsDeeperEqual(CardContext cardToCompare)
    {
        // wenn this deeper als cardToCompare -> true

        if (this == cardToCompare)
        {
            return true;
        }

        for (int i = 0; i < cardToCompare.GetIdentifier().Count; i++)
        {
            if (GetIdentifier().Count == i)
            {
                return false;
            }

            if (cardToCompare.GetIdentifier()[i] != GetIdentifier()[i])
            {
                return false;
            }

        }

        return true;
    }

    public List<Card> GetListOfReferencedCards()
    {
        List<Card> listOfCards = new();

        foreach (CardContext context in ChildCardContexts)
        {
            listOfCards.Add(context.GetCard());
        }

        return listOfCards;
    }

    public CardContext GetRootContext()
    {
        CardContext parentContext = this;

        while (parentContext.GetParentContext() != null)
        {
            parentContext = parentContext.GetParentContext();
        }

        return parentContext;
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



}
