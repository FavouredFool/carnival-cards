using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CardlikeManager _cardlikeManager;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            List<Cardlike> cardlikeList = _cardlikeManager.GetAllCardlikes();

            Card newCard = _cardlikeManager.CreateCard();

            if (newCard == cardlikeList[0])
            {
                return;
            }

            _cardlikeManager.AddCardToCardlike(newCard, cardlikeList[^2]);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Set card on the ground to the right of pile
        }
    }
}
