using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CameraMovement _camera;
    
    public LevelData LevelDataSO;
    [HideInInspector]
    public LevelBuilder LevelBuilder;
    [HideInInspector]
    public GameObject LevelContainer;

    public int StartNumber { get => startNumber; }
    public int Increment { get => increment; }
    public int IterationCount { get => iterationCount; }
    public Transform PlayerPrefab { get => playerPrefab; }
    public Transform StartPrefab { get => startPrefab; }
    public Transform FinishPrefab { get => finishPrefab; }
    public Section SectionPrefab { get => sectionPrefab; }
    public Cell CellPrefab { get => cellPrefab; }
    public float PropGaps { get => propGaps; }
    public int CurrentNumber { get => currentNumber; }
    public float SectionSideMarginSize { get => sectionSideMarginSize; }
    public int CellInSectionAmount { get => cellInSectionAmount; }
    public bool WillCellsMove { get => willCellsMove; }
    public float CellMoveSpeed { get => cellMoveSpeed; }

    int startNumber;
    int increment;
    int iterationCount;
    int iteration;
    Transform playerPrefab;
    Transform startPrefab;
    Transform finishPrefab;
    Section sectionPrefab;
    Cell cellPrefab;
    float propGaps;
    float sectionSideMarginSize;
    int cellInSectionAmount;
    bool willCellsMove;
    float cellMoveSpeed;
    bool enemies;
    Enemy enemyPrefab;
    List<bool> sectionsWithEnemies;


    int currentNumber;

    void Start()
    {
        CreateGameSequence();
        setDefaultGameStateValues();
        NextSectionNumber();
    }

    public void setDefaultGameStateValues(){
        currentNumber = startNumber;
        iteration = 0;
    }

    void setLevelData()
    {
        startNumber = LevelDataSO.StartNumber;
        increment = LevelDataSO.Increment;
        iterationCount = LevelDataSO.IterationCount;
        playerPrefab = LevelDataSO.PlayerPrefab;
        startPrefab = LevelDataSO.StartPrefab;
        finishPrefab = LevelDataSO.FinishPrefab;
        sectionPrefab = LevelDataSO.SectionPrefab;
        cellPrefab = LevelDataSO.CellPrefab;
        propGaps = LevelDataSO.PropGaps;
        sectionSideMarginSize = LevelDataSO.SectionSideMarginSize;
        cellInSectionAmount = LevelDataSO.CellInSectionAmount;
        willCellsMove = LevelDataSO.WillCellsMove;
        cellMoveSpeed = LevelDataSO.CellMoveSpeed;
        enemies = LevelDataSO.Enemies; 
        enemyPrefab = LevelDataSO.EnemyPrefab;
        sectionsWithEnemies = LevelDataSO.SectionsWithEnemies;
    }

    public void CreateGameSequence(bool fromEditor = false)
    {
        setLevelData();
        createLevel(fromEditor);
        if(!fromEditor){
            LevelBuilder.CreatePlayer();
            LevelBuilder.SpawnPlayer();
        }
    }

    void checkSectionsForActive()
    {
        LevelBuilder.SpawnedSectionsList.ForEach(delegate (Section section)
        {
            if (section.OrdinalNumber == iteration)
            {
                section.gameObject.SetActive(true);
            }
        });
    }

    public void NextSectionNumber()
    {
        if (currentNumber != 0)
            LevelBuilder.SpawnedSectionsList[currentNumber - 1].LeaveCorrectCell();

        iteration++;
        currentNumber += increment;
        GlobalEventManager.OnCurrentNumberChange.Fire();
        checkSectionsForActive();
    }

    void createLevel(bool fromEditor)
    {
        // Check if playground already exist
        GameObject playground = GameObject.Find("Playground");

        // If playground exist - delete it. That needs to spawn new one if you click a "Generate Level" button in inspector or start the level
        if (playground != null)
            GameManager.DestroyImmediate(playground);

        // Spawn a level parent container (playground)
        LevelContainer = new GameObject("Playground");

        LevelBuilder = new LevelBuilder(this, LevelContainer.transform);
        LevelBuilder.BuildLevel(fromEditor);

        GlobalEventManager.OnLevelBuilded.Fire();
    }

    public IEnumerator restartLevelSequence(){
        LevelBuilder.HidePlayer();

        Task sectionsHidingRoutine = new Task(HideSections());
        
        _camera.SetCameraStartPosition();
        yield return new WaitWhile(() => _camera.IsCameraMooving);
        yield return new WaitWhile(() => sectionsHidingRoutine.Running);

        restartLevel();
    }

    void restartLevel(){
        LevelBuilder.SpawnPlayer();
        LevelBuilder.CreateSections();
        setDefaultGameStateValues();
        NextSectionNumber();
    }

    public IEnumerator HideSections(){
        List<Section> spawnedSectionsList = LevelBuilder.SpawnedSectionsList;
        Task lastCellHidingProcess = null;

        for (int i = 0; i < spawnedSectionsList.Count; i++)
        {
            Section section = spawnedSectionsList[i].GetComponent<Section>();

            if(section.gameObject.active){
                lastCellHidingProcess = new Task(section.HideCellsSequence());
                yield return null;
            }
        }

        yield return new WaitWhile(()=> lastCellHidingProcess.Running);
        yield return null;

        for (int i = 0; i < spawnedSectionsList.Count; i++)
        {
            Section section = spawnedSectionsList[i].GetComponent<Section>();
            
            section.gameObject.SetActive(false);
        }
    }
}
