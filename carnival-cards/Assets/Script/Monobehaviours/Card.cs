using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private IOnClickAction _onClickAction;

    public void Update()
    {
        SynchronizeVisual();
    }

    public void AttachCardDirectly(Card cardToAttach)
    {
        cardToAttach.SetParentCard(this);
    }

    public void SynchronizeVisual()
    {
        if (GetIsAttached())
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        }
        else
        {
            if (transform.parent != null)
            {
                Debug.LogWarning("FEHLER");
            }
        }
    }

     public void SetHeight(float height)
     {
         transform.localPosition = new Vector3(transform.position.x, 0.005f * height, transform.position.z);
     }


    public void SetOnClickAction(IOnClickAction onClickAction)
    {
        _onClickAction = onClickAction;
    }

    public IOnClickAction GetOnClickAction()
    {
        return _onClickAction;
    }

    public string GetCardLabel()
    {
        return name;
    }

    public void SetParentCard(Card parentCard)
    {
        if (parentCard != null)
        {
            transform.parent = parentCard.transform;
        }
        else
        {
            transform.parent = null;
        }
            
    }

    public bool GetIsAttached()
    {
        return transform.parent != null;
    }
}
