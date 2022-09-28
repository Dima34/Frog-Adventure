using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class LevelButton : MonoBehaviour
{
    [SerializeField] TMP_Text buttonText;
    public string LevelName = "";
    public bool IsLevelOpened;

    public void LoadLevel(){
        if(IsLevelOpened){
            UIManager.Level = LevelName;
            LevelUtils.LoadLevel(3);
        }
    }

    public void ApplyText(){
        if(IsLevelOpened){
            buttonText.text = LevelUtils.GetLevelInfoByName(LevelName)[1].ToString();
        } else{
            buttonText.text = "L";
        }
    }
}
