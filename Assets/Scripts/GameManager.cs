using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelData LevelDataSO;

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
    [SerializeField] CameraMovement cameraObject;

    int startNumber;
    int increment;
    int iterationCount;
    Transform playerPrefab;
    Transform startPrefab;
    Transform finishPrefab;
    Transform sectionPrefab;
    Transform cellPrefab;
    float propGaps;

    Transform startObject;
    Transform finishObject;
    Transform playerObject;

    int currentNumber;

    void Start()
    {
        BuidLevelSequence();
    }

    void setCamera()
    {
        cameraObject.PlayerTransform = playerObject;
        cameraObject.SetPosition();
    }

    public void BuidLevelSequence(bool fromEditor = false)
    {
        setLevelData();
        createLevel(fromEditor);
        // setCamera();
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

    void createLevel(bool fromEditor)
    {
        // Check if playground already exist
        GameObject playground = GameObject.Find("Playground");

        // If playground exist - delete it. That needs to spawn new one if you click a "Generate Level" button in inspector or start the level
        if (playground != null)
            GameManager.DestroyImmediate(playground);

        // Spawn a level parent container (playground)
        GameObject levelContainer = new GameObject("Playground");

        buidLevel(levelContainer.transform, fromEditor);
        createPlayer(levelContainer.transform);
    }

    void buidLevel(Transform parentObject, bool fromEditor = false )
    {
        Vector2 levelDirection = (transform.localRotation * Vector2.up).normalized;

        // Spawn and place start section
        startObject = Instantiate(startPrefab, transform.position, transform.localRotation);
        startObject.SetParent(parentObject);

        for (int i = 1; i < iterationCount + 1; i++)
        {
            Vector3 newSectionPosiiton = (Vector2)startObject.transform.position + (levelDirection * propGaps * i);

            // Spawn and place section in hierarchy 
            Transform section = Instantiate(sectionPrefab, newSectionPosiiton, startObject.rotation);
            section.SetParent(parentObject, true);

            Section sectionScript = section.GetComponent<Section>();
            // Setup section
            sectionScript.SetUp(this, i);
            // Trigger cell spawn
            sectionScript.SpawnCells(cellPrefab);

            if (!fromEditor)
                // Disable section
                section.gameObject.SetActive(false);
        }

        Transform finishObject = Instantiate(finishPrefab, (Vector2)startObject.transform.position + (levelDirection * propGaps * (iterationCount + 1)), startObject.rotation);
        finishObject.SetParent(parentObject, true);
    }

    void createPlayer(Transform parentObject)
    {
        // Teleport player on start plate
        playerObject = Instantiate(playerPrefab, startObject.position, startPrefab.rotation);
        playerObject.SetParent(parentObject);
    }

    public void NextSectionNumber()
    {
        currentNumber += increment;
        GlobalEventManager.OnCurrentNumberChange.Fire(currentNumber);
        Debug.Log("Current number - " + currentNumber);
    }

    public void RestartLevel()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }
}
