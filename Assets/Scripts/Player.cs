using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool EnemyCollided = false;
    public Action OnCorrectCellUnderFeet;
    
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

    private void restartLevel()
    {
        StartCoroutine(gameManager.restartLevelSequence());
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
            restartLevel();
        }
    }

    public void CheckUnderFeet()
    {
        if (collidedObject == null)
        {
            restartLevel();
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

                StartCoroutine(gameManager.restartLevelSequence());
            }
        }
    }
}
