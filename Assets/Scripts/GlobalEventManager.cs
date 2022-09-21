using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GlobalEventManager
{
    public static class OnCurrentNumberChange{
        static Action listenerList;

        public static void AddListener(Action listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action listener){
            listenerList -= listener;
        }

        public static void Fire(){
            listenerList?.Invoke();
        }
    }

    public static class OnLevelBuilded{
        static Action listenerList;

        public static void AddListener(Action listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action listener){
            listenerList -= listener;
        }

        public static void Fire(){
            listenerList?.Invoke();
        }
    }

    public static class OnPlayerCreated{
        static Action<Transform> listenerList;

        public static void AddListener(Action<Transform> listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action<Transform> listener){
            listenerList -= listener;
        }

        public static void Fire(Transform playerObject){
            listenerList?.Invoke(playerObject);
        }
    }

    public static class OnCorrectCell{
        static Action<Collider2D> listenerList;

        public static void AddListener(Action<Collider2D> listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action<Collider2D> listener){
            listenerList -= listener;
        }

        public static void Fire(Collider2D cellObject){
            listenerList?.Invoke(cellObject);
        }
    }
}
