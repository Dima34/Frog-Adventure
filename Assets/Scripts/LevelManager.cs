using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    Transform parentObject;
    int startNumber;
    int increment;
    int iterationCount;
    Transform playerPrefab;
    Transform startPrefab;
    Transform finishPrefab;
    Section sectionPrefab;
    List<Section> spawnedSectionsList;
    float sectionSideMarginSize;
    int cellInSectionAmount;
    bool willCellsMove;
    float cellMoveSpeed;
    Cell cellPrefab;
    float propGaps;
    Vector3 levelDirection;
    Transform spawnedPlayer;
    

    public GameObject SectionsContainer;
    public Transform StartObject;
    public Transform FinishObject;
    public List<Section> SpawnedSectionsList { get => spawnedSectionsList; }
    public Transform SpawnedPlayer { get => spawnedPlayer;}

    public LevelManager(
        GameManager gameManager,
        Transform parentObject
    )
    {
        this.parentObject = parentObject;
        startNumber = gameManager.StartNumber;
        increment = gameManager.Increment;
        iterationCount = gameManager.IterationCount;
        playerPrefab = gameManager.PlayerPrefab;
        startPrefab = gameManager.StartPrefab;
        finishPrefab = gameManager.FinishPrefab;
        sectionPrefab = gameManager.SectionPrefab;
        sectionSideMarginSize = gameManager.SectionSideMarginSize;
        cellInSectionAmount = gameManager.CellInSectionAmount;
        willCellsMove = gameManager.WillCellsMove;
        cellMoveSpeed = gameManager.CellMoveSpeed;
        cellPrefab = gameManager.CellPrefab;
        propGaps = gameManager.PropGaps;
        levelDirection = parentObject.localRotation * Vector3.up;
    }

    public void BuildLevel(bool fromEditor = false)
    {
        CreateStartNFinish();
        CreateSections(fromEditor);
    }

    public void CreateStartNFinish()
    {
        // Spawn and place start section
        StartObject = Object.Instantiate(startPrefab, parentObject.position, parentObject.localRotation);
        StartObject.SetParent(parentObject);

        FinishObject = Object.Instantiate(finishPrefab, StartObject.transform.position + (levelDirection * propGaps * (iterationCount + 1)), StartObject.rotation);
        FinishObject.SetParent(parentObject, true);
    }

    public void CreateSections(bool fromEditor = false)
    {
        string containerName = "Sections container";

        if(SectionsContainer != null)
            GameObject.Destroy(SectionsContainer);
        
        SectionsContainer = new GameObject(containerName);
        SectionsContainer.transform.SetParent(parentObject);

        spawnedSectionsList = new List<Section>();

        for (int i = 1; i < iterationCount + 1; i++)
        {
            Vector3 newSectionPosition = StartObject.transform.position + (levelDirection * propGaps * i);

            // Spawn and place section in hierarchy 
            GameObject section = Object.Instantiate(sectionPrefab.gameObject, newSectionPosition, StartObject.rotation);
            section.transform.SetParent(SectionsContainer.transform, true);

            Section sectionScript = section.GetComponent<Section>();
            // Setup section
            sectionScript.SetUp(
                startNumber, 
                increment, 
                i,
                sectionSideMarginSize,
                cellInSectionAmount,
                willCellsMove,
                cellMoveSpeed              
            );
            // Trigger cell spawn
            sectionScript.SpawnCells(cellPrefab);
            // Add section to spawned sections list
            spawnedSectionsList.Add(sectionScript);

            if (!fromEditor)
                // Disable section
                section.gameObject.SetActive(false);
        }
    }

    public void CreatePlayer()
    {
        // Teleport player on start plate
        spawnedPlayer = Object.Instantiate(playerPrefab, new Vector3(0,0,-10), parentObject.rotation);
        spawnedPlayer.SetParent(parentObject);

        // Disable movement
        spawnedPlayer.GetComponent<PlayerMovement>().enabled = false;

        GlobalEventManager.OnPlayerCreated.Fire(spawnedPlayer);
    }

    public void SpawnPlayer(){
        if(spawnedPlayer == null)
            Debug.LogError("You cannot spawn player which you don`t create. Make CreatePlayer() first.");
        if(StartObject == null)
            Debug.LogError("Start object isn`t created. Make CreateStartNFinish() first");
        
        spawnedPlayer.position = startPrefab.position + (-startPrefab.transform.right * (startPrefab.lossyScale.x * 0.75f));
        Player player = spawnedPlayer.GetComponent<Player>();
        player.EnemyCollided = false;
        PlayerMovement playerMovement = spawnedPlayer.GetComponent<PlayerMovement>();


        playerMovement.MovePlayer(spawnedPlayer.position, StartObject.position);
        spawnedPlayer.GetComponent<PlayerMovement>().enabled = true;
    }

    public IEnumerator HideSections(){
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

        for (int i = 0; i < spawnedSectionsList.Count; i++)
        {
            Section section = spawnedSectionsList[i].GetComponent<Section>();
            
            section.gameObject.SetActive(false);
        }
    }
}
