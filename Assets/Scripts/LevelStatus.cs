using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LevelStatus
{
    static string fileName = "Data.json";

    public static void writeAsDefault(){
        List<LevelItem> defaultLevelItemList = new List<LevelItem>();

        foreach (var level in LevelUtils.GetLevels())
        {
            bool isOpened = false;
            int levelLevel = LevelUtils.GetLevelInfoByName(level.name)[1];

            if(levelLevel == 1)
                isOpened = true;

            LevelItem defaultLevelItem = new LevelItem(level.name, isOpened);
            defaultLevelItemList.Add(defaultLevelItem);
        }

        SaveData(new LevelItemsData(defaultLevelItemList));
    }

    public static void SaveData(LevelItemsData data){
        string savingPath = getLevelDataPath();
        string dataToSaveJson = JsonUtility.ToJson(data);

        using StreamWriter writer = new StreamWriter(savingPath);
        writer.Write(dataToSaveJson);
        writer.Close();
    }

    public static LevelItemsData GetData(){
        if(!isLevelDataExist()){
            createDefaultData();
        }

        LevelItemsData savedLevelData = getSavedLevelData();
        if(savedLevelData.Data.Count != LevelUtils.GetLevels().Count){
            createDefaultData();
            savedLevelData = getSavedLevelData();
        }
        
        return savedLevelData;
    }

    public static void createDefaultData(){
        createDataFile();
        writeAsDefault();
    }

    public static LevelItemsData getSavedLevelData(){
        string savingPath = getLevelDataPath();
        using StreamReader reader = new StreamReader(savingPath);

        string loadedDataJson = reader.ReadLine();
        LevelItemsData loadedData = JsonUtility.FromJson<LevelItemsData>(loadedDataJson);

        reader.Close();
        return loadedData;
    }

    static void createDataFile(){
        File.Create(getLevelDataPath()).Close();
    }
    
    static bool isLevelDataExist(){
        return File.Exists(getLevelDataPath());
    }

    static string getLevelDataPath(){
        return Application.persistentDataPath + Path.AltDirectorySeparatorChar + fileName;
    }

    public static void OpenLevel(string levelName){
        LevelItemsData levelsData = GetData();
       
        foreach (var level in levelsData.Data)
        {
            if(level.Name == levelName){
                level.IsOpened = true;
            }
        }
        
        SaveData(levelsData);
    }
}

[Serializable]
public class LevelItemsData{
    public List<LevelItem> Data;

    public LevelItemsData(List<LevelItem> data){
        Data = data;
    }

    public LevelItemsData(LevelItemsData data){
        Data = data.Data;
    }
}

[Serializable]
public class LevelItem{
    public string Name;
    public bool IsOpened;

    public LevelItem(string name, bool isOpened){
        Name = name;
        IsOpened = isOpened;
    }
}
