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
    [SerializeField] AudioSource _jumpSound;

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
        GlobalEventManager.OnCorrectCell.AddListener(magnetToCenter);
    }

    private void OnDisable() {
        GlobalEventManager.OnCorrectCell.RemoveListener(addCellFollow);
        GlobalEventManager.OnCorrectCell.RemoveListener(magnetToCenter);
    }

    private void OnTriggerExit2D(Collider2D collidedObj)
    {
        removeCellFollow(collidedObj);
    }

    void magnetToCenter(Collider2D cell){
        StartCoroutine(magnetToCenterProcess(cell));
    }

    IEnumerator magnetToCenterProcess(Collider2D cell){
        float t = 0;
        while(t < 1){
            transform.position = Vector3.Lerp(transform.position, cell.transform.position, t);
            t+=Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        HandleMove();
    }

    void HandleMove()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving && !cameraMovement.IsCameraMooving)
        {
            
            if(EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null){
                return;
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

        _jumpSound.Play();

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

