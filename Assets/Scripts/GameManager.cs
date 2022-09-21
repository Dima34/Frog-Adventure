using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public LevelData LevelDataSO;
    [HideInInspector]
    public LevelManager LevelManager;
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
    public Camera Camera { get => _camera;}
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
    List<EnemyTimepoint> enemyTimepoints = new List<EnemyTimepoint>();
    CameraMovement cameraMovement;
    float timeFromLevelStart;

    bool restartProcess = false;

    int currentNumber;

    void Start()
    {
        SetLevelData();
        InitLevelManager();
        
        GlobalEventManager.OnCurrentNumberChange.AddListener(checkSectionsForActive);
        GlobalEventManager.OnCorrectCell.AddListener((Collider2D collider)=>{
            NextSectionNumber();
        });
        
        CreateGameSequence();
        LevelManager.SpawnPlayer();
        setDefaultGameStateValues();
        NextSectionNumber();
    }

    private void FixedUpdate() {
        timeFromLevelStart += Time.fixedDeltaTime;
        checkForEnemies();
    }

    public void setDefaultGameStateValues(){
        currentNumber = startNumber;
        currentStep = 0;
        timeFromLevelStart = 0;
        foreach (EnemyTimepoint timePoint in enemyTimepoints)
        {
            timePoint.Spawned = false;
        }
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
        enemyTimepoints = LevelDataSO.EnemyTimepoints;

        cameraMovement = _camera.GetComponent<CameraMovement>();
    }

    public void CreateGameSequence(bool fromEditor = false)
    {
        LevelManager.BuildLevel(fromEditor);

        GlobalEventManager.OnLevelBuilded.Fire();
    }

    public void InitLevelManager(){
        // Check if playground already exist
        GameObject playground = GameObject.Find("Playground");

        // If playground exist - delete it. That needs to spawn new one if you click a "Generate Level" button in inspector or start the level
        if (playground != null)
            GameManager.DestroyImmediate(playground);

        // Spawn a level parent container (playground)
        LevelContainer = new GameObject("Playground");

        LevelManager = new LevelManager(this, LevelContainer.transform);
    }

    void checkSectionsForActive()
    {
        LevelManager.SpawnedSections.ForEach(delegate (Section section)
        {
            bool isActive = false;

            if (section.OrdinalNumber <= currentStep)
            {
                isActive = true;
            }

            section.gameObject.SetActive(isActive);
        });
    }

    public void NextSectionNumber()
    {
        currentStep++;
        currentNumber += increment;
        GlobalEventManager.OnCurrentNumberChange.Fire();
    }

    
    public void RestartLevel(){
        if(!restartProcess)
            StartCoroutine(restartLevelSequence());
    }

    public IEnumerator restartLevelSequence(){
        restartProcess = true;
        cameraMovement.SetCameraStartPosition();
        
        Task playerHidingProcess = new Task(LevelManager.HidePlayer());
        Task cellHidingProcess = new Task(LevelManager.HideCells());

        LevelManager.DestroyEnemies();

        yield return new WaitWhile(()=>cellHidingProcess.Running);
        LevelManager.DestroyCells();

        yield return new WaitWhile(()=>playerHidingProcess.Running);
        yield return new WaitWhile(() => cameraMovement.IsCameraMooving);
        

        setDefaultGameStateValues();
        LevelManager.SpawnCells();
        LevelManager.ShowPlayer();
        LevelManager.SpawnPlayer();

        NextSectionNumber();
        restartProcess = false;
    }

    void checkForEnemies(){ 
        float timeFromStart = System.MathF.Round(timeFromLevelStart, 1);

        foreach (EnemyTimepoint timePoint in enemyTimepoints)
        {
            float roundedTime = System.MathF.Round(timePoint.Time, 1);
            // Debug.Log(timeFromStart);

            if(roundedTime == timeFromStart && !timePoint.Spawned){
                timePoint.Spawned = true;
                StartCoroutine(spawnEnemy());
            }
        }
    }

    IEnumerator spawnEnemy(){
        yield return new WaitWhile(()=>CameraMovement.IsCameraMooving);
        LevelManager.SpawnEnemy();
    }
}
