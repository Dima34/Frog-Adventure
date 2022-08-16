using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;

    void Start()
    {
        setPosition();
    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
    }

    void setPosition(){
        transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10);
    }
}
