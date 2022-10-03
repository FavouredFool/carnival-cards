using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepOutAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Card card)
    {
        Debug.Log("stepOut");
    }
}
