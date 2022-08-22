using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "New GameEvent", order = 52)]
public class GameEvent : ScriptableObject {
    
    UnityEvent _event = new UnityEvent();

    public void AddListener(UnityAction listener){
        _event.AddListener(listener);
    }

    public void Fire(){
        _event.Invoke();
    }
}