using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementTime = 1.25f;
    [Space(5)]
    [Header("Move animation  settings")]
    [SerializeField] Animator _moveAnimator;
    [SerializeField] float _trimStart = 0.1f;
    [SerializeField] float _trimEnd = 0.1f;


    Camera _mainCamera;

    bool _isMoving;

    Vector3 _fromPos;
    Vector3 _toPos;

    float _animationLengthTime = 0.2f;
    


    private void Start()
    {
        _mainCamera = Camera.main;

        // setAnimationSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    void setAnimationSpeed(){
        float croppedMovementTime = _movementTime - _trimStart - _trimEnd;

        if(croppedMovementTime < 0 )
            croppedMovementTime = 1;

        float animationTimeMultiplier = croppedMovementTime / _animationLengthTime;
        float summaryMultiplier = croppedMovementTime / animationTimeMultiplier;

        Debug.Log("Anim time " + summaryMultiplier);

        _moveAnimator.SetFloat("JumpSpeed", summaryMultiplier);
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
            transform.position = Vector3.Lerp(_fromPos, _toPos, easeInOut(t / _movementTime));
            yield return null;
        }

        _isMoving = false;
    }
    void startAnimationSequence(){
        StartCoroutine("handleAnimationTimeChange");
    }

    IEnumerator handleAnimationTimeChange(){
        _moveAnimator.SetBool("IsJumping", true);
        
        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime;
            _moveAnimator.SetFloat("JumpTime", t);
            yield return null;
        }

        _moveAnimator.SetBool("IsJumping", false);        
    }

    float easeInCirc(float x) {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }

    float easeInOut(float x)
    {
        return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
    }
}

// git commit -m "moving system with animation added"
