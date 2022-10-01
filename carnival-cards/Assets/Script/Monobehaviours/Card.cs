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
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.parent = null;
        }
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
