using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardLabel : MonoBehaviour
{
    public TMP_Text label;
    public Card card;

    public void Update()
    {
        label.text = card.GetCardLabel().ToString();
    }
}
