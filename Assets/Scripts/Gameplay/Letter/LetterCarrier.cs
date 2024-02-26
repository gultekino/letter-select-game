using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class LetterCarrier : MonoBehaviour, ICarryable, ISelecable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text tmpText;
    Slot carryingSlot;
    bool isGettingCarried = false;
    
    public Slot CarryingSlot
    {
        get => carryingSlot;
        set => carryingSlot = value;
    }
    
    public void OnMouseDown()
    {
        TrySelect();
    }

    private void Awake()
    {
        var rndV = Random.value;
        if (rndV> 0.66f)
        {
            tmpText.SetText("A");
        }else if (rndV > 0.33f)
        {
            tmpText.SetText("B");
        }
        else
        {
            tmpText.SetText("L");
        }
    }

    public void TrySelect()
    {
        if (carryingSlot.SlotLocation == SlotLocation.GridA)
            Select();
    }

    private void Select()
    {
        LetterManager.Instance.LetterClicked(this);
    }
    
    public void GetCarried(Slot carryingSlot)
    {
        this.carryingSlot = carryingSlot;
        isGettingCarried = true;
        transform.DOMove(carryingSlot.WorldPosition, 0.5f);
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
    
    public char GetLetter()
    {
        return tmpText.text[0];
    }
}
