using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardTypeManager;

public static class ExampleCardContexts
{
    private static List<CardContext> _cardContexts = new()
    {
        new CardContext("The Surrounding Room", 0, CardType.PLACE),
        new CardContext("Subroom1", 1, CardType.PLACE),
        new CardContext("Subroom2", 2, CardType.PLACE),
        new CardContext("Subroom3", 3, CardType.PLACE),
        new CardContext("Subroom4", 4, CardType.PLACE),
    };

    private static int _counter = _cardContexts.Count - 1;

    public static CardContext GetCardContext()
    {
        _counter %= _cardContexts.Count;
        _counter = (_counter < 0) ? _cardContexts.Count - 1 : _counter;

        return _cardContexts[_counter--];
    }

    public static int GetTotalCards()
    {
        return _cardContexts.Count;
    }
}
