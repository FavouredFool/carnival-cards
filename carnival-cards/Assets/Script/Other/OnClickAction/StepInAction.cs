using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepInAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Card card)
    {
        cardManager.SetPlaceLayout(card);
    }
}
