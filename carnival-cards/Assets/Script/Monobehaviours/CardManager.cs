using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardFactory _cardFactory;

    int counter = 0;

    public Card CreateCard()
    {
        Card newCard = _cardFactory.CreateNewInstance();
        newCard.Init(counter);

        counter++;

        return newCard;
    }

}
