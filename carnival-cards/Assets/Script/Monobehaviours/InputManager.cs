using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CardManager _cardManager;
    public Camera _camera;
    public TextAsset textJSON;


    /**
    private void CreateCompleteDeck()
    {
        Pile newPile = _cardManager.CreatePile();

        for (int i = ExampleCardContexts.GetTotalCards() - 1; i >= 0; i--)
        {
            _cardManager.CreateCardAddToPile(newPile);
        }

        _cardManager.MovePileRandom(newPile);
    }
    */


    private Card GetCardFromMouseClick()
    {
        Ray shotRay = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(shotRay, out hit))
        {
            if (hit.collider != null)
            {
                Card hitCard = hit.collider.GetComponent<Card>();
                return hitCard.GetRootCard();
            }
        }

        return null;
    }

    private void DisplayReferencedCards(Card card)
    {
        List<Card> childCardsCopy = new(card.GetChildCards());

        foreach (Card activeCard in childCardsCopy)
        {
            activeCard.GetParentCard().GetChildCards().Remove(activeCard);
            activeCard.SetParentCard(null);

            _cardManager.MoveCardRandom(activeCard);
        }
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            FindCardFromClick();
        }
    }

    private void FindCardFromClick()
    {
        Card foundCard = GetCardFromMouseClick();

        if (!foundCard)
        {
            return;
        }

        DisplayReferencedCards(foundCard);
    }
}
