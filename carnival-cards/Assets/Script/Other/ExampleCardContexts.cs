using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardTypeManager;

public static class ExampleCardContexts
{
    private static int _counter = 0;

    private static List<CardContext> _cardContexts = new()
    {
        new CardContext("The Cover", 0, CardType.COVER),
        new CardContext("Story", 1, CardType.STORY),
        new CardContext("More Story", 2, CardType.STORY),
        new CardContext("Even more Story", 3, CardType.STORY),
        new CardContext("Some Place", 4, CardType.PLACE),
    };


    public static CardContext GetCardContext()
    {
        _counter %= _cardContexts.Count;
        return _cardContexts[_counter++];
    }
}
