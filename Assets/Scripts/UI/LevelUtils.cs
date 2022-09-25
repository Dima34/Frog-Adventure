using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public static class LevelUtils
{
    // zero array elem is a section
    // first array elem is a level
    public static List<int> GetLevelNameInfo(string name){
        string[] splittedName = name.Split("_");
        List<int> numArr = new List<int>();
        int sectionNum;

        for (int i = 1; i < splittedName.Length; i++)
        {
            int num;
            Int32.TryParse(splittedName[i], out num);
            numArr.Add(num);
        }

        return numArr;
    }

    public static void LoadLevel(int index){
        SceneManager.LoadScene(index);
    }

    public static LevelData GetLevelDataByName(string name){
        return Resources.Load<LevelData>("Levels/" + name);
    }
}
