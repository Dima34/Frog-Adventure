using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    [SerializeField] TMP_Text _numberText;
    
    public int Number;
    public bool IsNext;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake() {
        _numberText.text = Number.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
