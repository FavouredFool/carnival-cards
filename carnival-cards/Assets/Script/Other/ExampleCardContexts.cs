using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardTypeManager;

public static class ExampleCardContexts
{
    private static List<CardContext> _cardContexts = new()
    {
        new CardContext("The Cover", 0, CardType.COVER),
        new CardContext("Story", 1, CardType.STORY),
        new CardContext("More Story", 2, CardType.STORY),
        new CardContext("Even more Story", 3, CardType.STORY),
        new CardContext("Some Place", 4, CardType.PLACE),
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
