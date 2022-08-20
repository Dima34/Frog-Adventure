using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] GameObject _cell;
    [SerializeField] float _sizeMargin = 2f;

    public int OrdinalNumber;

    public void SpawnCells()
    {
        // Find cell half size
        float cellHalfSize = _cell.transform.localScale.x / 2;

        // Find section half size
        float sectionHalfSize = transform.localScale.x / 2;

        // Find corner cell pos
        Vector2 rightSectionSide = transform.position + (transform.localRotation * (Vector3.right * (sectionHalfSize - cellHalfSize - _sizeMargin)));
        Vector2 leftSectionSide = transform.position + (transform.localRotation * (-Vector3.right * (sectionHalfSize - cellHalfSize - _sizeMargin)));

        Instantiate(_cell, leftSectionSide, transform.rotation).transform.SetParent(transform);
        Instantiate(_cell, transform.position, transform.rotation).transform.SetParent(transform);
        Instantiate(_cell, rightSectionSide, transform.rotation).transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
