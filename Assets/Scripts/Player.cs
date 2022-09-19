using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [HideInInspector]
    public bool EnemyCollided = false;
    public Action OnCorrectCellUnderFeet;
    [SerializeField] float _hideAnimTime = 2f;


    Collider2D collidedObject;
    GameManager gameManager;

    public Collider2D CollidedObject { get => collidedObject; }

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collidedObj)
    {
        collidedObject = collidedObj;
    }

    private void OnTriggerExit2D(Collider2D collidedObj)
    {
        collidedObject = null;
        checkEnemy(collidedObj);
    }

    void cellUnderFeet()
    {
        Cell cellScript = collidedObject.transform.GetComponent<Cell>();
        int cellNumber = cellScript.Number;

        if (cellNumber == gameManager.CurrentNumber)
        {
            gameManager.NextSectionNumber();
            OnCorrectCellUnderFeet?.Invoke();
        }
        else
        {
           gameManager.RestartLevel();
        }
    }

    public void CheckUnderFeet()
    {
        if (collidedObject == null)
        {
            gameManager.RestartLevel();
            return;
        }

        switch (collidedObject.tag)
        {
            case "Cell":
                cellUnderFeet();
                break;
            case "Finsh":
                break;
            case "Start":
                break;
        }
    }

    public void checkEnemy(Collider2D collidedObj){
        if(collidedObj.tag == "Enemy"){
            if(!EnemyCollided){
                EnemyCollided = true;
                Destroy(collidedObj.gameObject);

                gameManager.RestartLevel();
            }
        }
    }

    public IEnumerator Hide(){
        float t = _hideAnimTime;
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();

        do{
            t -= Time.deltaTime;
            float fadeValue = t / _hideAnimTime;
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, fadeValue);

            yield return null;
        }while(t >= 0);
    }

    public void Show(){
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
    }
}
