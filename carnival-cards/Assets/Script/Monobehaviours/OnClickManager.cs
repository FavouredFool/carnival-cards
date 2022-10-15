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
    private IOnClickAction _stepPostCoverAction;
    private IOnClickAction _stepPostPostCoverAction;


    public void Awake()
    {
        _stepToAction = new StepToAction();
        _nothingAction = new NothingAction();
        _lookAtAction = new CloseUpAction();
        _stepNextAction = new StepNextAction();
        _stepCoverAction = new StepCoverAction();
        _stepPostCoverAction = new StepPostCoverAction();
        _stepPostPostCoverAction = new StepPostPostCoverAction();
    }

    public enum OnClickAction { STEPTO, CLOSEUP, STEPNEXT, STEPCOVER, STEPPOSTCOVER, STEPPOSTPOSTCOVER, NOTHING }


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

            case OnClickAction.STEPPOSTCOVER:
                return _stepPostCoverAction;

            case OnClickAction.STEPPOSTPOSTCOVER:
                return _stepPostPostCoverAction;

            case OnClickAction.NOTHING:
                return _nothingAction;

            default:
                Debug.LogWarning("FEHLER");
                return null;
        }
    }
}
