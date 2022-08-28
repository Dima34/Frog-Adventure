using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class Cell : MonoBehaviour
{
    [SerializeField] TMP_Text _numberText;
    [SerializeField] float _animTime = 2f;
    public Action<Vector2> OnMoveEvent;

    
    [HideInInspector]
    public int Number;

    private void Awake() {
        Debug.Log("Awake");
        SetNumber();
        PlayAnim();
    }

    public void SetNumber(){
        _numberText.text = Number.ToString();
    }

    public void PlayAnim(){
        StartCoroutine("playAnim");
    }

    IEnumerator playAnim(){
        Debug.Log("Coroutine started, animTime - " + _animTime);
        float t = 0;
        Vector3 startScale = transform.localScale;

        while(t < _animTime){
            Debug.Log("statement - " + (t <= _animTime).ToString());
            t += Time.deltaTime;
            Debug.Log("t - " + t);

            transform.localScale = startScale * Utils.EaseOutBounce(t);
            yield return null;
        }
    }
}
