using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CardManager _cardManager;

    private IOnClickAction _stepInAction;
    private IOnClickAction _stepOutAction;
    private IOnClickAction _nothingAction;


    public void Start()
    {
        _stepInAction = new StepInAction();
        _stepOutAction = new StepOutAction();
        _nothingAction = new NothingAction();
    }

    public enum OnClickAction { STEPIN, STEPOUT, NOTHING }


    public IOnClickAction GetActionFromOnClickAction(OnClickAction actionEnum)
    {
        switch (actionEnum)
        {
            case OnClickAction.STEPIN:
                return _stepInAction;

            case OnClickAction.STEPOUT:
                return _stepOutAction;

            case OnClickAction.NOTHING:
                return _nothingAction;

            default:
                Debug.LogWarning("FEHLER");
                return null;
        }
    }
}
