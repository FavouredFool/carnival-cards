using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAction : IOnClickAction
{
    public void OnClick(CardManager cardManager, Context context)
    {
        cardManager.InitToggleInventory(true);
    }
}
