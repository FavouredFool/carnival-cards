using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCoverAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.SetCoverLayout();
    }
}
