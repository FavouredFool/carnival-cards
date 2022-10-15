using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CardManager _cardManager;

    private IOnClickAction _stepToAction;
    private IOnClickAction _nothingAction;
    private IOnClickAction _lookAtAction;
    private IOnClickAction _stepNextAction;
    private IOnClickAction _stepCoverAction;


    public void Awake()
    {
        _stepToAction = new StepToAction();
        _nothingAction = new NothingAction();
        _lookAtAction = new CloseUpAction();
        _stepNextAction = new StepNextAction();
        _stepCoverAction = new StepCoverAction();
    }

    public enum OnClickAction { STEPTO, CLOSEUP, STEPNEXT, STEPCOVER, NOTHING }


    public IOnClickAction GetActionFromOnClickAction(OnClickAction actionEnum)
    {
        switch (actionEnum)
        {
            case OnClickAction.STEPTO:
                return _stepToAction;

            case OnClickAction.CLOSEUP:
                return _lookAtAction;

            case OnClickAction.STEPNEXT:
                return _stepNextAction;

            case OnClickAction.STEPCOVER:
                return _stepCoverAction;

            case OnClickAction.NOTHING:
                return _nothingAction;

            default:
                Debug.LogWarning("FEHLER");
                return null;
        }
    }
}
