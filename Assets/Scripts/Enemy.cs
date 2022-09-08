using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[Serializable]
public class Enemy : MonoBehaviour
{
    public float MovementSpeed = 2f;

    public IEnumerator MoveTo(Vector3 moveToPoint){
        Vector3 currentPosition = transform.position;
        moveToPoint = new Vector3(currentPosition.x, moveToPoint.y - transform.localScale.y,currentPosition.z);

        float distLenght = Vector3.Distance(transform.position, moveToPoint);
        float timeToMove = distLenght / MovementSpeed;
        float t = 0;

        while(t < timeToMove){
            t += Time.deltaTime;
            float i = t / timeToMove;
            
            transform.position = Vector2.Lerp(currentPosition, moveToPoint, i);
            yield return null;
        }
    }
}

