using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner
{
    Enemy enemyPrefab;
    Cell cellPrefab;
    Section sectionPrefab;
    float sideMarginSize;
    int cellAmount;
    float includedSectionWidth;
    float spawnPointsGap;
    Vector3 localLeftVector;
    Vector3 marginIncludedLeftPoint;
    GameManager gameManager;
    float enemySpeed;
    Camera camera;
    
    public EnemySpawner(GameManager gameManager){
        this.gameManager = gameManager;
        enemyPrefab = gameManager.EnemyPrefab;
        cellPrefab = gameManager.CellPrefab;
        cellAmount = gameManager.CellInSectionAmount;
        sectionPrefab = gameManager.SectionPrefab;
        sideMarginSize = gameManager.SectionSideMarginSize;
        camera = Camera.main;
        enemySpeed = gameManager.EnemyMovementSpeed;
    }
    
    void calcEnemySpawnBounds(Transform spawnPoint){
        includedSectionWidth = Utils.CalcMarginIncludedSize(cellPrefab, sectionPrefab.transform.localScale.x, sideMarginSize);
        // Calctulate gaps between enemies
        spawnPointsGap = includedSectionWidth / (cellAmount == 1 ? cellAmount : (cellAmount - 1));
        // Vector from center to left
        localLeftVector = gameManager.transform.localRotation * Vector3.left;
        // Get margin included left side coords
        marginIncludedLeftPoint = spawnPoint.position + (localLeftVector * (includedSectionWidth / 2));
    }

    Enemy spawnEnemy(Transform levelCenterPoint){
        calcEnemySpawnBounds(levelCenterPoint);

        float spawnLineNumber = Random.Range(1, cellAmount + 1);
        float xSpawnPoint = marginIncludedLeftPoint.x + (spawnPointsGap * spawnLineNumber);
        float ySpawnPoint = camera.ViewportToWorldPoint(new Vector3(1,1,0)).y;

        return Object.Instantiate(enemyPrefab, new Vector3(xSpawnPoint, ySpawnPoint + enemyPrefab.transform.localScale.y, 10),gameManager.transform.rotation);
    }

    public void EnemyLifeCycleSequence(
        Transform levelCenterPoint,
        Vector3 moveToPoint
    ){
        Enemy enemy = spawnEnemy(levelCenterPoint);
        enemy.MovementSpeed = enemySpeed;

        Task movingProcess = new Task(enemy.MoveTo(moveToPoint));
        movingProcess.Finished += delegate(bool manual){
            Object.Destroy(enemy.gameObject);
        };
    }
}
