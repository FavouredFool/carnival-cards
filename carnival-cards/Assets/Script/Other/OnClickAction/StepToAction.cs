using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepToAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, CardContext cardContext)
    {
        cardManager.SetLayout(cardContext);
    }
}
