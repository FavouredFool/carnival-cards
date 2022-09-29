using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public TextAsset _jsonText;

    public PileFactory _pileFactory;
    public CardFactory _cardFactory;

    private List<Pile> _pileList = new();

    private JsonReader jsonReader;

    private void Start()
    {
        jsonReader = new JsonReader();

        Pile newPile = CreatePile();


        CreateCardAddToPile(newPile);
        

        MovePileRandom(newPile);

    }

    private void Update()
    {
        // how bad is this? -> Iterating through every card of every pile per frame. pretty bad.
        List<Pile> pileListCopy = new(_pileList);
        foreach (Pile pile in pileListCopy)
        {
            if (pile.GetCardList().Count <= 0)
            {
                RemovePileFromList(pile);
                Destroy(pile.gameObject);
            }
        }
    }

    #region Create Stuff
    public Pile CreatePileWithCards(List<Card> cardList)
    {
        Pile newPile = CreatePile();
        newPile.AddCardListAtIndex(cardList, 0);
        return newPile;
    }

    public void CreateCardAddToPile(Pile pile)
    {
        Card newCard = CreateCard();
        pile.AddCardOnTop(newCard);
    }

    public Pile CreatePile()
    {
        Pile newPile = _pileFactory.CreateNewInstance();
        AddPileToList(newPile);
        return newPile;
    }
    public Card CreateCard()
    {
        // GetCardContext
        CardContext uppermostCardContext = jsonReader.ReadJsonForCardContext(_jsonText);


        return _cardFactory.CreateNewInstance(uppermostCardContext);
    }
    #endregion

    #region Move Piles
    public void MovePile(Pile pile, Vector2 newPosition)
    {
        pile.transform.position = new Vector3(newPosition.x, pile.transform.position.y, newPosition.y);
    }

    public void MovePileRandom(Pile pile)
    {
        MovePile(pile, Random.insideUnitCircle * 3);
    }
    #endregion

    #region PileList
    public void AddPileToList(Pile pile)
    {
        _pileList.Add(pile);
    }

    public void RemovePileFromList(Pile pile)
    {
        _pileList.Remove(pile);
    }

    public List<Pile> GetAllPiles()
    {
        return _pileList;
    }
    #endregion

    #region SplitOperation
    public Pile SplitPileInHalf(Pile pile)
    {
        return SplitPileAtRange(pile, Mathf.CeilToInt(pile.GetCardList().Count / 2f), Mathf.FloorToInt(pile.GetCardList().Count / 2f));
    }

    public Pile SplitPileAtRange(Pile pile, int index, int cardCount)
    {
        Debug.Assert(cardCount > 0);
        Debug.Assert(0 <= index && index + cardCount - 1 < pile.GetCardList().Count);
        
        List<Card> currentPileCards = pile.GetCardList();
        List<Card> newCards = currentPileCards.GetRange(index, cardCount);

        List<Card> remainingCardList = currentPileCards.Except(newCards).ToList();

        //pile.GetCardList().Clear();
        //pile.AddCardListAtIndex(remainingCardList, pile.GetCardList().Count);
        pile.RemoveCardList(newCards);

        // Create new Pile
        Pile newPile = CreatePileWithCards(newCards);

        return newPile;
    }
    #endregion

    #region Add Piles
    public void AddPileToPileAtIndex(Pile pileToAdd, Pile pileBase, int index)
    {
        pileBase.AddCardListAtIndex(pileToAdd.GetCardList(), index);
        pileToAdd.RemoveAllCards();
    }
    #endregion

}
