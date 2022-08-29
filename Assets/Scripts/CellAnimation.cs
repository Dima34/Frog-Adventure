using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
public class CellAnimation : MonoBehaviour
{
    [SerializeField] float _animTime = 2f;
    [SerializeField] float _hideCellAnimTime = 2f;

    
    private void Start() {
        StartCoroutine(PlayAnim());
    }

    public IEnumerator PlayAnim(){
        float t = 0;
        Vector3 startScale = transform.localScale;

        while(t < _animTime){
            t += Time.deltaTime;
            transform.localScale = startScale * Utils.EaseOutBounce(t / _animTime);
            
            yield return null;
        }
    }

    public IEnumerator HideCell(){
        float t = _hideCellAnimTime;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        TMP_Text cellText = GetComponentInChildren<TMP_Text>();

        do{
            t -= Time.deltaTime;
            float fadeValue = t / _hideCellAnimTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, fadeValue);
            cellText.color = new Color(cellText.color.r, cellText.color.g, cellText.color.b, fadeValue);

            if(t >= 0){
                Destroy(this);
            }

            yield return null;
        }while(t >= 0);
    }
}
