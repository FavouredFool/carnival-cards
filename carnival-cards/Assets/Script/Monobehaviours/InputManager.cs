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

    private Pile GetPileFromMouseClick()
    {
        // MOUSE POS
        Ray shotRay = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(shotRay, out hit))
        {
            if (hit.collider != null)
            {
                return hit.collider.GetComponent<Pile>();
            }
        }

        return null;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Pile foundPile = GetPileFromMouseClick();

            if (!foundPile)
            {
                return;
            }

            Debug.Log("clicked Pile");
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            List<Pile> pileListCopy = new List<Pile>(_cardManager.GetAllPiles());

            foreach (Pile pile in pileListCopy)
            {
                Pile newPile = _cardManager.SplitPileAtRange(pile, 0, 1);

                if (newPile != pile)
                {
                    _cardManager.MovePileRandom(newPile);
                }

            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            List<Pile> pileListCopy = new List<Pile>(_cardManager.GetAllPiles());

            if (pileListCopy.Count <= 1)
            {
                return;
            }

            for (int i = 1; i < pileListCopy.Count; i++)
            {
                _cardManager.AddPileToPileAtIndex(pileListCopy[i], pileListCopy[0], 1);
            }
        }
    }
}
