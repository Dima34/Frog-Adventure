using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Cell : MonoBehaviour
{
    [SerializeField] TMP_Text _numberText;
    public Action<Vector2> OnMoveEvent;

    
    [HideInInspector]
    public int Number;

    private void Awake() {
        SetNumber();
    }

    public void SetNumber(){
        _numberText.text = Number.ToString();
    }
}
