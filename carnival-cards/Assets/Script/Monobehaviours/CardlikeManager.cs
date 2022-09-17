using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardlikeManager : MonoBehaviour
{
    public GameObject CardPrefab;
    public CardPileManager CardPileManager;

    public Material cardMat1;
    public Material cardMat2;

    private List<Cardlike> _cardlikeList;

    private int counter = 0;

    void Start()
    {
        _cardlikeList = new List<Cardlike>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Card newCard = CreateCard();

            if (newCard == _cardlikeList[0])
            {
                return;
            }

            _cardlikeList[^2].AddCardToCardPile(newCard);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Set card on the ground to the right of stack
            

        }
    }

    private Card CreateCard()
    {
        Card newCard = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity).GetComponent<Card>();
        newCard.Init(CardPileManager, counter);

        Material material = counter % 2 == 0 ? cardMat1 : cardMat2;
        newCard.GetComponent<MeshRenderer>().material = material;

        _cardlikeList.Add(newCard);

        counter++;

        return newCard;
    }

    public void AddCardlike(Cardlike cardlike)
    {
        _cardlikeList.Add(cardlike);
    }

    public void RemoveCardlike(Cardlike cardlike)
    {
        _cardlikeList.Remove(cardlike);
    }
}
