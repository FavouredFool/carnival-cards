using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    public OnClickManager _onClickManager;

    public void SetPlaceLayout(Context mainCard, List<Context> subCards, Context backCard, Context discardCard)
    {
        Vector2 mainPos = new Vector2(-5f, 0f);
        Vector2 backPos = new Vector2(-5f, 3f);
        Vector2 discardPos = new Vector2(5f, 3f);

        if (mainCard != null)
        {
            mainCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.CLOSEUP));
        }
        
        if (backCard != null)
        {
            backCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPTO));
        }

        for (int i = 0; i < subCards.Count; i++)
        {
            subCards[i].SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPTO));
        }

        if (discardCard != null)
        {
            discardCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.NOTHING));
        }

        if (mainCard != null)
        {
            MoveCard(mainCard.GetCard(), mainPos);
        }

        FanOutCardListAtPos(subCards);

        if (backCard != null)
        {
            MoveCard(backCard.GetCard(), backPos);
        }

        if (discardCard != null)
        {
            MoveCard(discardCard.GetCard(), discardPos);
        }
    }

    public void SetCoverLayout(Context rootContext)
    {
        Vector2 mainPos = new(0, 0);

        rootContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPPOSTCOVER));
        MoveCard(rootContext.GetCard(), mainPos);
        
    }

    public void SetPostCoverLayout(Context rootContext)
    {
        Vector2 infoPos = new(0, 0);
        Vector2 coverPos = new(-5, 3);

        Context infoContext = rootContext.ChildContexts[0];

        rootContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPCOVER));
        infoContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPPOSTPOSTCOVER));

        MoveCard(rootContext.GetCard(), coverPos);
        MoveCard(infoContext.GetCard(), infoPos);
    }

    public void SetPostPostCoverLayout(Context rootContext)
    {
        Vector2 infoPos = new(-5, 0);
        Vector2 coverPos = new(-5, 3);
        Vector2 placePos = new(0, 0);

        Context child = rootContext.ChildContexts[0];
        Context childChild = child.ChildContexts[0];

        rootContext.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPCOVER));
        child.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.CLOSEUP));
        childChild.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPTO));

        MoveCard(rootContext.GetCard(), coverPos);
        MoveCard(child.GetCard(), infoPos);
        MoveCard(childChild.GetCard(), placePos);

    }

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

}
