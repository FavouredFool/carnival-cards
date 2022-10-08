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
            mainCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.NOTHING));
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
