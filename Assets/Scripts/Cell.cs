using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    [SerializeField] TMP_Text _numberText;
    
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
