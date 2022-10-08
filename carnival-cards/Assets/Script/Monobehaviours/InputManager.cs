using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public CardManager _cardManager;
    public Camera _camera;
    public TextAsset textJSON;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FindCardFromClick();
        }
    }

    private Context GetContextFromMouseClick()
    {
        Ray shotRay = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(shotRay, out hit))
        {
            if (hit.collider != null)
            {
                Card hitCard = hit.collider.GetComponent<Card>();
                return _cardManager.FindContextFromCard(hitCard).GetNextNotAttachedContext();
            }
        }

        return null;
    }


    private void FindCardFromClick()
    {
        Context context = GetContextFromMouseClick();

        if (context == null)
        {
            return;
        }

        context.OnClickAction(_cardManager);
    }
}
