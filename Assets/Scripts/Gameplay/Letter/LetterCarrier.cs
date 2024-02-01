using System;
using TMPro;
using UnityEngine;

public class LetterCarrier : MonoBehaviour, ICarryable, ISelecable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text tmpText;
    bool isGettingCarried = false;

    public void OnMouseDown()
    {
        TrySelect();
    }
    
    public void TrySelect()
    {
        if (!isGettingCarried)
            Select();
    }
    
    public void Select()
    {
        LetterManager.Instance.LetterClicked(this);
    }
    
    public void GetCarried(Vector2 carryingPos)
    {
        transform.position = carryingPos;
    }

    public bool IsGettingCarried()
    {
        return isGettingCarried;
    }

    public bool IsSelectable()
    {
        return isGettingCarried;
    }
    
    public void UpdateVisuals()
    {
        throw new NotImplementedException();
    }
    
    public char GetLetterCarrying()
    {
        return tmpText.text[0];
    }
}
