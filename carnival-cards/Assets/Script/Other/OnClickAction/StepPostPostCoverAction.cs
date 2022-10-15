using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPostPostCoverAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.SetPostPostCoverLayout();
    }
}
