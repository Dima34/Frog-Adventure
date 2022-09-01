using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    Enemy enemyPrefab;
    Cell cellPrefab;
    float sideMarginSize;
    int cellAmount;
    float includedSectionWidth;
    float spawnPointsGap;
    Vector3 localLeftVector;
    Vector3 marginIncludedLeftPoint;
    
    public EnemySpawner(GameManager gameManager){
        enemyPrefab = gameManager.EnemyPrefab;
        cellPrefab = gameManager.CellPrefab;
        cellAmount = gameManager.CellInSectionAmount;
    }
    
    void calcEnemySpawnBounds(Transform spawnPoint){
        includedSectionWidth = Utils.CalcMarginIncludedSize(cellPrefab, transform.localScale.x, sideMarginSize);
        // Calctulate gaps between cells
        spawnPointsGap = includedSectionWidth / (cellAmount == 1 ? cellAmount : (cellAmount - 1));
        // Get section horizontal vector from center to left
        localLeftVector = transform.localRotation * Vector3.left;
        // Get margin included left side coords
        marginIncludedLeftPoint = spawnPoint.position + (localLeftVector * (includedSectionWidth / 2));
    }

    public void SpawnEnemy(Transform spawnPointCenter){
        calcEnemySpawnBounds(spawnPointCenter);

        
    }
}
