using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    public OnClickManager _onClickManager;

    public void SetPlaceLayout(Card mainCard, List<Card> subCards, Card backCard, Card discardCard)
    {
        Vector2 mainPos = new Vector2(-5f, 0f);
        Vector2 stepOutPos = new Vector2(-5f, 3f);
        Vector2 discardPos = new Vector2(5f, 3f);

        mainCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.NOTHING));
        
        if (backCard != null)
        {
            backCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPOUT));
        }

        for (int i = 0; i < subCards.Count; i++)
        {
            subCards[i].SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.STEPIN));
        }

        if (discardCard != null)
        {
            discardCard.SetOnClickAction(_onClickManager.GetActionFromOnClickAction(OnClickManager.OnClickAction.NOTHING));
        }


        CardContext greaterCardContext = mainCard.GetCardContext().GetParentContext();

        MoveCard(mainCard, mainPos);
        FanOutCardListAtPos(subCards);

        if (greaterCardContext != null)
        {
            MoveCard(backCard, stepOutPos);
        }

        if (discardCard != null)
        {
            MoveCard(discardCard, discardPos);
        }
    }

    public void CenteredLayout(Card mainCard)
    {
        Vector2 mainPos = new Vector2(0f, 0f);
        MoveCard(mainCard, mainPos);
    }

    public void FanOutCardListAtPos(List<Card> cardList)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            MoveCard(cardList[i], new Vector2(2f * i, 0f));
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
        card.transform.position = new Vector3(newPosition.x, card.transform.position.y, newPosition.y);
    }

    public void MoveCardRandom(Card card)
    {
        MoveCard(card, Random.insideUnitCircle * 3);
    }
    #endregion

}
