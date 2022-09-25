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
            Pile newPile = _cardlikeManager.CreatePile();

            for (int i = ExampleCardContexts.GetTotalCards() - 1; i >= 0; i--)
            {
                _cardlikeManager.CreateCardAddToPile(newPile);
            }

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
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            List<Pile> pileListCopy = new List<Pile>(_cardlikeManager.GetAllPiles());

            foreach (Pile pile in pileListCopy)
            {
                Pile newPile = _cardlikeManager.SplitPileAtRange(pile, 1, 1);

                if (newPile != pile)
                {
                    _cardlikeManager.MovePileRandom(newPile);
                }

            }
        }
    }
}
