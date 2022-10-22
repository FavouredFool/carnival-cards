using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static OnClickManager;
using static CardTypeManager;

public class LayoutManager : MonoBehaviour
{
    public OnClickManager _onClickManager;
    public CardManager _cardManager;

    private Context _activeContext;

    Vector2 zeroPos = Vector2.zero;
    Vector2 mainPos = new(-5, 0);
    Vector2 backPos = new(-5, 3);
    Vector2 discardPos = new(5, 3);
    Vector2 actionPos = new(-5, -2f);

    public void SetBasicLayout(Context mainContext)
    {
        _activeContext = mainContext;

        // Main
        DetachCard(mainContext);
        mainContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.CLOSEUP));
        MoveCard(mainContext.GetCard(), mainPos);

        // Action
        Context actionContext = GetActionContext(mainContext);
        if (actionContext != null)
        {
            DetachCard(actionContext);
            actionContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            MoveCard(actionContext.GetCard(), actionPos);
        }
        
        // Children
        List<Context> subContexts = mainContext.ChildContexts.Where(e => e != actionContext).ToList();
        foreach (Context subContext in subContexts)
        {
            DetachCard(subContext);
            if (subContext.Type == CardType.ITEM)
            {
                subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.PICKUP));
            }
            else
            {
                subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            }

        }
        FanOutCardListAtPos(subContexts);

        //Back
        Context backContext = mainContext.GetParentContext();
        if (backContext != null)
        {
            DetachCard(backContext);
            backContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            MoveCard(backContext.GetCard(), backPos);
        }

        //Discard
        Context rootContext = _cardManager.GetRootContext();
        if (rootContext != mainContext && rootContext != backContext)
        {
            Context discardContext = rootContext;
            DetachCard(discardContext);
            discardContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.NOTHING));
            MoveCard(discardContext.GetCard(), discardPos);
        }
    }

    public void SetCoverLayout(Context rootContext)
    {
        if (rootContext == _activeContext)
        {
            _activeContext = null;

            // Root
            rootContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPTO));
            MoveCard(rootContext.GetCard(), zeroPos);
        }
        else
        {
            _activeContext = rootContext;

            // Root
            rootContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPTO));
            MoveCard(rootContext.GetCard(), backPos);

            // Children
            foreach (Context subContext in rootContext.ChildContexts)
            {
                DetachCard(subContext);
                subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            }
            FanOutCardListAtPos(rootContext.ChildContexts);
        }
    }

    private Context GetActionContext(Context mainContext)
    {
        Context actionContext = null;
        // Find INVESTIGATE CARD
        foreach (Context context in mainContext.ChildContexts)
        {
            if (context.Type == CardType.INVESTIGATION)
            {
                actionContext = context;
                break;
            }
        }

        return actionContext;
    }

    public void DetachCard(Context contextToRemove)
    {
        if (contextToRemove.GetCard().GetIsAttached())
        {
            contextToRemove.GetCard().SetParentCard(null);

            _cardManager.GetTopContextList().Add(contextToRemove);

            contextToRemove.GetCard().SynchronizeVisual();
        }
    }

    /*
    public void AttachCard(Context contextToAttach, Context baseContext)
    {
        if (!_cardManager.GetTopContextList().Contains(contextToAttach))
        {
            Debug.LogWarning("FEHLER");
        }

        baseContext.AttachCardAtEnd(contextToAttach);

        _cardManager.GetTopContextList().Remove(contextToAttach);
    }
    */

    public void FanOutCardListAtPos(List<Context> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].Type == CardTypeManager.CardType.INVESTIGATION) {
                continue;
            }

            MoveCard(cardList[i].GetCard(), new Vector2(2f * i, 0f));
        }
    }

    public void MoveCard(Card card, Vector2 newPosition)
    {
        card.transform.localPosition = new Vector3(newPosition.x, card.transform.position.y, newPosition.y);
    }

    public void SetActiveContext(Context activeContext)
    {
        _activeContext = activeContext;
    }

}
