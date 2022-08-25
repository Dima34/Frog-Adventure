using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelData LevelDataSO;
    [SerializeField] CameraMovement cameraObject;

    public int StartNumber { get => startNumber; }
    public int Increment { get => increment; }
    public int IterationCount { get => iterationCount; }
    public Transform PlayerPrefab { get => playerPrefab; }
    public Transform StartPrefab { get => startPrefab; }
    public Transform FinishPrefab { get => finishPrefab; }
    public Transform SectionPrefab { get => sectionPrefab; }
    public Transform CellPrefab { get => cellPrefab; }
    public float PropGaps { get => propGaps; }
    public int CurrentNumber { get => currentNumber; }

    int startNumber;
    int increment;
    int iterationCount;
    int iteration = 0;
    Transform playerPrefab;
    Transform startPrefab;
    Transform finishPrefab;
    Transform sectionPrefab;
    Transform cellPrefab;
    float propGaps;
    LevelBuilder levelBuilder;


    int currentNumber;

    void Start()
    {
        BuildLevelSequence();
        NextSectionNumber();
    }

    public void BuildLevelSequence(bool fromEditor = false)
    {
        setLevelData();
        createLevel(fromEditor);
        setCamera();
    }

    void setLevelData()
    {
        startNumber = LevelDataSO.StartNumber;
        increment = LevelDataSO.Increment;
        iterationCount = LevelDataSO.IterationCount;
        playerPrefab = LevelDataSO.Player;
        startPrefab = LevelDataSO.StartPrefab;
        finishPrefab = LevelDataSO.FinishPrefab;
        sectionPrefab = LevelDataSO.Section;
        cellPrefab = LevelDataSO.Cell;
        propGaps = LevelDataSO.PropGaps;
    }

    void checkSections(){
        levelBuilder.SpawnedSectionsList.ForEach(delegate(Section section){
            if(section.OrdinalNumber == iteration){
                section.gameObject.SetActive(true);
            }
        });
    }

    void setCamera()
    {
        cameraObject.PlayerTransform = levelBuilder.PlayerObject;
        cameraObject.SetPosition();
    }

    public void NextSectionNumber()
    {
        iteration++;
        currentNumber += increment;
        GlobalEventManager.OnCurrentNumberChange.Fire(currentNumber);
        checkSections();
    }

    void createLevel(bool fromEditor)
    {
        // Check if playground already exist
        GameObject playground = GameObject.Find("Playground");

        // If playground exist - delete it. That needs to spawn new one if you click a "Generate Level" button in inspector or start the level
        if (playground != null)
            GameManager.DestroyImmediate(playground);

        // Spawn a level parent container (playground)
        GameObject levelContainer = new GameObject("Playground");

        levelBuilder = new LevelBuilder(this, levelContainer.transform);
        levelBuilder.BuidLevel(fromEditor);
        levelBuilder.CreatePlayer();
    }

    public void RestartLevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }
}
