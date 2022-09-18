using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject _cardPrefab;

    int counter = 0;

    public Card CreateCard()
    {
        Card newCard = Instantiate(_cardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();
        newCard.Init(counter);

        counter++;

        return newCard;
    }

}
