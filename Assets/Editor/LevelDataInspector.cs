using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LevelData))]
public class LevelDataInspector : Editor {
    SerializedProperty startNumber;
    SerializedProperty increment;
    SerializedProperty iterationCount;
    SerializedProperty playerPrefab;
    SerializedProperty startPrefab;
    SerializedProperty finishPrefab;
    SerializedProperty backgroundPrefab;
    SerializedProperty enemies;
    SerializedProperty sectionPrefab;
    SerializedProperty cellPrefab;
    SerializedProperty propGaps;
    SerializedProperty cellInSectionAmount;
    SerializedProperty sectionSideMarginSize;
    SerializedProperty willCellsMove;
    SerializedProperty cellMoveSpeed;
    SerializedProperty enemyPrefab;
    SerializedProperty enemyMovementSpeed;
    SerializedProperty enemyTimepointList;
    ReorderableList timepointsReorderable;

    GUIStyle headingStyle;

    private void OnEnable() {
        startNumber = serializedObject.FindProperty("StartNumber");
        increment = serializedObject.FindProperty("Increment");
        iterationCount = serializedObject.FindProperty("IterationCount");
        playerPrefab = serializedObject.FindProperty("PlayerPrefab");
        startPrefab = serializedObject.FindProperty("StartPrefab");
        finishPrefab = serializedObject.FindProperty("FinishPrefab");
        backgroundPrefab = serializedObject.FindProperty("BackgroundPrefab");
        enemies = serializedObject.FindProperty("Enemies");
        sectionPrefab = serializedObject.FindProperty("SectionPrefab");
        cellPrefab = serializedObject.FindProperty("CellPrefab");
        propGaps = serializedObject.FindProperty("PropGaps");
        cellInSectionAmount = serializedObject.FindProperty("CellInSectionAmount");
        sectionSideMarginSize = serializedObject.FindProperty("SectionSideMarginSize");
        willCellsMove = serializedObject.FindProperty("WillCellsMove");
        cellMoveSpeed = serializedObject.FindProperty("CellMoveSpeed");
        enemyPrefab = serializedObject.FindProperty("EnemyPrefab");
        enemyMovementSpeed = serializedObject.FindProperty("EnemyMovementSpeed");
        enemyTimepointList = serializedObject.FindProperty("EnemyTimepoints");

        timepointsReorderable = new ReorderableList(serializedObject, enemyTimepointList, false, false, true, true);
        timepointsReorderable.drawElementCallback = drawTimepointsElement;
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
        EditorGUILayout.ObjectField(backgroundPrefab, typeof(Transform));
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
            EditorGUILayout.PropertyField(enemyMovementSpeed);
            EditorGUILayout.LabelField("Enemies on section");


            timepointsReorderable.DoLayoutList();
        }        

        serializedObject.ApplyModifiedProperties();
    }

    public void drawTimepointsElement(Rect rect, int index, bool isActive, bool isFocused){
        SerializedProperty element = enemyTimepointList.GetArrayElementAtIndex(index);

        Rect leftRect = new Rect(rect.x, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight);
        Rect rightRect = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, EditorGUIUtility.singleLineHeight);

        EditorGUI.indentLevel = 1;
        EditorGUI.LabelField(leftRect, "On time");
        EditorGUI.PropertyField(rightRect, element.FindPropertyRelative("time"), GUIContent.none);
        EditorGUI.indentLevel = 0;
    }

}