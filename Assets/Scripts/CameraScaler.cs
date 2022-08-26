using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] Vector2 _defaultResolution = new Vector2(570, 780);

    Camera cameraComponent;

    float initAspect;
    float initSize;

    private void Start() {
        cameraComponent = GetComponent<Camera>();
        initAspect = _defaultResolution.x / _defaultResolution.y;
        initSize = cameraComponent.orthographicSize;
    }

    private void Update() {
        float calculatedSize = initSize * (initAspect / cameraComponent.aspect);
        cameraComponent.orthographicSize = calculatedSize;
    }
}
