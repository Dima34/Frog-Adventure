using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GlobalEventManager
{
    public static class OnCurrentNumberChange{
        static Action<int> listenerList;

        public static void AddListener(Action<int> listener){
            listenerList += listener;
        }

        public static void RemoveListener(Action<int> listener){
            listenerList -= listener;
        }

        public static void Fire(int newCurrentNumber){
            listenerList?.Invoke(newCurrentNumber);
        }
    }
}
