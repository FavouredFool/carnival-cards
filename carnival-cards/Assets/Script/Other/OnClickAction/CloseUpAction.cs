using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.CloseUpCardback(context);
    }
}
