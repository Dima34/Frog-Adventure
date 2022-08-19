using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] int _startNumber = 0;
    [SerializeField] int _increment = 1;
    [SerializeField] int _iterationCount = 10;
    [SerializeField] GameObject _startPrefab;
    [SerializeField] GameObject _finishPrefab;
    
    [Tooltip("Space between start - jumprops, jumprops - jumprops, jumprops - finish")]
    [SerializeField] float _propGaps = 20;


    int nextNum;
    
    // Start is called before the first frame update
    void Start()
    {
        nextNum = _startNumber + _increment;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
