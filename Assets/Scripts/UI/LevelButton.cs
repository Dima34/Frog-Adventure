using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Image image;

    [HideInInspector]
    public string LevelName = "";
    [HideInInspector]
    public bool IsLevelOpened;

    public void LoadLevel(){
        if(IsLevelOpened){
            UIManager.Level = LevelName;
            UIManager.LevelFromMenu = true;
            LevelUtils.LoadLevel(3);
        }
    }

    public void ApplyText(){
        buttonText.text = LevelUtils.GetLevelInfoByName(LevelName)[1].ToString();

        if(!IsLevelOpened){
            Color disbledColor = new Color(.45f,.45f,.45f);
            image.color = disbledColor;
            buttonText.color = disbledColor;
        }
    }
}
