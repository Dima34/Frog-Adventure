using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    Transform parentObject;
    int startNumber;
    int increment;
    int iterationCount;
    Transform playerPrefab;
    Transform startPrefab;
    Transform finishPrefab;
    Transform sectionPrefab;
    Transform cellPrefab;
    float propGaps;

    public Transform StartObject;
    public Transform FinishObject;
    public Transform PlayerObject;

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
    
    public void BuidLevel(bool fromEditor = false)
    {
        Vector2 levelDirection = (parentObject.localRotation * Vector2.up).normalized;

        // Spawn and place start section
        StartObject = Instantiate(startPrefab, parentObject.position, parentObject.localRotation);
        StartObject.SetParent(parentObject);

        for (int i = 1; i < iterationCount + 1; i++)
        {
            Vector3 newSectionPosiiton = (Vector2)StartObject.transform.position + (levelDirection * propGaps * i);

            // Spawn and place section in hierarchy 
            Transform section = Instantiate(sectionPrefab, newSectionPosiiton, StartObject.rotation);
            section.SetParent(parentObject, true);

            Section sectionScript = section.GetComponent<Section>();
            // Setup section
            sectionScript.SetUp(startNumber, increment, i);
            // Trigger cell spawn
            sectionScript.SpawnCells(cellPrefab);

            if (!fromEditor)
                // Disable section
                section.gameObject.SetActive(false);
        }

        Transform finishObject = Instantiate(finishPrefab, (Vector2)StartObject.transform.position + (levelDirection * propGaps * (iterationCount + 1)), StartObject.rotation);
        finishObject.SetParent(parentObject, true);
    }

    public void CreatePlayer()
    {
        // Teleport player on start plate
        PlayerObject = Instantiate(playerPrefab, StartObject.position, StartObject.rotation);
        PlayerObject.SetParent(parentObject);
    }
}
