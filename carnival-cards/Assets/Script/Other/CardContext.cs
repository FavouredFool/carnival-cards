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

    private IOnClickAction _onClickAction;


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

    public CardContext FindCardContextFromCard(Card card)
    {
        if (GetCard() == card)
        {
            return this;
        }

        foreach (CardContext cardContext in ChildCardContexts)
        {
            CardContext result = cardContext.FindCardContextFromCard(card);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public void AttachCardAtEnd(CardContext cardContextToAttach)
    {
        if (!cardContextToAttach.IsDeeperEqual(this))
        {
            Debug.Log("FEHLER: BASIS ZU TIEF");
            return;
        }

        AttachCardRecursive(cardContextToAttach, GetIdentifier().Count);
    }

    private void AttachCardRecursive(CardContext cardContextToAttach, int depth)
    {
        List<int> cardToAttachIdentifer = cardContextToAttach.GetIdentifier();

        if (cardToAttachIdentifer.Count == depth)
        {
            Debug.LogWarning("FEHLER: IDENTIFIER VORBEI");
        }

        for (int i = 0; i < cardContextToAttach.ChildCardContexts.Count; i++)
        {
            if (cardContextToAttach.ChildCardContexts[i].GetIdentifier()[depth] == cardToAttachIdentifer[depth])
            {
                //Found correct card, go deeper if identifier is not fully traversed

                // Is this card child of parent? -> if not, attach
                if (!cardContextToAttach.ChildCardContexts[i].GetCard().GetIsAttached())
                {
                    GetCard().AttachCardDirectly(cardContextToAttach.ChildCardContexts[i].GetCard());

                }
                else
                {
                    cardContextToAttach.ChildCardContexts[i].AttachCardRecursive(cardContextToAttach, ++depth);
                }

                break;

            }
        }
    }

    public void SynchronizeHeight()
    {
        int startHeight = GetPileAmount() - 1;
        SetHeightRecursive(0);

        // Hier alles nach oben verschieben weil bisher nach unten gebaut wurde
        GetCard().SetHeight(startHeight);
    }

    private int SetHeightRecursive(int height)
    {
        GetCard().SetHeight(height);

        height = 0;
        foreach (CardContext card in ChildCardContexts)
        {
            if (card.GetCard().GetIsAttached())
            {
                height += -1;
                height += card.SetHeightRecursive(height);
            }
        }

        return height;
    }

    public int GetPileAmount()
    {
        return AddPileAmountRecursive(0);
    }

    private int AddPileAmountRecursive(int counter)
    {
        counter += 1;

        foreach (CardContext context in ChildCardContexts)
        {
            if (context.GetCard().GetIsAttached())
            {
                counter = context.AddPileAmountRecursive(counter);
            }
        }

        return counter;
    }


    public int GetCardAmount()
    {
        return AddCardAmountRecursive(0);
    }

    private int AddCardAmountRecursive(int counter)
    {
        counter += 1;
        foreach (CardContext context in ChildCardContexts)
        {
            counter = context.AddCardAmountRecursive(counter);
        }

        return counter;
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

    public CardContext GetNextNotAttachedContext()
    {
        CardContext activeContext = this;

        while (activeContext.GetParentContext() != null && activeContext.GetCard().GetIsAttached())
        {
            activeContext = activeContext.GetParentContext();
        }

        return activeContext;
    }

    public void OnClickAction(CardManager cardManager)
    {
        _onClickAction.OnClick(cardManager, this);
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

    public void SetOnClickAction(IOnClickAction onClickAction)
    {
        _onClickAction = onClickAction;
    }

    public IOnClickAction GetOnClickAction()
    {
        return _onClickAction;
    }



}
