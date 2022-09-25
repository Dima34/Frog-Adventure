using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class LevelButton : MonoBehaviour
{
    [SerializeField] TMP_Text buttonText;
    public string LevelName = "";

    public void LoadLevel(){
        UIManager.Level = LevelName;
        LevelUtils.LoadLevel(3);
    }

    public void ApplyText(){
        buttonText.text = LevelUtils.GetLevelNameInfo(LevelName)[1].ToString();
    }
}
