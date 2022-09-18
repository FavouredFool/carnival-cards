using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PileManager _cardlikeManager;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _cardlikeManager.CreateCardAndPile();

            List<Pile> pileListCopy = new List<Pile>(_cardlikeManager.GetAllPiles());

            if (pileListCopy.Count > 1)
            {
                _cardlikeManager.AddPileToPile(pileListCopy[^1], pileListCopy[^2]);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (Pile pile in _cardlikeManager.GetAllPiles())
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // Set card on the ground to the right of pile
        }
    }
}
