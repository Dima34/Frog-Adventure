using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {
    GameManager targetScript;

    void OnEnable() {
        targetScript = (GameManager)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Level")){
            targetScript.CreateGameSequence(true);
        }
    }
}