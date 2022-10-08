using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnClickAction
{
    public void OnClick(CardManager cardManager, CardContext cardContext);
}
