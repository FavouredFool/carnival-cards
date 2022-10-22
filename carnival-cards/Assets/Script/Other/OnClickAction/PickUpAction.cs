using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.PickUp(context);
    }
}
