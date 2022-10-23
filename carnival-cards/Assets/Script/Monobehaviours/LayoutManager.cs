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

    

    Vector2 zeroPos = Vector2.zero;
    Vector2 mainPos = new(-5, 0);
    Vector2 backPos = new(-5, 3);
    Vector2 discardPos = new(5, 3);
    Vector2 actionPos = new(-5, -2);
    Vector2 inventoryPos = new(6, -3);

    public void SetBasicLayout(Context mainContext)
    {
        _cardManager.SetActiveContext(mainContext);

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
        DetachCard(backContext);
        backContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
        MoveCard(backContext.GetCard(), backPos);
        

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
        if (rootContext == _cardManager.GetActiveContext())
        {
            _cardManager.SetActiveContext(null);

            // Root
            rootContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPTO));
            MoveCard(rootContext.GetCard(), zeroPos);
        }
        else
        {
            _cardManager.SetActiveContext(rootContext);

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

    public void SetLockLayout(Context lockContext, Context inventoryContext)
    {
        _cardManager.SetActiveContext(lockContext);

        // Root
        DetachCard(lockContext);
        lockContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.CLOSEUP));
        MoveCard(lockContext.GetCard(), mainPos);

        // Inventory
        foreach (Context subContext in inventoryContext.ChildContexts)
        {
            DetachCard(subContext);

            subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.NOTHING));
        }
        FanOutCardListAtPos(inventoryContext.ChildContexts);

        // Back
        Context backContext = lockContext.GetParentContext();
        DetachCard(backContext);
        backContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
        MoveCard(backContext.GetCard(), backPos);
        

        // Discard
        Context rootContext = _cardManager.GetRootContext();
        if (rootContext != lockContext && rootContext != backContext)
        {
            Context discardContext = rootContext;
            DetachCard(discardContext);
            discardContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.NOTHING));
            MoveCard(discardContext.GetCard(), discardPos);
        }
    }

    public void SetInventoryContext(Context inventoryContext)
    {
        inventoryContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.INVENTORY));
        MoveCard(inventoryContext.GetCard(), inventoryPos);
    }

    public void ToggleInventory(Context inventoryContext, bool inventoryIsOpen)
    {
        if (inventoryIsOpen)
        {
            foreach (Context subContext in inventoryContext.ChildContexts)
            {
                DetachCard(subContext);
                subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.NOTHING));
            }
            FanOutInventory(inventoryContext.ChildContexts);
        }
    }

    private void FanOutInventory(List<Context> invlist)
    {
        for (int i = 0; i < invlist.Count; i++)
        {
            MoveCard(invlist[i].GetCard(), new Vector2(4 - (2 * i), -3f));
        }
    }

    private Context GetActionContext(Context mainContext)
    {
        Context actionContext = null;

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
            if (cardList[i].Type == CardType.INVESTIGATION) {
                continue;
            }

            MoveCard(cardList[i].GetCard(), new Vector2(2f * i, 0f));
        }
    }

    public void MoveCard(Card card, Vector2 newPosition)
    {
        card.transform.localPosition = new Vector3(newPosition.x, card.transform.position.y, newPosition.y);
    }

}
