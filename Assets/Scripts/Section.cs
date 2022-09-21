using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public int OrdinalNumber { get => ordinalNumber; }
    public int CorrectCellIndex { get => correctCellIndex; }
    public List<Cell> CellsList { get => cellsList; }
    public int ordinalNumber;

    float sideMarginSize;
    int cellAmount;
    bool willCellsMove;
    float cellMoveSpeed;
    int startNumber;
    int increment;
    int correctCellIndex;
    List<Cell> cellsList;
    Coroutine cellMovementProcess;
    Task lastHideAnim = null;

    public void SetUp(
        int startNumber,
        int increment,
        int ordinalNumber,
        float sideMarginSize,
        int cellAmount,
        bool willCellsMove,
        float cellMoveSpeed
    )
    {
        this.increment = increment;
        this.startNumber = startNumber;
        this.ordinalNumber = ordinalNumber;
        this.sideMarginSize = sideMarginSize;
        this.cellAmount = cellAmount;
        this.willCellsMove = willCellsMove;
        this.cellMoveSpeed = cellMoveSpeed;
    }

    void setUpCell(Cell cell, int number)
    {
        cell.Number = number;
        cell.SetNumber();
    }

    public void SpawnCells(Cell cell)
    {
        if (cellAmount < 1) cellAmount = 1;
        cellsList = new List<Cell>();

        float includedSectionWidth = Utils.CalcMarginIncludedSize(cell, transform.localScale.x, sideMarginSize);
        // Calctulate gaps between cells
        float cellGap = includedSectionWidth / (cellAmount == 1 ? cellAmount : (cellAmount - 1));
        // Get section horizontal vector from center to left
        Vector3 localLeftVector = transform.localRotation * Vector3.left;
        // Get margin included left side coords
        Vector3 marginIncludedLeftPoint = transform.position + (localLeftVector * (includedSectionWidth / 2));


        // Get random correct cell posistion
        correctCellIndex = Random.Range(0, cellAmount);
        // Calculate correct number value;
        int correctNumber = startNumber + (increment * ordinalNumber);

        // Create exception array to prevent cell number repeating
        List<int> createdNumberArray = new List<int>() { correctNumber };

        // Spawn cells
        for (int i = 0; i < cellAmount; i++)
        {
            Vector3 spawnPoint = marginIncludedLeftPoint + (-localLeftVector * i * cellGap);

            GameObject spawnedCell = Instantiate(cell.gameObject, spawnPoint, transform.rotation);
            spawnedCell.transform.SetParent(transform);

            int cellNumber;

            // (n-1) till (n+cellAmount) except of correct number 
            if (correctCellIndex != i)
            {
                cellNumber = Utils.GetRandomExceptNumList(correctNumber - 1, correctNumber + cellAmount, createdNumberArray);
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

    public void DestroyCells(){
        if(cellsList != null){
            foreach (var cell in cellsList)
            {
                if(cell != null){
                    if(cellMovementProcess != null){
                        StopCoroutine(cellMovementProcess);
                        cellMovementProcess = null;
                    }
                    Destroy(cell.gameObject);
                }                    
            }

            cellsList = null;
        }
    }

    public IEnumerator HideCells()
    {
        for (int i = 0; i < CellsList.Count; i++)
        {
            if(cellsList[i]){
                HideCell(cellsList[i]);
            }
        }

        yield return new WaitWhile(()=>lastHideAnim.Running);
    }

    public void HideCell(Cell cell){
        CellAnimation cellAnimation = cell.GetComponent<CellAnimation>();

        lastHideAnim = new Task(cellAnimation.HideCell());
    }

    public IEnumerator LeaveCorrectCell()
    {
        for (int i = 0; i < cellsList.Count; i++)
        {
            if (i != correctCellIndex)
            {
                HideCell(cellsList[i]);
            }
            else if (willCellsMove)
            {
                StartCellMoveSequence(cellsList[i]);
            }
        }

        yield return new WaitWhile(()=>lastHideAnim.Running);

        for (int i = 0; i < cellsList.Count; i++)
        {
            if (i != correctCellIndex)
                Destroy(cellsList[i].gameObject);
        }
    }

    public void StartCellMoveSequence(Cell cell)
    {
        float moveWayLength = Utils.CalcMarginIncludedSize(cell, transform.localScale.x, sideMarginSize) - cell.transform.lossyScale.x;
        Vector2 leftPoint = transform.position - (transform.right * moveWayLength / 2);
        Vector2 rightPoint = transform.position + (transform.right * moveWayLength / 2);

        cellMovementProcess = StartCoroutine(cellMovement(cell, leftPoint, rightPoint));
    }

    IEnumerator cellMovement(Cell cell, Vector2 leftPoint, Vector2 rightPoint)
    {
        bool isMovingLeft = true;

        float movingSpeed = cellMoveSpeed / 10;
        float leftX = leftPoint.x;
        float rightX = rightPoint.x;
        float currentX = cell.transform.position.x;

        while (true)
        {
            if (currentX < leftX)
            {
                currentX = leftX;
                isMovingLeft = !isMovingLeft;
            }
            else if (currentX > rightX)
            {
                currentX = rightX;
                isMovingLeft = !isMovingLeft;
            }

            float xAddition = (movingSpeed * Time.deltaTime * (isMovingLeft ? 1 : -1));
            cell.OnMoveEvent?.Invoke(new Vector2(xAddition, 0));

            currentX = currentX + xAddition;
            cell.transform.position = new Vector2(currentX, cell.transform.position.y);
            yield return null;
        }

    }
}
