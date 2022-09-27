using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false;
    public GameObject pauseMenuUI;

    private void Start() {
        Resume();
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }
    
    public void Pause(){
        IsGamePaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadCountMenu(){
        LevelUtils.LoadLevel("NumberMenu");
    }
}
