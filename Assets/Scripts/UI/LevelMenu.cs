using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] LevelButton _levelButton;
    [SerializeField] GridLayoutGroup _buttonContainer;


    void Start()
    {
        createLevelButtons();    
    }
    
    void createLevelButtons()
    {
        LevelData[] allLevels = Resources.LoadAll<LevelData>("Levels");
        List<LevelData> currentSectionLevels = new List<LevelData>();

        // get all levels of current section
        foreach (var level in allLevels)
        {
            if(LevelUtils.GetLevelInfoByName(level.name)[0] == UIManager.SectionNumber){
                currentSectionLevels.Add(level);
            }
        }
        
        foreach (var level in currentSectionLevels)
        {
            LevelButton levelButton = UnityEngine.Object.Instantiate(_levelButton, new Vector3(), new Quaternion(), _buttonContainer.transform);
            levelButton.LevelName = level.name;
            levelButton.ApplyText();
        }
    }

    public void LoadSectionMenu(){
        LevelUtils.LoadLevel(1);
    }
}
