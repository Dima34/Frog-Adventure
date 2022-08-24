using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "New LevelData", order = 51)]
public class LevelData : ScriptableObject {
    public int StartNumber = 0;
    public int Increment = 1;
    public int IterationCount = 10;
    public Transform Player;
    public Transform StartPrefab;
    public Transform FinishPrefab;
    public Transform Section;
    public Transform Cell;
    public float PropGaps = 20;    
}