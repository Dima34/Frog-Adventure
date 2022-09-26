using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public static class LevelUtils
{
    // zero array elem is a section
    // first array elem is a level
    public static List<int> GetLevelInfoByName(string name){
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

    public static void LoadLevel(string name){
        SceneManager.LoadScene(name);
    }

    public static LevelData GetLevelByName(string name){
        return Resources.Load<LevelData>("Levels/" + name);
    }

    public static LevelData[] GetLevels(){
        return Resources.LoadAll<LevelData>("Levels");
    }

    public static LevelData GetNextLevel(List<LevelData> levelsInSectionList, int currentLevelNumber){
        foreach (var level in levelsInSectionList)
        {
            int levelNumber = LevelUtils.GetLevelInfoByName(level.name)[1];

            if(levelNumber > currentLevelNumber){
                return level;
            }
        }

        return null;
    }

    // Collect all levels with same section
    public static List<LevelData> GetSectionLevels(int sectionNumber){
        List<LevelData> currentSectionLevels = new List<LevelData>();

        LevelData[] levels = GetLevels();
        foreach (var level in levels)
        {
            int levelSection = LevelUtils.GetLevelInfoByName(level.name)[0];
            if(levelSection == sectionNumber){
                currentSectionLevels.Add(level);
            }
        }

        return currentSectionLevels;
    }

    public static bool IsNextLevelExist(List<LevelData> sectionLevels, int currentLevelNumber){
        int lastLevelNubmer = 0;
        // Search how much levels is in section
        foreach (var level in sectionLevels)
        {
            lastLevelNubmer = LevelUtils.GetLevelInfoByName(level.name)[1];
        }

        if(lastLevelNubmer > currentLevelNumber){
            return true;
        } else{
            return false;
        }
    }
}
