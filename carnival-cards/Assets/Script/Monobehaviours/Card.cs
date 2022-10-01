using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private CardContext _cardContext;

    private List<Card> _childCards;

    private Card _parentCard;

    public void Update()
    {
        SynchronizeVisual();

    }

    public void Init(CardContext cardContext)
    {
        _cardContext = cardContext;
    }

    public void SynchronizeVisual()
    {
        if (_parentCard != null)
        {
            transform.parent = _parentCard.transform;
            transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
            
        }
        else
        {
            transform.parent = null;
        }
    }

    public int GetCardAmount()
    {
        return AddCardAmountRecursive(0);
    }

    private int AddCardAmountRecursive(int counter)
    {
        counter += 1;
        foreach (Card card in _childCards)
        {
            counter = card.AddCardAmountRecursive(counter);
        }

        return counter;
    }

    public void SynchronizeHeight()
    {
        int startHeight = GetCardAmount() - 1;
        SetHeightRecursive(0);

        // Hier alles nach oben verschieben
        transform.position = new Vector3(transform.position.x, 0.005f * startHeight, transform.position.z);
    }

    private int SetHeightRecursive(int height)
    {
        transform.localPosition = new Vector3(transform.position.x, 0.005f * height, transform.position.z);
       
        height = 0;
        foreach (Card card in _childCards)
        {
            
            height += -1;
            height += card.SetHeightRecursive(height);
        }

        return height;
    }

    public Card GetRootCard()
    {
        Card parentCard = this;

        while (parentCard._parentCard != null)
        {
            parentCard = parentCard._parentCard;
        }

        return parentCard;
    }

    public string GetCardLabel()
    {
        return _cardContext.GetCardLabel();
    }

    public CardContext GetCardContext()
    {
        return _cardContext;
    }

    public Card GetParentCard()
    {
        return _parentCard;
    }

    public void SetParentCard(Card parentCard)
    {
        _parentCard = parentCard;
    }

    public void SetChildCards(List<Card> childCards)
    {
        _childCards = childCards;
    }

    public List<Card> GetChildCards()
    {
        return _childCards;
    }
}
