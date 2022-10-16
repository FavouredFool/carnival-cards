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

    public void SetPlaceLayout(Context placeContext)
    {
        _activeContext = placeContext;

        // Main
        if (placeContext != null)
        {
            DetachCard(placeContext);
            placeContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.CLOSEUP));
            MoveCard(placeContext.GetCard(), mainPos);
        }

        // Children
        List<Context> subContexts = placeContext.ChildContexts;
        foreach (Context subContext in subContexts)
        {
            DetachCard(subContext);
            subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
        }
        FanOutCardListAtPos(subContexts);

        //Back
        Context backContext = placeContext.GetParentContext();
        if (backContext != null)
        {
            DetachCard(backContext);
            backContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            MoveCard(backContext.GetCard(), backPos);
        }

        //Discard
        Context rootContext = _cardManager.GetRootContext();
        if (rootContext != placeContext && rootContext != backContext)
        {
            Context discardContext = rootContext;
            DetachCard(discardContext);
            discardContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.NOTHING));
            MoveCard(discardContext.GetCard(), discardPos);
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

        if (actionContext == null)
        {
            Debug.LogWarning("FEHLER: KEIN INVESTIGATE WURDE GEFUNDEN");
        }

        return actionContext;
    }

    public void SetItemLayout(Context itemContext)
    {
        _activeContext = itemContext;

        // Main
        if (itemContext != null)
        {
            DetachCard(itemContext);
            itemContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.CLOSEUP));
            MoveCard(itemContext.GetCard(), mainPos);
        }

        // Action
        Context actionContext = GetActionContext(itemContext);
        DetachCard(actionContext);
        actionContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
        MoveCard(actionContext.GetCard(), actionPos);

        // Children
        List<Context> subContexts = itemContext.ChildContexts.Where(e => e != actionContext).ToList();
        foreach (Context subContext in subContexts)
        {
            DetachCard(subContext);
            subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
        }
        FanOutCardListAtPos(subContexts);

        //Back
        Context backContext = itemContext.GetParentContext();
        if (backContext != null)
        {
            DetachCard(backContext);
            backContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            MoveCard(backContext.GetCard(), backPos);
        }

        //Discard
        Context rootContext = _cardManager.GetRootContext();
        if (rootContext != itemContext && rootContext != backContext)
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
            MoveCard(rootContext.GetCard(), mainPos);

            // Children
            foreach (Context subContext in rootContext.ChildContexts)
            {
                DetachCard(subContext);
                subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            }
            FanOutCardListAtPos(rootContext.ChildContexts);
        }
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

    public void CenteredLayout(Card mainCard)
    {
        Vector2 mainPos = new Vector2(0f, 0f);
        MoveCard(mainCard, mainPos);
    }

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

    #region Move Card
    public void MoveCard(Card card, Vector2 newPosition)
    {
        card.transform.localPosition = new Vector3(newPosition.x, card.transform.position.y, newPosition.y);
    }

    public void MoveCardRandom(Card card)
    {
        MoveCard(card, Random.insideUnitCircle * 3);
    }
    #endregion

    public void SetActiveContext(Context activeContext)
    {
        _activeContext = activeContext;
    }

}
