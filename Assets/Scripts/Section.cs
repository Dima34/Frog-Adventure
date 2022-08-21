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

        // Get correct cell number
        int correctCellNum = getRandomCorrectCellNumber();

        // Spawn cells
        for (int i = 0; i < _cellAmount; i++)
        {
            Vector3 spawnPoint = marginIncludedLeftPoint + (-localLeftVector * i * cellGap);

            Transform spawnedCell = Instantiate(cell, spawnPoint, transform.rotation);
            spawnedCell.SetParent(transform);

            // Calculate correct number value;
            int cellNum = startNumber + (increment * OrdinalNumber);

            // (n-1) till (n+cellAmount) except of correct number 
            if (correctCellNum != i)
                cellNum = getRandomExcept(cellNum - 1, cellNum + _cellAmount, cellNum);

            setUpCell(spawnedCell, cellNum);
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

    int getRandomExcept(int min, int max, int exceptNumber){
        int number;

        do
        {
            number = Random.Range(min, max);
        } while (number == exceptNumber);

        return number;
    }
}
