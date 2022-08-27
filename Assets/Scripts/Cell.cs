using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class Cell : MonoBehaviour
{
    [SerializeField] TMP_Text _numberText;
    Action<Vector2> OnMoveEvent;    
    
    // Commented for development
    // [HideInInspector]
    public int Number;

    private void Awake() {
        SetNumber();
    }

    public void SetNumber(){
        _numberText.text = Number.ToString();
    }
}
