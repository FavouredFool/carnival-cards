using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OnClickManager;

public class LayoutManager : MonoBehaviour
{
    public OnClickManager _onClickManager;
    public CardManager _cardManager;

    private Context _activeContext;

    Vector2 zeroPos = Vector2.zero;
    Vector2 mainPos = new(-5, 0);
    Vector2 backPos = new(-5, 3);
    Vector2 discardPos = new(5, 3);

    public void SetPlaceLayout(Context mainCardContext)
    {
        _activeContext = mainCardContext;

        // Main
        if (mainCardContext != null)
        {
            DetachCard(mainCardContext);
            mainCardContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.CLOSEUP));
            MoveCard(mainCardContext.GetCard(), mainPos);
        }

        // Children
        List<Context> subContexts = mainCardContext.ChildContexts;
        foreach (Context subContext in subContexts)
        {
            DetachCard(subContext);
            subContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
        }
        FanOutCardListAtPos(subContexts);

        //Back
        Context backContext = mainCardContext.GetParentContext();
        if (backContext != null)
        {
            DetachCard(backContext);
            backContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickAction.STEPTO));
            MoveCard(backContext.GetCard(), backPos);
        }

        //Discard
        Context rootContext = _cardManager.GetRootContext();
        if (rootContext != mainCardContext && rootContext != backContext)
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
            MoveCard(cardList[i].GetCard(), new Vector2(2f * i, 0f));
        }
    }

    public void SetCardsInLayout()
    {

    }

    public void GetLayoutFromCardType()
    {

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
