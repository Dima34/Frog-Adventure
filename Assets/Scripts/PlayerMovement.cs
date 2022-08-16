using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementTime = 1.25f;
    
    Camera _mainCamera;

    bool _isMoving;

    Vector3 _fromPos;
    Vector3 _toPos;
    

    private void Start() {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 clickPos;

        if(Input.GetMouseButtonDown(0) && !_isMoving){
            clickPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _fromPos = transform.position;
            _toPos = new Vector3(clickPos.x, clickPos.y, 0);

            StartCoroutine("moveSequence");
            _isMoving = true;
        }        


    }

    IEnumerator moveSequence(){
        float t = 0;

        while (t < _movementTime){
            t+= Time.deltaTime;
            transform.position = Vector3.Lerp(_fromPos, _toPos, easeInOut(t / _movementTime));
            yield return null;
        }

        _isMoving = false;
    }

    float easeInOut(float x){
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }
}
