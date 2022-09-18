using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CardManager _cardlikeManager;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Pile newPile = _cardlikeManager.CreateCardAndPile();
            _cardlikeManager.MovePileRandom(newPile);

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            List<Pile> pileListCopy = new List<Pile>(_cardlikeManager.GetAllPiles());

            foreach (Pile pile in pileListCopy)
            {
                _cardlikeManager.CreateCardAddToPile(pile);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            List<Pile> pileListCopy = new List<Pile>(_cardlikeManager.GetAllPiles());

            foreach (Pile pile in pileListCopy)
            {
                Pile newPile = _cardlikeManager.SplitPileInHalf(pile);

                if (newPile != pile)
                {
                    _cardlikeManager.MovePileRandom(newPile);
                }
                
            }
        }
    }
}
