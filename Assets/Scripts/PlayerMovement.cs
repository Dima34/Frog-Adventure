using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementTime = 1.25f;
    [Space(5)]
    [Header("Move animation  settings")]
    [SerializeField] Animator _moveAnimator;
    [SerializeField] float _delayedUnplugTime = 0.5f;

    GameManager gameManager;
    Camera mainCamera;
    Vector3 fromPos;
    Vector3 toPos;
    Collider2D collidedObject;

    bool _isMoving;

    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = GameObject.FindObjectOfType<GameManager>();
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
            clickPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            fromPos = transform.position;
            toPos = new Vector3(clickPos.x, clickPos.y, 0);

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
            transform.position = Vector3.Lerp(fromPos, toPos, Utils.EaseInOut(t / _movementTime));
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
        if (collidedObject == null)
        {
            restartLevel();
            return;
        }

        switch (collidedObject.tag)
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
        Cell cellScript = collidedObject.transform.GetComponent<Cell>();
        int cellNumber = cellScript.Number;

        if (cellNumber == gameManager.CurrentNumber)
        {
            gameManager.NextSectionNumber();
            cellScript.OnMoveEvent += followCell;
        }
        else
        {
            restartLevel();
        }
    }

    void followCell(Vector2 positionAddition)
    {
        transform.position += (Vector3)positionAddition;
        fromPos += (Vector3)positionAddition;
    }

    void restartLevel()
    {
        gameManager.RestartLevel();
    }

    void removeCellFollow(Collider2D collidedObject)
    {
        if (collidedObject.tag == "Cell")
        {
            Cell cellScript = collidedObject.GetComponent<Cell>();

            if (cellScript.OnMoveEvent != null)
            {
                cellScript.OnMoveEvent -= followCell;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collidedObj)
    {
        collidedObject = collidedObj;
    }

    private void OnTriggerExit2D(Collider2D collidedObj)
    {
        removeCellFollow(collidedObj);

        collidedObject = null;
    }
}

