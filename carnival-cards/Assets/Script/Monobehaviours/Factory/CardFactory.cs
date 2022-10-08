using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : GenericFactory<Card>
{

    public Card CreateNewInstance(Context context)
    {
        Card newCard = base.CreateNewInstance();
        newCard.name = context.Name;

        context.SetCard(newCard);
        newCard.GetComponent<MeshRenderer>().material.color = context.GetColor();

        return newCard;

    }
}
