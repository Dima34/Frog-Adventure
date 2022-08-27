using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    Camera cameraObject;
    Transform playerObject;
    GameObject levelContainer;
    Transform startObject;
    Transform finishObject;
    float cameraInUnitHeight;

    bool isCameraMooving = false;
    bool nextInstantCameraMove = false;

    private void Awake() {
        GlobalEventManager.OnLevelBuilded.AddListener(Initialize);
        GlobalEventManager.OnPlayerCreated.AddListener((Transform player) => {
            playerObject = player;
            nextInstantCameraMove = true;
        });
        GlobalEventManager.OnCurrentNumberChange.AddListener(checkPosition);
    }

    void Initialize(){
        cameraObject = GetComponent<Camera>();
        cameraInUnitHeight = cameraObject.orthographicSize * 2;
        startObject = gameManager.LevelBuilder.StartObject;
        finishObject = gameManager.LevelBuilder.FinishObject;
        levelContainer = gameManager.LevelContainer;
    }

    

    void checkPosition(){
        if(playerObject == null){
            Debug.LogError("Player variable in camera movement is null *check position method. Try assign player first");
            return;
        }

        float yPlayerPosition = cameraObject.WorldToViewportPoint(playerObject.position).y;
        float cameraHeight = cameraObject.scaledPixelHeight;

        Vector2 finishTop = finishObject.position - (-finishObject.up * (finishObject.localScale.y / 2));
        Vector2 startBottom = startObject.position - (startObject.up * (startObject.localScale.y / 2));

        List<Vector2> deathPoints = new List<Vector2>();
        deathPoints.Add(finishTop);
        deathPoints.Add(startBottom);

        if(yPlayerPosition > 0.125){
            // Get camera center in world position
            Vector2 currentCameraPosition = cameraObject.transform.position;
            // Get current player posistion
            Vector2 currentPlayerPosition = playerObject.transform.position;
            Vector2 cameraShift = cameraObject.transform.up * (cameraInUnitHeight / 4);
            moveVertical(cameraObject ,currentCameraPosition, currentPlayerPosition + cameraShift, deathPoints);
        }
    }

    void moveVertical(Camera cameraObject, Vector2 a, Vector2 b, List<Vector2> deathPoints){
        if(!isCameraMooving){
            isCameraMooving = true;

            StartCoroutine(moveVerticalCoroutine(cameraObject, a, b, deathPoints));
        }
    }

    IEnumerator moveVerticalCoroutine(Camera cameraObject, Vector2 a, Vector2 b, List<Vector2> deathPoints){
        float aY = a.y;
        float bY = b.y;
        float t = 0;

        while (t <= 1)
        {
            for (int i = 0; i < deathPoints.Count; i++)
            {
                Vector2 deathPoint = deathPoints[i];
                float objectRelativeY = MathF.Round(cameraObject.WorldToViewportPoint(deathPoint).y * cameraInUnitHeight, 2);
                float pointY = deathPoint.y;

                if(objectRelativeY < 0) continue;

                if(objectRelativeY <= cameraInUnitHeight / 2 && objectRelativeY > 0)
                    bY = (cameraObject.transform.position + (objectRelativeY * cameraObject.transform.up)).y;

                if(objectRelativeY > cameraInUnitHeight / 2 && objectRelativeY <= cameraInUnitHeight){
                    isCameraMooving = false;
                    yield break;
                }
            }

            if(nextInstantCameraMove){
                t = 1;
                nextInstantCameraMove = false;
            } else{
                t += Time.deltaTime;
            }
            float newY = Mathf.Lerp(aY, bY, t);
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, newY, -10);
            yield return null;
        }

        isCameraMooving = false;
    }
}
