using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "New LevelData", order = 51)]
public class LevelData : ScriptableObject {
    public int StartNumber = 0;
    public int Increment = 1;
    public int IterationCount = 10;
    public Transform Player;
    public Transform StartPrefab;
    public Transform FinishPrefab;

    [Header("Section settings")]
    public Section Section;
    public Cell Cell;
    public float PropGaps = 20;
    public int CellInSectionAmount = 3;
    public float SectionSideMarginSize = 2f;
    public bool WillCellsMove = false;
    public float CellMoveSpeed = 30f;

    [Header("Enemy settings")]
    List<Enemy> EnemyList;
}