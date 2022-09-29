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
    List<Enemy> existingEmenyList = new List<Enemy>();

    public List<Enemy> ExistingEmenyList { get => existingEmenyList; set => existingEmenyList = value; }

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

    Enemy createEnemy(Transform levelCenterPoint, float ySpawnPoint){
        calcEnemySpawnBounds(levelCenterPoint);

        float spawnLineNumber = Random.Range(1, cellAmount + 1);
        float xSpawnPoint = marginIncludedLeftPoint.x + (spawnPointsGap * spawnLineNumber);

        Enemy enemy = Object.Instantiate(
            enemyPrefab, 
            new Vector3(xSpawnPoint, ySpawnPoint + enemyPrefab.transform.localScale.y, 10),
            Quaternion.AngleAxis(-90, new Vector3(0,0,1))
        );
        if(enemySpeed == 0)
            Debug.LogError("Warning! Enemy speed = 0");
        enemy.MovementSpeed = enemySpeed;
        return enemy;
    }

    public IEnumerator SpawnEnemy(
        Transform levelCenterPoint,
        float yFromPoint,
        Vector3 moveToPoint
    ){
        Enemy enemy = createEnemy(levelCenterPoint,yFromPoint);
        existingEmenyList.Add(enemy);

        yield return enemy.StartCoroutine(enemy.MoveTo(moveToPoint));
        
        existingEmenyList.Remove(enemy);
        Object.Destroy(enemy.gameObject);
    }
}
