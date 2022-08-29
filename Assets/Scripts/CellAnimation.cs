using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAnimation : MonoBehaviour
{
    [SerializeField] float _animTime = 2f;
    
    private void Start() {
        PlayAnim();
    }

    public void PlayAnim(){
        StartCoroutine("playAnim");
    }

    IEnumerator playAnim(){
        float t = 0;
        Vector3 startScale = transform.localScale;

        while(t < _animTime){
            t += Time.deltaTime;
            Debug.Log("t - " + t);

            transform.localScale = startScale * Utils.EaseOutBounce(t / _animTime);
            Debug.Log("While...");
            yield return null;
        }

        Debug.Log("end coroutine");
    }
}
