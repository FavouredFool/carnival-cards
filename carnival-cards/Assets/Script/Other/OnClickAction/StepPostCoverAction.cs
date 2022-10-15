using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPostCoverAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.SetPostCoverLayout();
    }
}
