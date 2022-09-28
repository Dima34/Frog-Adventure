using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] LevelButton _levelButton;
    [SerializeField] GridLayoutGroup _buttonContainer;


    void Start()
    {
        LevelStatus.GetData();
        createLevelButtons();    
    }
    
    void createLevelButtons()
    {
        LevelItemsData levelStatus = LevelStatus.GetData();
        List<LevelItem> currentSectionLevels = new List<LevelItem>();

        // get all levels of current section
        foreach (var level in levelStatus.Data)
        {
            if(LevelUtils.GetLevelInfoByName(level.Name)[0] == UIManager.SectionNumber){
                currentSectionLevels.Add(level);
            }
        }
        
        // Create buttons
        foreach (var level in currentSectionLevels)
        {
            LevelButton levelButton = UnityEngine.Object.Instantiate(_levelButton, new Vector3(), new Quaternion(), _buttonContainer.transform);
            levelButton.LevelName = level.Name;
            levelButton.IsLevelOpened = level.IsOpened;
            levelButton.ApplyText();
        }
    }

    public void LoadSectionMenu(){
        LevelUtils.LoadLevel(1);
    }
}
