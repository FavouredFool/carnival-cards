using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static CardTypeManager;


public class Context
{
    [JsonConverter(typeof(StringEnumConverter))]
    public CardType Type { get; set; }

    public string Name { get; set; }

    public List<Context> ChildContexts { get; set; }


    private Card _card;

    private List<int> _identifier;

    private Context _parentContext;

    private IOnClickAction _onClickAction;

    public Context(string name, CardType type)
    {
        Name = name;
        Type = type;
        ChildContexts = new();
        _parentContext = null;
        _onClickAction = null;
        _identifier = new() { 0 };
    }

    public void InitContextRecursive(Context parentContext, List<int> upperIdentifier, int index)
    {
        SetParentContext(parentContext);

        List<int> identifier = new();
        identifier.AddRange(upperIdentifier);
        identifier.Add(index);
        SetIdentifier(identifier);

        for (int i = 0; i < ChildContexts.Count; i++)
        {
            ChildContexts[i].InitContextRecursive(this, identifier, i);
        }

    }

    public Context FindContextFromCard(Card card)
    {
        if (GetCard() == card)
        {
            return this;
        }

        foreach (Context context in ChildContexts)
        {
            Context result = context.FindContextFromCard(card);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public void AttachCardAtEnd(Context contextToAttach)
    {
        if (!contextToAttach.IsDeeperEqual(this))
        {
            Debug.Log("FEHLER: BASIS ZU TIEF");
            return;
        }
        // NOT TESTED ATM
        AttachCardRecursive(contextToAttach, GetIdentifier().Count);
    }

    private void AttachCardRecursive(Context contextToAttach, int depth)
    {
        List<int> cardToAttachIdentifer = contextToAttach.GetIdentifier();

        if (cardToAttachIdentifer.Count == depth)
        {
            Debug.LogWarning("FEHLER: IDENTIFIER VORBEI");
        }

        for (int i = 0; i < contextToAttach.ChildContexts.Count; i++)
        {
            if (contextToAttach.ChildContexts[i].GetIdentifier()[depth] == cardToAttachIdentifer[depth])
            {
                //Found correct card, go deeper if identifier is not fully traversed

                // Is this card child of parent? -> if not, attach
                if (!contextToAttach.ChildContexts[i].GetCard().GetIsAttached())
                {
                    GetCard().AttachCardDirectly(contextToAttach.ChildContexts[i].GetCard());
                }
                else
                {
                    contextToAttach.ChildContexts[i].AttachCardRecursive(contextToAttach, ++depth);
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
        foreach (Context card in ChildContexts)
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

        foreach (Context context in ChildContexts)
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
        foreach (Context context in ChildContexts)
        {
            counter = context.AddCardAmountRecursive(counter);
        }

        return counter;
    }

    public bool IsDeeperEqual(Context cardToCompare)
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

        foreach (Context context in ChildContexts)
        {
            listOfCards.Add(context.GetCard());
        }

        return listOfCards;
    }

    public Context GetNextNotAttachedContext()
    {
        Context activeContext = this;

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

    public Context GetParentContext()
    {
        return _parentContext;
    }

    public void SetParentContext(Context parentContext)
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
