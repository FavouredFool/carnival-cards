using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepToAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.InitSetLayout(context);
    }
}
