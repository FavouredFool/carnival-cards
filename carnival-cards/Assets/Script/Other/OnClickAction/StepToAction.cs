using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepToAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Card card)
    {
        cardManager.SetLayout(card);
    }
}
