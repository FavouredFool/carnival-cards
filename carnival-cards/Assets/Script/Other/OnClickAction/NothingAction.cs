using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        Debug.Log("nothing");
    }
}
