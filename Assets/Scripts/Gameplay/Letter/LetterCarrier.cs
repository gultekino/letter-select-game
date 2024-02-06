using System;
using TMPro;
using UnityEngine;

public class LetterCarrier : MonoBehaviour, ICarryable, ISelecable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text tmpText;
    Slot carryingSlot;
    public Slot CarryingSlot
    {
        get => carryingSlot;
        set => carryingSlot = value;
    }
    
    bool isGettingCarried = false;

    public void OnMouseDown()
    {
        TrySelect();
    }
    
    public void TrySelect()
    {
        if (carryingSlot.SlotLocation == SlotLocation.GridA)
            Select();
    }
    
    public void Select()
    {
        LetterManager.Instance.LetterClicked(this);
    }
    
    public void GetCarried(Slot carryingSlot)
    {
        this.carryingSlot = carryingSlot;
        isGettingCarried = true;
        transform.position = carryingSlot.WorldPosition;
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
    }
    
    public char GetLetterCarrying()
    {
        return tmpText.text[0];
    }
}