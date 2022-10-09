using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CardManager _cardManager;

    private IOnClickAction _stepToAction;
    private IOnClickAction _nothingAction;
    private IOnClickAction _lookAtAction;


    public void Awake()
    {
        _stepToAction = new StepToAction();
        _nothingAction = new NothingAction();
        _lookAtAction = new CloseUpAction();
    }

    public enum OnClickAction { STEPTO, CLOSEUP, NOTHING }


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

            default:
                Debug.LogWarning("FEHLER");
                return null;
        }
    }
}
