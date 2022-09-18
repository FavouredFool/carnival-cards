using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : GenericFactory<Card>
{

    int counter = 0;

    public override Card CreateNewInstance()
    {
        Card newCard = base.CreateNewInstance();
        newCard.Init(++counter);

        return newCard;

    }
}
