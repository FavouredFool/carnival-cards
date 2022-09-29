using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFactory : GenericFactory<Card>
{

    public Card CreateNewInstance(CardContext cardContext)
    {
        Card newCard = base.CreateNewInstance();

        newCard.Init(cardContext);
        cardContext.SetCard(newCard);
        newCard.GetComponent<MeshRenderer>().material.color = cardContext.GetColor();

        return newCard;

    }
}
