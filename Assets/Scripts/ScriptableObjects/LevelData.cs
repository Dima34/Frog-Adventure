using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "New LevelData", order = 51)]
public class LevelData : ScriptableObject {
    [SerializeField] public int StartNumber = 0;
    [SerializeField] public int Increment = 1;
    [SerializeField] public int IterationCount = 10;
    [SerializeField] public Transform PlayerPrefab;
    [SerializeField] public Transform StartPrefab;
    [SerializeField] public Transform FinishPrefab;
    [SerializeField] public Transform BackgroundPrefab;
    [SerializeField] public Transform HintArrowPrefab;
    [SerializeField] public bool Enemies;

    // Sound
    [SerializeField] public AudioClip BackgroundClip;

    // Section settings
    [SerializeField] public Section SectionPrefab;
    [SerializeField] public Cell CellPrefab;
    [SerializeField] public float PropGaps = 20;
    [SerializeField] public int CellInSectionAmount = 3;
    [SerializeField] public float SectionSideMarginSize = 2f;
    [SerializeField] public bool WillCellsMove = false;
    [SerializeField] public float CellMoveSpeed = 30f;

    // Enemy settings
    [SerializeField] public Enemy EnemyPrefab;
    [SerializeField] public float EnemyMovementSpeed;
    [SerializeField] public List<EnemyTimepoint> EnemyTimepoints = new List<EnemyTimepoint>();
}

[System.Serializable]
public class EnemyTimepoint{
    [SerializeField] int time;
    bool spawned;

    public EnemyTimepoint(int onTime){
        time = onTime;
        spawned = false;
    }

    public bool Spawned { get => spawned; set => spawned = value; }
    public int Time { get => time; set => time = value;}
}