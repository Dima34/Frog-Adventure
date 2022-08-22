using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameManagerData", menuName = "New Game Manager Data", order = 51)]
public class GameManagerData : ScriptableObject {
    public int StartNumber {get => _startNumber;}
    public int Increment {get => _increment;}
    public int IterationCount {get => _iterationCount;}
    public int CurrentNumber {get => _currentNumber;}
    public Action OnCurrentNumberChange;
    
    int _startNumber = 0;
    int _increment = 1;
    int _iterationCount = 10;
    int _currentNumber = 0;


    public void SetStartNumber(int value){
        _startNumber = value;
    }

    public void SetIncrement(int value){
        _increment = value;
    }

    public void SetIterationCount(int value){
        _iterationCount = value;
    }

    public void SetCurrentNumber(int value){
        _currentNumber = value;
    }

    public void NextSectionNumber(){
        _currentNumber += _increment;
        Debug.Log("hey! The next number is " + _currentNumber);
        OnCurrentNumberChange?.Invoke();
    }
}