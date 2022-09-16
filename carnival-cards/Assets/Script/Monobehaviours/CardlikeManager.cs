using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardlikeManager : MonoBehaviour
{
    public GameObject CardPrefab;
    public CardPileManager CardPileManager;

    public Material cardMat1;
    public Material cardMat2;

    private List<Card> _cardlikeList;

    private Card _startCard;

    private int counter = 0;

    void Start()
    {
        _startCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();
        _startCard.Init(CardPileManager, counter);

        counter++;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Card newCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();
            newCard.Init(CardPileManager, counter);

            Material material = counter % 2 == 0 ? cardMat1 : cardMat2;
            newCard.GetComponent<MeshRenderer>().material = material;
            _startCard.AddCardToCardPile(newCard);

            counter++;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Set card on the ground to the right of stack

        }
    }

    private void CreateCard()
    {

    }
}
