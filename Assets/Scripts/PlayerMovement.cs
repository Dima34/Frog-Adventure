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
    CameraMovement cameraMovement;
    Vector3 fromPos;
    Vector3 toPos;
    Collider2D collidedObject;

    bool _isMoving;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        HandleMove();
    }

    void HandleMove()
    {
        if (Input.GetMouseButtonDown(0) && !_isMoving && !cameraMovement.IsCameraMooving)
        {
            Vector3 pointToMove = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            pointToMove.z = 0;

            MovePlayer(transform.position, pointToMove);
        }
    }

    public void MovePlayer(Vector3 from, Vector3 to){
        StartCoroutine(MovePlayerSequence(from, to));
    }

    public IEnumerator MovePlayerSequence(Vector3 from, Vector3 to)
    {
        fromPos = from;
        toPos = to;
        _isMoving = true;
        float t = 0;

        StartCoroutine("handleAnimationTimeChange");

        while (t < _movementTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(fromPos, toPos, Utils.EaseInOut(t / _movementTime));
            yield return null;
        }

        _isMoving = false;
        checkUnderFeet();
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
                break;
            case "Start":
                break;
        }
    }

    private void restartLevel()
    {
        StartCoroutine(gameManager.restartLevelSequence());
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

