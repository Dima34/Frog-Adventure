using UnityEngine;

[CreateAssetMenu(fileName = "LevelManagerData", menuName = "New Level Manager Data", order = 51)]
public class LevelManager : ScriptableObject {
    int _startNumber = 0;
    int _increment = 1;
    int _iterationCount = 10;
    int _currentNumber;
    
    public int StartNumber {get => _startNumber;}
    public int Increment {get => _increment;}
    public int IterationCount {get => _iterationCount;}
    public int CurrentNumber {get => _currentNumber;}

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
}