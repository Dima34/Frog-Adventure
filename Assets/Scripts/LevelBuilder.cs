using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder
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
    Cell cellPrefab;
    float propGaps;
    Vector3 levelDirection;

    public GameObject SectionsContainer;
    public Transform StartObject;
    public Transform FinishObject;
    public Transform PlayerObject;
    public List<Section> SpawnedSectionsList { get => spawnedSectionsList; }

    public LevelBuilder(
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
            sectionScript.SetUp(startNumber, increment, i);
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
        PlayerObject = Object.Instantiate(playerPrefab, new Vector3(0,0,-10), parentObject.rotation);
        PlayerObject.SetParent(parentObject);

        GlobalEventManager.OnPlayerCreated.Fire(PlayerObject);
    }

    public void HidePlayer(){
        PlayerObject.position = new Vector3(0,0,-10);
    }

    public void SpawnPlayer(){
        if(PlayerObject == null)
            Debug.LogError("You cannot spawn player which you don`t create. Make CreatePlayer() first.");
        if(StartObject == null)
            Debug.LogError("Start object isn`t created. Make CreateStartNFinish() first");
        
        PlayerObject.position = startPrefab.position + (-startPrefab.transform.right * (startPrefab.lossyScale.x * 0.75f));
        PlayerMovement playerMovement = PlayerObject.GetComponent<PlayerMovement>();

        playerMovement.MovePlayer(PlayerObject.position, StartObject.position);
    }
}
