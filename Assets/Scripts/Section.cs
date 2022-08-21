using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] float _sideMarginSize = 2f;
    [SerializeField] int _cellAmount = 3;
    [SerializeField] LevelManager _levelManagerDataSO;

    // Commented for development debug
    // [HideInInspector] 
    public int OrdinalNumber;

    public void SpawnCells(Transform cell)
    {
        if (_cellAmount < 1) _cellAmount = 1;

        int startNumber = _levelManagerDataSO.StartNumber;
        int increment = _levelManagerDataSO.Increment;

        // Find cell half size
        float cellHalfSize = cell.localScale.x / 2;
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

        // Get correct cell posistion
        int correctCellPosition = getRandomCorrectCellNumber();
        // Calculate correct number value;
        int correctNumber = startNumber + (increment * OrdinalNumber);

        // Create exception array to prevent cell number repeating
        List<int> createdNumberArray = new List<int>(){correctNumber};

        // Spawn cells
        for (int i = 0; i < _cellAmount; i++)
        {
            Vector3 spawnPoint = marginIncludedLeftPoint + (-localLeftVector * i * cellGap);

            Transform spawnedCell = Instantiate(cell, spawnPoint, transform.rotation);
            spawnedCell.SetParent(transform);

            int cellNumber;

            // (n-1) till (n+cellAmount) except of correct number 
            if (correctCellPosition != i)
            {
                cellNumber = getRandomExcept(correctNumber - 1, correctNumber + _cellAmount, createdNumberArray);
                createdNumberArray.Add(cellNumber);
            }
            else
                cellNumber = correctNumber;

            setUpCell(spawnedCell, cellNumber);
            createdNumberArray.Add(correctNumber);
        }
    }

    void setUpCell(Transform cell, int number)
    {
        Cell cellScript = cell.GetComponent<Cell>();
        cellScript.Number = number;
        cellScript.SetNumber();
    }

    int getRandomCorrectCellNumber()
    {
        return Random.Range(0, _cellAmount);
    }

    int getRandomExcept(int min, int max, List<int> exceptNumberList)
    {
        int number;

        do
        {
            number = Random.Range(min, max);
        } while (exceptNumberList.Contains(number));

        return number;
    }
}
