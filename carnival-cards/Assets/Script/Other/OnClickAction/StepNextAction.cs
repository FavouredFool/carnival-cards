using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepNextAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        if (context.ChildContexts[0] != null)
        {
            cardManager.SetPlaceLayout(context.ChildContexts[0]);
        }
    }
}
