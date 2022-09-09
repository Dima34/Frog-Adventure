using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelData LevelDataSO;
    [HideInInspector]
    public LevelBuilder LevelBuilder;
    [HideInInspector]
    public GameObject LevelContainer;

    [SerializeField] Camera _camera;

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
    public Enemy EnemyPrefab { get => enemyPrefab; }
    public CameraMovement CameraMovement { get => cameraMovement;}
    public float EnemyMovementSpeed { get => enemyMovementSpeed; }

    int startNumber;
    int increment;
    int iterationCount;
    int currentStep;
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
    float enemyMovementSpeed;
    List<bool> sectionsWithEnemies;
    EnemySpawner enemySpawner;
    CameraMovement cameraMovement;


    int currentNumber;

    void Start()
    {
        SetLevelData();
        
        GlobalEventManager.OnCurrentNumberChange.AddListener(checkSectionsForActive);
        if(enemies){
            enemySpawner = new EnemySpawner(this);
            GlobalEventManager.OnCurrentNumberChange.AddListener(checkForEnemies);
        }
        
        CreateGameSequence();
        setDefaultGameStateValues();
        NextSectionNumber();
    }

    public void setDefaultGameStateValues(){
        currentNumber = startNumber;
        currentStep = 0;
    }

    public void SetLevelData()
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
        enemyMovementSpeed = LevelDataSO.EnemyMovementSpeed;
        sectionsWithEnemies = LevelDataSO.SectionsWithEnemies;
        cameraMovement = _camera.GetComponent<CameraMovement>();
    }

    public void CreateGameSequence(bool fromEditor = false)
    {
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
            if (section.OrdinalNumber == currentStep)
            {
                section.gameObject.SetActive(true);
            }
        });
    }

    public void NextSectionNumber()
    {
        if (currentNumber != 0)
            LevelBuilder.SpawnedSectionsList[currentNumber - 1].LeaveCorrectCell();

        currentStep++;
        currentNumber += increment;
        GlobalEventManager.OnCurrentNumberChange.Fire();
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
        
        cameraMovement.SetCameraStartPosition();
        yield return new WaitWhile(() => cameraMovement.IsCameraMooving);
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

    void checkForEnemies(){
        if(sectionsWithEnemies[currentStep - 1]){
            StartCoroutine(enemySpawnProcess());
        }
    }

    IEnumerator enemySpawnProcess(){
        yield return new WaitWhile(()=>CameraMovement.IsCameraMooving);
        
        Vector3 moveToPoint = _camera.ViewportToWorldPoint(new Vector3(0.5f,0,0));
        StartCoroutine(enemySpawner.EnemyLifeCycleSequence(this.transform,moveToPoint));
    }
}
