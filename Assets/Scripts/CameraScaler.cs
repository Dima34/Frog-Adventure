using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] Vector2 _defaultResolution = new Vector2(570, 780);

    Camera cameraComponent;

    public void ScaleCamera(){
        cameraComponent = GetComponent<Camera>();
        float initAspect = _defaultResolution.x / _defaultResolution.y;
        float initSize = cameraComponent.orthographicSize;

        float calculatedSize = initSize * (initAspect / cameraComponent.aspect);
        cameraComponent.orthographicSize = calculatedSize;
    }
}
