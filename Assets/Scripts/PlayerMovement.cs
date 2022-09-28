using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementTime = 1.25f;
    [Space(5)]
    [Header("Move animation  settings")]
    [SerializeField] Animator _moveAnimator;
    [SerializeField] float _delayedUnplugTime = 0.5f;

    Camera mainCamera;
    CameraMovement cameraMovement;
    Vector3 fromPos;
    Vector3 toPos;
    Player player;

    bool isMoving;
    bool isFollowing;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        player = GetComponent<Player>();
    }

    private void OnEnable() {
        GlobalEventManager.OnCorrectCell.AddListener(addCellFollow);
    }

    private void OnDisable() {
        GlobalEventManager.OnCorrectCell.RemoveListener(addCellFollow);
    }

    private void OnTriggerExit2D(Collider2D collidedObj)
    {
        removeCellFollow(collidedObj);
    }

    void Update()
    {
        HandleMove();
    }

    void HandleMove()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && !cameraMovement.IsCameraMooving)
        {
            
            if(EventSystem.current.IsPointerOverGameObject()){
                if(EventSystem.current.currentSelectedGameObject != null)
                    return;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit))
            {
                Debug.Log("hit");
            }

            Vector3 pointToMove = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            pointToMove.z = 0;
            
            MovePlayer(transform.position, pointToMove);
        }
    }

    public void MovePlayer(Vector3 from, Vector3 to){
        StartCoroutine(MovePlayerSequence(from, to));
    }

    public IEnumerator MovePlayerSequence(Vector3 from, Vector3 to)
    {
        fromPos = from;
        toPos = to;
        isMoving = true;
        float t = 0;

        StartCoroutine("handleAnimationTimeChange");

        while (t < _movementTime)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(fromPos, toPos, Utils.EaseInOut(t / _movementTime));
            yield return null;
        }

        isMoving = false;
        player.CheckUnderFeet();
    }

    IEnumerator handleAnimationTimeChange()
    {
        _moveAnimator.SetBool("IsJumping", true);

        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime;
            _moveAnimator.SetFloat("JumpTime", Utils.EaseInCirc(t));
            yield return null;
        }

        _moveAnimator.SetBool("IsJumping", false);
    }

    void addCellFollow(Collider2D cellObject){
        Cell cellScript = cellObject.transform.GetComponent<Cell>();
        cellScript.OnMoveEvent += followCell;
        isFollowing = true;
    }
    
    void followCell(Vector2 positionAddition)
    {
        transform.position += (Vector3)positionAddition;
        fromPos += (Vector3)positionAddition;
    }

    void removeCellFollow(Collider2D collidedObject)
    {
        if (collidedObject.tag == "Cell" && isFollowing)
        {
            isFollowing = false;
            Cell cellScript = collidedObject.GetComponent<Cell>();

            if (cellScript.OnMoveEvent != null)
            {
                cellScript.OnMoveEvent -= followCell;
            }
        }
    }
}

