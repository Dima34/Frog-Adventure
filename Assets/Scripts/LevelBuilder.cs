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

    public Transform StartObject;
    public Transform FinishObject;
    public Transform PlayerObject;
    public List<Section> SpawnedSectionsList { get => spawnedSectionsList;}

    public LevelBuilder(
        GameManager gameManager,
        Transform parentObject
    ){
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
    }
    
    public void BuildLevel(bool fromEditor = false)
    {
        Vector3 levelDirection = (parentObject.localRotation * Vector3.up).normalized;
        spawnedSectionsList = new List<Section>();

        // Spawn and place start section
        StartObject = Object.Instantiate(startPrefab, parentObject.position, parentObject.localRotation);
        StartObject.SetParent(parentObject);

        for (int i = 1; i < iterationCount + 1; i++)
        {
            Vector3 newSectionPosiiton = StartObject.transform.position + (levelDirection * propGaps * i);

            // Spawn and place section in hierarchy 
            GameObject section = Object.Instantiate(sectionPrefab.gameObject, newSectionPosiiton, StartObject.rotation);
            section.transform.SetParent(parentObject, true);

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

        FinishObject = Object.Instantiate(finishPrefab, StartObject.transform.position + (levelDirection * propGaps * (iterationCount + 1)), StartObject.rotation);
        FinishObject.SetParent(parentObject, true);
    }

    public void CreatePlayer()
    {
        // Teleport player on start plate
        PlayerObject = Object.Instantiate(playerPrefab, StartObject.position, StartObject.rotation);
        PlayerObject.SetParent(parentObject);

        GlobalEventManager.OnPlayerCreated.Fire(PlayerObject);
    }
}
