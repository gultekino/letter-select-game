using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterCarrier : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TMP_Text tmpText;

    private void OnMouseEnter()
    {
        TrySelectLetter();
    }

    private void TrySelectLetter()
    {
        
    }

    void UpdateVisuals()
    {
        
    }
}
