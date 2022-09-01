using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : Editor {
        SerializedProperty startNumber;
    SerializedProperty increment;
    SerializedProperty iterationCount;
    SerializedProperty playerPrefab;
    SerializedProperty startPrefab;
    SerializedProperty finishPrefab;
    SerializedProperty enemies;
    SerializedProperty sectionPrefab;
    SerializedProperty cellPrefab;
    SerializedProperty propGaps;
    SerializedProperty cellInSectionAmount;
    SerializedProperty sectionSideMarginSize;
    SerializedProperty willCellsMove;
    SerializedProperty cellMoveSpeed;
    SerializedProperty enemyPrefab;
    SerializedProperty sectionsWithEnemies;

    GUIStyle headingStyle;

    private void OnEnable() {
        startNumber = serializedObject.FindProperty("StartNumber");
        increment = serializedObject.FindProperty("Increment");
        iterationCount = serializedObject.FindProperty("IterationCount");
        playerPrefab = serializedObject.FindProperty("PlayerPrefab");
        startPrefab = serializedObject.FindProperty("StartPrefab");
        finishPrefab = serializedObject.FindProperty("FinishPrefab");
        enemies = serializedObject.FindProperty("Enemies");
        sectionPrefab = serializedObject.FindProperty("SectionPrefab");
        cellPrefab = serializedObject.FindProperty("CellPrefab");
        propGaps = serializedObject.FindProperty("PropGaps");
        cellInSectionAmount = serializedObject.FindProperty("CellInSectionAmount");
        sectionSideMarginSize = serializedObject.FindProperty("SectionSideMarginSize");
        willCellsMove = serializedObject.FindProperty("WillCellsMove");
        cellMoveSpeed = serializedObject.FindProperty("CellMoveSpeed");
        enemyPrefab = serializedObject.FindProperty("EnemyPrefab");
        sectionsWithEnemies = serializedObject.FindProperty("SectionsWithEnemies");
    
        
    }

    public override void OnInspectorGUI() {
        headingStyle = new GUIStyle(GUI.skin.label){
            fontSize = 16,
            fixedHeight = 22,
            alignment = TextAnchor.UpperCenter
        };

        serializedObject.Update();

        EditorGUILayout.LabelField("Global Settings", headingStyle);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(startNumber);
        EditorGUILayout.PropertyField(increment);
        EditorGUILayout.PropertyField(iterationCount);
        EditorGUILayout.ObjectField(playerPrefab, typeof(Transform));
        EditorGUILayout.ObjectField(startPrefab, typeof(Transform));
        EditorGUILayout.ObjectField(finishPrefab, typeof(Transform));
        EditorGUILayout.PropertyField(enemies);

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Section", headingStyle);
        EditorGUILayout.Space(10);
        EditorGUILayout.ObjectField(sectionPrefab, typeof(Section));
        EditorGUILayout.ObjectField(cellPrefab, typeof(Cell));
        EditorGUILayout.PropertyField(propGaps);
        EditorGUILayout.PropertyField(cellInSectionAmount);
        EditorGUILayout.PropertyField(sectionSideMarginSize);
        EditorGUILayout.PropertyField(willCellsMove);
        if(willCellsMove.boolValue){
            EditorGUILayout.PropertyField(cellMoveSpeed);
        }

        if(enemies.boolValue){
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Enemy", headingStyle);
            EditorGUILayout.Space(10);
            EditorGUILayout.ObjectField(enemyPrefab, typeof(Enemy));
            EditorGUILayout.LabelField("Enemies on section");

            if(sectionsWithEnemies.arraySize != iterationCount.intValue){
                int sizeDiff = iterationCount.intValue - sectionsWithEnemies.arraySize;
                sectionsWithEnemies.arraySize += sizeDiff;
                Debug.Log("array size" + sectionsWithEnemies.arraySize );
            }

            EditorGUI.indentLevel = 1;
            for (int i = sectionsWithEnemies.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty element = sectionsWithEnemies.GetArrayElementAtIndex(i);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField((i + 1).ToString());
                EditorGUILayout.PropertyField(element, GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel = 0;

        }        

        serializedObject.ApplyModifiedProperties();
    }
}