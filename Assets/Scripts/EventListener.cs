using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] GameEvent _eventToListen;
    [SerializeField] UnityAction _responce;

    private void OnEnable() {
        _eventToListen.AddListener(raiseEvent);
    }

    private void raiseEvent(){
        _responce.Invoke();
    }
}
