using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] int _startNumber = 0;
    [SerializeField] int _increment = 1;
    [SerializeField] int _iterationCount = 10;
    [SerializeField] LevelManager _levelManagerDataSO;

    [Header("Level Generation Settings")]
    [SerializeField] Transform _player;
    [SerializeField] Transform _startPrefab;
    [SerializeField] Transform _finishPrefab;
    [SerializeField] Transform _section;
    [SerializeField] Transform _cell;
    
    [Tooltip("Space between start - jumprops, jumprops - jumprops, jumprops - finish")]
    [SerializeField] float _propGaps = 20;

    
    void Start()
    {
        StartBuildSequence();
    }

    public void StartBuildSequence(){
        writeDataToSO();
        initializeLevelManager();
        BuidLevel();
    }

    public void BuidLevel(){
        Vector2 levelDirection = (_startPrefab.localRotation * Vector2.up).normalized;

        // Check if playground already exist
        GameObject playground = GameObject.Find("Playground");

        // If playground exist - delete it. That needs to spawn new one if you click a "Generate Level" button in inspector or start the level
        if(playground != null){
            GameManager.DestroyImmediate(playground);
        }

        // Create&spawn a level parent container
        GameObject levelContainer = new GameObject("Playground");
        // levelContainer = Instantiate(levelContainer, _startPrefab.transform.position, _startPrefab.transform.localRotation);
        
        // Spawn and place start section
        Transform startObject = Instantiate(_startPrefab, transform.position, transform.localRotation);       
        startObject.SetParent(levelContainer.transform);

        // Teleport player into start plate
        _player.position = startObject.position;


        for (int i = 1; i < _iterationCount + 1; i++)
        {
            Vector3 newSectionPosiiton = (Vector2)startObject.transform.position + (levelDirection * _propGaps * i);

            // Spawn and place section in hierarchy 
            Transform section = Instantiate(_section, newSectionPosiiton, startObject.rotation);
            section.SetParent(levelContainer.transform, true);

            Section sectionScript = section.GetComponent<Section>();
            // Set a ordinal number
            sectionScript.OrdinalNumber = i;
            // Trigger cell spawn
            sectionScript.SpawnCells(_cell);
        }

        Transform finishObject = Instantiate(_finishPrefab, (Vector2)startObject.transform.position + (levelDirection * _propGaps * 10), startObject.rotation);  
        finishObject.SetParent(levelContainer.transform, true);
    }

    void initializeLevelManager(){
        _levelManagerDataSO.SetCurrentNumber(_startNumber + _increment);
    }

    void writeDataToSO(){
        _levelManagerDataSO.SetIncrement(_increment);
        _levelManagerDataSO.SetIterationCount(_iterationCount);
        _levelManagerDataSO.SetStartNumber(_startNumber);
    }
}
