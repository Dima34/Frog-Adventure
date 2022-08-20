using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] float _sizeMargin = 2f;
    [SerializeField] int _cellAmount = 3;

    public int OrdinalNumber;

    public void SpawnCells(Transform cell)
    {
        // Find cell half size
        float cellHalfSize = cell.localScale.x / 2;

        // Find one side margin which includes margins and cell half (because when we spawn we specify the center coords. We need that to create cells on side and don`t calculate the half + center coords each time)
        float sideMargin = cellHalfSize / 2 - _sizeMargin;

        // Calculate section width includes margin
        float includedSectionWidth = transform.localScale.x - sideMargin * 2;

        // Calctulate gaps between cells
        float cellGap = includedSectionWidth / _cellAmount;

        // Get section rotation vector
        Vector3 localLeftVector = transform.localRotation * Vector3.left;        
        
        // Get margin included side coords
        
        

        // Find section half size
        float sectionHalfSize = transform.localScale.x / 2;

        // Find corner cell pos
        Vector2 rightSectionSide = transform.position + (transform.localRotation * (Vector3.right * (sectionHalfSize - cellHalfSize - _sizeMargin)));
        Vector2 leftSectionSide = transform.position + (transform.localRotation * (-Vector3.right * (sectionHalfSize - cellHalfSize - _sizeMargin)));

        Instantiate(cell, leftSectionSide, transform.rotation).transform.SetParent(transform);
        Instantiate(cell, transform.position, transform.rotation).transform.SetParent(transform);
        Instantiate(cell, rightSectionSide, transform.rotation).transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
