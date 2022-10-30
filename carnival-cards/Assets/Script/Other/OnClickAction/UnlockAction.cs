using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.Unlock(context);
    }
}
