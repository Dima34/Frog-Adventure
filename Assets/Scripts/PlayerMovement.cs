using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameManagerData gameManagerDataSO;
    [SerializeField] float _movementTime = 1.25f;
    [Space(5)]
    [Header("Move animation  settings")]
    [SerializeField] Animator _moveAnimator;


    Camera _mainCamera;
    Vector3 _fromPos;
    Vector3 _toPos;
    Collider2D _collidedObject;

    bool _isMoving;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        HandleMove();
    }

    void HandleMove()
    {
        Vector3 clickPos;

        if (Input.GetMouseButtonDown(0) && !_isMoving)
        {
            clickPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _fromPos = transform.position;
            _toPos = new Vector3(clickPos.x, clickPos.y, 0);

            StartCoroutine("moveSequence");
            _isMoving = true;
        }
    }

    IEnumerator moveSequence()
    {
        float t = 0;

        startAnimationSequence();

        while (t < _movementTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(_fromPos, _toPos, Utils.EaseInOut(t / _movementTime));
            yield return null;
        }

        _isMoving = false;
        checkUnderFeet();
    }
    void startAnimationSequence()
    {
        StartCoroutine("handleAnimationTimeChange");
    }

    IEnumerator handleAnimationTimeChange()
    {
        _moveAnimator.SetBool("IsJumping", true);

        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime;
            _moveAnimator.SetFloat("JumpTime", Utils.EaseInCirc(t));
            yield return null;
        }

        _moveAnimator.SetBool("IsJumping", false);
    }

    void checkUnderFeet()
    {
        if (_collidedObject == null)
            return;

        switch (_collidedObject.tag)
        {
            case "Cell":
                cellUnderFeet();
                break;
            case "Finsh":
                Debug.Log("obj is cell");
                break;
            case "Start":
                Debug.Log("this is still start");
                break;
        }
    }

    void cellUnderFeet()
    {
        Transform cellTransform = _collidedObject.transform;
        int cellNumber = cellTransform.GetComponent<Cell>().Number;

        if(cellNumber == gameManagerDataSO.CurrentNumber){
            gameManagerDataSO.NextSectionNumber();
        } else{
            Debug.Log("Wrong cell number");
        }
    }

    void restartLevel(){
        
    }

    private void OnTriggerEnter2D(Collider2D collidedObj)
    {
        _collidedObject = collidedObj;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _collidedObject = null;
    }
}

