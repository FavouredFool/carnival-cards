using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTestManager : MonoBehaviour
{

    public GameObject CardPrefab;

    public Material cardMat1;
    public Material cardMat2;

    private Card _startCard;

    private int counter = 0;

    void Start()
    {
        // Add Card
        _startCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();

        counter++;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        { 
            Card newCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();

            Material material = counter % 2 == 0 ? cardMat1 : cardMat2;
            newCard.GetComponent<MeshRenderer>().material = material;
            _startCard.AddCardToCardPile(newCard);

            counter++;
        }
    }
}
