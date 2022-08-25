using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public int OrdinalNumber { get => ordinalNumber;}
    public int CorrectCellIndex { get => correctCellIndex; }
    public List<Cell> CellsList { get => cellsList; }

    [SerializeField] float _sideMarginSize = 2f;
    [SerializeField] int _cellAmount = 3;

    int ordinalNumber;
    int startNumber;
    int increment;
    int correctCellIndex;
    List<Cell> cellsList;

    public void SetUp(int startNumber, int increment, int ordinalNumber)
    {
        this.increment = increment;
        this.startNumber = startNumber;
        this.ordinalNumber = ordinalNumber;
    }

    public void SpawnCells(Cell cell)
    {
        if (_cellAmount < 1) _cellAmount = 1;
        cellsList = new List<Cell>();

        // Find cell half size
        float cellHalfSize = cell.transform.localScale.x / 2;
        // Find one side margin which includes margins and cell half (because when we spawn we specify the center coords. We need that to create cells on side and don`t calculate the half + center coords each time)
        float sideMargin = cellHalfSize + _sideMarginSize;
        // Calculate section width includes margin
        float includedSectionWidth = transform.localScale.x - sideMargin * 2;
        // Calctulate gaps between cells
        float cellGap = includedSectionWidth / (_cellAmount == 1 ? _cellAmount : (_cellAmount - 1));
        // Get section horizontal vector from center to left
        Vector3 localLeftVector = transform.localRotation * Vector3.left;
        // Get margin included left side coords
        Vector3 marginIncludedLeftPoint = transform.position + (localLeftVector * (includedSectionWidth / 2));

        // Get random correct cell posistion
        correctCellIndex = Random.Range(0, _cellAmount);
        // Calculate correct number value;
        int correctNumber = startNumber + (increment * ordinalNumber);

        // Create exception array to prevent cell number repeating
        List<int> createdNumberArray = new List<int>() { correctNumber };

        // Spawn cells
        for (int i = 0; i < _cellAmount; i++)
        {
            Vector3 spawnPoint = marginIncludedLeftPoint + (-localLeftVector * i * cellGap);

            GameObject spawnedCell = Instantiate(cell.gameObject, spawnPoint, transform.rotation);
            spawnedCell.transform.SetParent(transform);

            int cellNumber;

            // (n-1) till (n+cellAmount) except of correct number 
            if (correctCellIndex != i)
            {
                cellNumber = Utils.GetRandomExceptNumList(correctNumber - 1, correctNumber + _cellAmount, createdNumberArray);
                createdNumberArray.Add(cellNumber);
            }
            else
                cellNumber = correctNumber;

            Cell cellScript = spawnedCell.GetComponent<Cell>();

            setUpCell(cellScript, cellNumber);
            createdNumberArray.Add(correctNumber);
            cellsList.Add(cellScript);
        }
    }

    void setUpCell(Cell cell, int number)
    {
        cell.Number = number;
        cell.SetNumber();
    }

    public void LeaveCorrectCell(){
        Debug.Log("leave correct cell");
        for (int i = 0; i < cellsList.Count; i++)
        {
            if(i != correctCellIndex){
                GameObject.Destroy(cellsList[i].gameObject);
            }
        }
    }
}
