using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CameraScaler))]
public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] float _cameraMoveSpeed = 20f;

    CameraScaler cameraScaler;
    Camera cameraObject;
    Vector2 currentCameraPosition;
    Transform playerObject;
    GameObject levelContainer;
    Transform startObject;
    Transform finishObject;
    Vector2 finishTop;
    Vector2 startBottom;
    float cameraInUnitHeight;

    bool isCameraMooving = false;
    bool nextInstantCameraMove = false;
    bool isLevelBuilded = false;
    bool isCameraScaled = false;

    public bool IsCameraMooving { get => isCameraMooving;}

    private void Awake()
    {
        GetComponent<CameraScaler>().ScaleCamera();
        GlobalEventManager.OnLevelBuilded.AddListener(Initialize);
        GlobalEventManager.OnPlayerCreated.AddListener(setPlayerObject);
        GlobalEventManager.OnCurrentNumberChange.AddListener(checkPosition);
    }

    private void OnDestroy() {
        ClearEventListeners();
    }

    public void ClearEventListeners(){
        GlobalEventManager.OnLevelBuilded.RemoveListener(Initialize);
        GlobalEventManager.OnPlayerCreated.RemoveListener(setPlayerObject);
        GlobalEventManager.OnCurrentNumberChange.RemoveListener(checkPosition);
    }

    void Initialize()
    {
        initializeStartNFinish();
        initializeCamera();
        levelContainer = _gameManager.LevelContainer;
    }

    void initializeCamera()
    {
        cameraObject = GetComponent<Camera>();
        cameraInUnitHeight = cameraObject.orthographicSize * 2;
        currentCameraPosition = cameraObject.transform.position;

        SetCameraStartPosition(true);
    }

    void initializeStartNFinish()
    {
        startObject = _gameManager.LevelManager.StartObject;
        finishObject = _gameManager.LevelManager.FinishObject;

        finishTop = finishObject.position - (-finishObject.up * (finishObject.localScale.y / 2));
        startBottom = startObject.position - (startObject.up * (startObject.localScale.y / 2));
    }

    void setPlayerObject(Transform player){
        playerObject = player;
    }

    public void SetCameraStartPosition(bool intantMove = false)
    {
        nextInstantCameraMove = intantMove;
        currentCameraPosition = cameraObject.transform.position;
        
        Vector2 newCameraPosition = startBottom + (Vector2)(cameraObject.transform.up * (cameraInUnitHeight / 2));

        moveVertical(cameraObject, currentCameraPosition, newCameraPosition, finishTop);
    }
    
    void checkPosition()
    {
        float yPlayerPosition = cameraObject.WorldToViewportPoint(playerObject.position).y;

        if (yPlayerPosition > 0.125)
        {
            currentCameraPosition = cameraObject.transform.position;
            // Get current player posistion
            Vector2 currentPlayerPosition = playerObject.transform.position;
            Vector2 cameraShift = cameraObject.transform.up * (cameraInUnitHeight / 4);
            moveVertical(cameraObject, currentCameraPosition, currentPlayerPosition + cameraShift, finishTop);
        }
    }

    void moveVertical(Camera cameraObject, Vector2 a, Vector2 b, Vector2 finishDeathCorner)
    {
        if (!isCameraMooving)
        {
            isCameraMooving = true;
            StartCoroutine(moveVerticalCoroutine(cameraObject, a, b, finishDeathCorner));
        }
    }

    IEnumerator moveVerticalCoroutine(Camera cameraObject, Vector2 a, Vector2 b, Vector2 finishDeathCorner)
    {
        float aY = a.y;
        float bY = b.y;
        float t = 0;

        while (t < 1)
        {
            float finishObjectRelativeY = cameraObject.WorldToViewportPoint(finishDeathCorner).y * cameraInUnitHeight;

            if(finishObjectRelativeY <= cameraInUnitHeight && t != 0){
                bY = (finishDeathCorner - (Vector2)(cameraObject.transform.up * (cameraInUnitHeight / 2))).y;
                nextInstantCameraMove = true;
            }

            if (nextInstantCameraMove)
            {
                t = 1;
                nextInstantCameraMove = false;
            } else
            {
                t += (_cameraMoveSpeed / 10) * Time.deltaTime;
            }

            float newY = Mathf.Lerp(aY, bY, t);
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, -10);

            yield return null;
        }

        isCameraMooving = false;
    }
}
