using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    public GameObject SectionsContainer;
    public Transform StartObject;
    public Transform FinishObject;
    public Transform BackgroundObject;
    public Transform HintArrowObject;
    public List<Section> SpawnedSections { get => spawnedSections; }
    public Transform SpawnedPlayer { get => spawnedPlayer; }
    
    string sectionContainerName = "Sections container";
    Transform parentObject;
    int startNumber;
    int increment;
    int iterationCount;
    Transform playerPrefab;
    Transform startPrefab;
    Transform finishPrefab;
    Transform backgroundPrefab;
    Section sectionPrefab;
    Transform hintArrowPrefab;
    List<Section> spawnedSections;
    float sectionSideMarginSize;
    int cellInSectionAmount;
    bool willCellsMove;
    float cellMoveSpeed;
    Cell cellPrefab;
    float propGaps;
    Vector3 levelDirection;
    Transform spawnedPlayer;
    EnemySpawner enemySpawner;
    GameManager gameManager;


    public LevelManager(
        GameManager gameManager,
        Transform parentObject
    )
    {
        this.parentObject = parentObject;
        this.gameManager = gameManager;
        startNumber = gameManager.StartNumber;
        increment = gameManager.Increment;
        iterationCount = gameManager.IterationCount;
        playerPrefab = gameManager.PlayerPrefab;
        startPrefab = gameManager.StartPrefab;
        finishPrefab = gameManager.FinishPrefab;
        backgroundPrefab = gameManager.BackgroundPrefab;
        sectionPrefab = gameManager.SectionPrefab;
        hintArrowPrefab = gameManager.HintArrowPrefab;
        sectionSideMarginSize = gameManager.SectionSideMarginSize;
        cellInSectionAmount = gameManager.CellInSectionAmount;
        willCellsMove = gameManager.WillCellsMove;
        cellMoveSpeed = gameManager.CellMoveSpeed;
        cellPrefab = gameManager.CellPrefab;
        propGaps = gameManager.PropGaps;
        levelDirection = parentObject.localRotation * Vector3.up;
        enemySpawner = new EnemySpawner(gameManager);

        GlobalEventManager.OnCorrectCell.AddListener(leaveCorrectCell);
    }

    public void BuildLevel(bool fromEditor = false)
    {
        DestroyLevel();
        
        CreateBackground();
        CreatePlayer();
        CreateStartPlate();
        CreateSections(fromEditor);
        SpawnCells();
        CreateFinishPlate();
    }
    
    public void DestroyLevel(){
        DestroySections();
        DestroyPlayer();
        DestroyBackground();
        DestroyStartPlate();
        DestroyFinishPlate();
        DestroyEnemies();
    }

    public void CreatePlayer()
    {
        // Teleport player on start plate
        spawnedPlayer = Object.Instantiate(playerPrefab, new Vector3(0, 0, -10), parentObject.rotation);
        spawnedPlayer.SetParent(parentObject);

        // Disable movement
        spawnedPlayer.GetComponent<PlayerMovement>().enabled = false;

        GlobalEventManager.OnPlayerCreated.Fire(spawnedPlayer);
    }

    public void DestroyPlayer(){
        if(spawnedPlayer){
            Object.Destroy(spawnedPlayer.gameObject);
            spawnedPlayer = null;
        }
    }

    public void CreateStartPlate()
    {
        StartObject = Object.Instantiate(startPrefab, parentObject.position, parentObject.localRotation);
        StartObject.SetParent(parentObject);
    }

    public void DestroyStartPlate(){
        if(StartObject){
            Object.Destroy(StartObject.gameObject);
            StartObject = null;
        }
    }

    public void CreateFinishPlate()
    {
        FinishObject = Object.Instantiate(finishPrefab, parentObject.position + (levelDirection * propGaps * (iterationCount + 1)), parentObject.rotation);
        FinishObject.SetParent(parentObject, true);
    }

    public void DestroyFinishPlate(){
        if(FinishObject){
            Object.Destroy(FinishObject.gameObject);
            FinishObject = null;
        }
    }

    public void CreateBackground(){
        Vector3 levelLength = parentObject.position + (levelDirection * propGaps * (iterationCount + 1));
        BackgroundObject = Object.Instantiate(backgroundPrefab, levelLength / 2, parentObject.rotation);
        
        SpriteRenderer spriteRenderer = BackgroundObject.GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, levelLength.y / BackgroundObject.transform.localScale.y);

        BackgroundObject.SetParent(parentObject, true);
    }

    public void DestroyBackground(){
        if(BackgroundObject){
            Object.Destroy(BackgroundObject.gameObject);
            BackgroundObject = null;
        }
    }

    public void CreateSections(bool fromEditor = false)
    {
        SectionsContainer = new GameObject(sectionContainerName);
        SectionsContainer.transform.SetParent(parentObject);
        spawnedSections = new List<Section>();

        for (int i = 1; i < iterationCount + 1; i++)
        {
            Vector3 newSectionPosition = StartObject.transform.position + (levelDirection * propGaps * i);

            // Spawn and place section in hierarchy 
            GameObject section = Object.Instantiate(sectionPrefab.gameObject, newSectionPosition, StartObject.rotation);
            section.transform.SetParent(SectionsContainer.transform, true);

            Section sectionScript = section.GetComponent<Section>();
            // Setup section
            sectionScript.SetUp(
                startNumber,
                increment,
                i,
                sectionSideMarginSize,
                cellInSectionAmount,
                willCellsMove,
                cellMoveSpeed
            );
            // Add section to spawned sections list
            spawnedSections.Add(sectionScript);


            // If call function not from editor - disable section
            if (!fromEditor)
                section.gameObject.SetActive(false);
        }
    }

    public void DestroySections(){
        if(spawnedSections != null && spawnedSections.Count > 0)
        {
            foreach (var section in spawnedSections)
            {
                Object.Destroy(section.gameObject);
            }

            spawnedSections = null;
        }
    }

    public void SpawnPlayer()
    {
        if (spawnedPlayer == null){
            Debug.LogError("You cannot spawn player which you don`t create. Create level first.");
            return;
        }
        if (StartObject == null){
            Debug.LogError("Start object isn`t created. Create level first");
            return;
        }

        spawnedPlayer.position = startPrefab.position + (-startPrefab.transform.right * (startPrefab.lossyScale.x * 0.75f));

        Player player = spawnedPlayer.GetComponent<Player>();
        player.EnemyCollided = false;

        PlayerMovement playerMovement = spawnedPlayer.GetComponent<PlayerMovement>();
        playerMovement.MovePlayer(spawnedPlayer.position, StartObject.position);
        playerMovement.enabled = true;
    }

    public IEnumerator HidePlayer(){
        spawnedPlayer.GetComponent<PlayerMovement>().enabled = false;

        Task hidingProcess = new Task(spawnedPlayer.GetComponent<Player>().Hide());
        yield return new WaitWhile(()=>hidingProcess.Running);

        spawnedPlayer.transform.position = new Vector3(0,0,-10);
    }

    public void ShowPlayer(){
        spawnedPlayer.GetComponent<Player>().Show();
    }
    
    public void SpawnCells(){
        if(spawnedSections == null){
            Debug.LogError("Can`t spawn cells. Create sections first");
            return;
        }

        foreach (var section in spawnedSections)
        {
            section.SpawnCells(cellPrefab);
        }
    }

    public IEnumerator HideCells(){
        if(spawnedSections == null){
            Debug.LogError("Can`t destroy cells. Create sections first");
            yield break;
        }

        Task hideCellsProcess = null;

        foreach (var section in spawnedSections)
        {
            hideCellsProcess = new Task(section.HideCells());
        }

        yield return new WaitWhile(()=>hideCellsProcess.Running);
    }

    public void DestroyCells(){
        foreach (var section in spawnedSections)
        {
            section.DestroyCells();
        }
    }

    void leaveCorrectCell(Collider2D collided){
        LeaveCorrectCell();
    }
    
    public void LeaveCorrectCell(){
        int currentStep = gameManager.CurrentStep;

        if (currentStep != 0){
            gameManager.StartCoroutine(SpawnedSections[currentStep - 1].LeaveCorrectCell());
        }
    }

    public void SpawnEnemy(){
        Vector3 moveToPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0,0));

        float enemyHeight = gameManager.EnemyPrefab.transform.localScale.y;
        float fromPointY = FinishObject.position.y + enemyHeight;
        Vector3 toPoint = StartObject.position - (StartObject.up * enemyHeight);

        gameManager.StartCoroutine(enemySpawner.SpawnEnemy(gameManager.transform,fromPointY,toPoint));
    }

    public void DestroyEnemies(){
        foreach (var enemy in enemySpawner.ExistingEmenyList)
        {
            if(enemy) Object.Destroy(enemy.gameObject);
        }

        enemySpawner.ExistingEmenyList = new List<Enemy>();
    }

    public void SpawnHintArrow(){
        for (int i = spawnedSections.Count - 1; i >= 0 ; i--)
        {
            if(spawnedSections[i].isActiveAndEnabled){
                Cell correctCell = spawnedSections[i].CellsList[spawnedSections[i].CorrectCellIndex];

                // 1.1 is a 10% specing
                Vector3 arrowPosition = correctCell.transform.position + (correctCell.transform.up * 1.1f * correctCell.transform.lossyScale.y);
                HintArrowObject = Object.Instantiate(hintArrowPrefab, arrowPosition, correctCell.transform.rotation);

                break;
            }
        }
    }

    public void HideHintArrow(){
        if(HintArrowObject){
            Object.Destroy(HintArrowObject.gameObject);
            HintArrowObject = null;
        }
    }

    public void ClearEventListeners(){
        GlobalEventManager.OnCorrectCell.RemoveListener(leaveCorrectCell);
    }
}
