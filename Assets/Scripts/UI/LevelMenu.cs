using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Open a " + UIManager.SectionNumber + " section levels page.");    
    }

    public void LoadSectionMenu(){
        SceneManager.LoadScene(1);
    }
}
