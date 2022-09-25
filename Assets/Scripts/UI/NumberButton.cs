using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NumberButton : MonoBehaviour
{
    [SerializeField] TMP_Text buttonText;
    public int SectionNumber;

    public void OnClick(){
        UIManager.SectionNumber = SectionNumber;
        SceneManager.LoadScene(2);
    }

    public void ApplyText(){
        buttonText.text = SectionNumber + "`s";
    }
}
