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

    public static List<LevelData> GetLevels(){
        List<LevelData> allLevels = getLevels();
        List<LevelData> sortedLevels = new List<LevelData>();

        // filter in right order
        HashSet<int> levelsBySection = new HashSet<int>();

        for (int i = 0; i < allLevels.Count; i++)
        {
            levelsBySection.Add(LevelUtils.GetLevelInfoByName(allLevels[i].name)[0]);
        }
        
        foreach (var sectionNum in levelsBySection)
        {
            List<LevelData> currentSectionLevels = GetSectionLevels(allLevels, sectionNum);

            while(true){
                bool isChanged = false;

                for (int i = 0; i < currentSectionLevels.Count - 1; i++)
                {
                    int levelNumber = LevelUtils.GetLevelInfoByName(currentSectionLevels[i].name)[1];
                    int nextLevelNumber = LevelUtils.GetLevelInfoByName(currentSectionLevels[i+1].name)[1];

                    if(levelNumber > nextLevelNumber){
                        LevelData temp = currentSectionLevels[i];
                        currentSectionLevels[i] = currentSectionLevels[i + 1];
                        currentSectionLevels[i + 1] = temp;
                        isChanged = true;
                    }
                }

                if(isChanged){
                    isChanged = false;
                } else
                {
                    break;
                }
            }

            foreach (var level in currentSectionLevels)
            {
                sortedLevels.Add(level);
            }
        }

        return sortedLevels;
    }

    public static LevelData GetNextLevel(List<LevelData> levelsInSection, int currentLevelNumber){
        foreach (var level in levelsInSection)
        {
            int levelNumber = LevelUtils.GetLevelInfoByName(level.name)[1];

            if(levelNumber > currentLevelNumber){
                return level;
            }
        }

        return null;
    }

    static List<LevelData> getLevels(){
        LevelData[] levelData = Resources.LoadAll<LevelData>("Levels");
        List<LevelData> levelDataList = new List<LevelData>();

        for (int i = 0; i < levelData.Length; i++)
        {
            levelDataList.Add(levelData[i]);
        }

        return levelDataList;
    }

    // Collect all levels with same section
    public static List<LevelData> GetSectionLevels(List<LevelData> levels,int sectionNumber){
        List<LevelData> currentSectionLevels = new List<LevelData>();

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
