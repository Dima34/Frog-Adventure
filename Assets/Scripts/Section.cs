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
    [SerializeField] bool willCellsMove = false;
    [SerializeField] float cellMoveSpeed = 30f;

    public int ordinalNumber;
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

    float calcMarginIncludedSize(Cell cell){
        // Find cell half size
        float cellHalfSize = cell.transform.localScale.x / 2;
        // Find one side margin which includes margins and cell half (because when we spawn we specify the center coords. We need that to create cells on side and don`t calculate the half + center coords each time)
        float sideMargin = cellHalfSize + _sideMarginSize;
        // Calculate section width includes margin
        return transform.localScale.x - sideMargin * 2;
    }

    public void SpawnCells(Cell cell)
    {
        if (_cellAmount < 1) _cellAmount = 1;
        cellsList = new List<Cell>();

        float includedSectionWidth = calcMarginIncludedSize(cell);
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

    public void HideCells(){
        
    }

    public IEnumerator HideCellsSequence(){
        Task lastHideAnim = null;

        for (int i = 0; i < CellsList.Count; i++)
        {
            if(CellsList[i]){
                CellAnimation cellAnimation = CellsList[i].GetComponent<CellAnimation>();
                
                lastHideAnim = new Task(cellAnimation.HideCell());
            }            
        }

        yield return new WaitWhile(()=> lastHideAnim.Running);
    }

    void setUpCell(Cell cell, int number)
    {
        cell.Number = number;
        cell.SetNumber();
    }

    public void LeaveCorrectCell(){
        for (int i = 0; i < cellsList.Count; i++)
        {
            if(i != correctCellIndex){
                GameObject.Destroy(cellsList[i].gameObject);
            } else if(willCellsMove){
                StartCellMoveSequence(cellsList[i]);
            }
        }
    }

    public void StartCellMoveSequence(Cell cell){
        float moveWayLength = calcMarginIncludedSize(cell) - cell.transform.lossyScale.x;
        Vector2 leftPoint = transform.position - (transform.right * moveWayLength / 2);
        Vector2 rightPoint = transform.position + (transform.right * moveWayLength / 2);

        StartCoroutine(cellMovement(cell, leftPoint, rightPoint));
    }

    IEnumerator cellMovement(Cell cell, Vector2 leftPoint, Vector2 rightPoint){        
        bool isMovingLeft = true;

        float movingSpeed = cellMoveSpeed / 10;
        float leftX = leftPoint.x;
        float rightX = rightPoint.x;
        float currentX = cell.transform.position.x;

        while (true)
        {
            if(currentX < leftX){
                currentX = leftX;
                isMovingLeft = !isMovingLeft;
            } else if(currentX > rightX){
                currentX = rightX;
                isMovingLeft = !isMovingLeft;
            }
            
            float xAddition = (movingSpeed * Time.deltaTime * (isMovingLeft ? 1 : -1));
            cell.OnMoveEvent?.Invoke(new Vector2(xAddition, 0));

            currentX = currentX + xAddition;
            cell.transform.position = new Vector2(currentX ,cell.transform.position.y);            
            yield return null;
        }

    }
}
