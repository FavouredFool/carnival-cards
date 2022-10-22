using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CardManager _cardManager;

    private IOnClickAction _stepToAction;
    private IOnClickAction _nothingAction;
    private IOnClickAction _lookAtAction;
    private IOnClickAction _pickUpAction;
    private IOnClickAction _inventoryAction;


    public void Awake()
    {
        _stepToAction = new StepToAction();
        _nothingAction = new NothingAction();
        _lookAtAction = new CloseUpAction();
        _pickUpAction = new PickUpAction();
        _inventoryAction = new InventoryAction();
    }

    public enum OnClickAction { STEPTO, CLOSEUP, PICKUP, NOTHING, INVENTORY }


    public IOnClickAction GetActionFromOnClickAction(OnClickAction actionEnum)
    {
        switch (actionEnum)
        {
            case OnClickAction.STEPTO:
                return _stepToAction;

            case OnClickAction.CLOSEUP:
                return _lookAtAction;

            case OnClickAction.NOTHING:
                return _nothingAction;

            case OnClickAction.PICKUP:
                return _pickUpAction;

            case OnClickAction.INVENTORY:
                return _inventoryAction;

            default:
                Debug.LogWarning("FEHLER");
                return null;
        }
    }
}
