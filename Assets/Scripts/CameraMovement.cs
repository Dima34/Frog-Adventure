using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform PlayerTransform;

    public void SetPosition(){
        transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, -10);
    }
}
