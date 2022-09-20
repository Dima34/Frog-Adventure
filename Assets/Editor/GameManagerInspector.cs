using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {
    GameManager gameManager;

    void OnEnable() {
        gameManager = (GameManager)target;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Level")){
            gameManager.SetLevelData();
            gameManager.InitLevelManager();
            gameManager.CreateGameSequence();
        }
    }
}