using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyManager : MonoBehaviour
{

    public static MummyManager inst = null;

    [SerializeField]
    PlayerManager playerManager;
    public Vector2 currentPosGrid;
    public float moveSpeed;
    
    public Transform lookAtTarget;

    Vector2 movement;
    [SerializeField]
    public Vector2 startPos;

    Vector2 truePos = new Vector2(-2.5f,-2.5f);

    public int step = 2;

    public bool toggleMove = false;
    bool isMoving = false;

    float speed;

    // [HideInInspector]
    public int stepInx;

    public bool isStarted = false;

    public Transform mummySprite;

    void Start()
    {
        MummyManager.inst = this;

        mummySprite.parent = null;
        speed = moveSpeed;
        
        
        stepInx = 0;
    }
    void Update()
    {

        if (!isStarted){
            transform.position = startPos + truePos;
            currentPosGrid = transform.position;
        }

        LookAt();
        if (stepInx <= 0)
        {
            toggleMove = false;
        }
        else toggleMove = true;

        currentPosGrid = transform.position;

        mummySprite.position = Vector3.MoveTowards(mummySprite.position,transform.position + new Vector3(0,0,-.2f),speed * Time.deltaTime);

        if (Vector2.Distance(mummySprite.position, transform.position) <= 0.2f)
        {
            isMoving = false;
        }


        if (toggleMove && stepInx > 0)
        {
            HandleMovement();
        }
    }

    void updateNextStepDirection()
    {
        movement = Vector2.zero;
        GridManager.inst.UpdateMoveAbleGridForMummy();
        if (currentPosGrid.y != playerManager.currentPosGrid.y)
        {
            if (currentPosGrid.y < playerManager.currentPosGrid.y && GridManager.inst.mummyMoveAbleGrid[3] != Vector2.zero)
            {
                movement = Vector2.up;
            }
            else if (currentPosGrid.y > playerManager.currentPosGrid.y && GridManager.inst.mummyMoveAbleGrid[1] != Vector2.zero)
            {
                movement = Vector2.down;
            } 
        }
        if (currentPosGrid.x != playerManager.currentPosGrid.x)
        {
            if (currentPosGrid.x < playerManager.currentPosGrid.x && GridManager.inst.mummyMoveAbleGrid[0] != Vector2.zero)
            {
                movement = Vector2.right;
            }
            else if (currentPosGrid.x > playerManager.currentPosGrid.x && GridManager.inst.mummyMoveAbleGrid[2] != Vector2.zero)
            {
                movement = Vector2.left;
            } 
        }

        
    }
    public void HandleMovement()
    {
        if (isMoving)
            return;

        updateNextStepDirection();
        Vector2 targetPos = Vector2.zero;
        if (movement == Vector2.right) {
            targetPos = GridManager.inst.mummyMoveAbleGrid[0];
        } else if (movement == Vector2.down) {
            targetPos = GridManager.inst.mummyMoveAbleGrid[1];
        } else if (movement == Vector2.left) {
            targetPos = GridManager.inst.mummyMoveAbleGrid[2];
        }  else if (movement == Vector2.up) {
            targetPos = GridManager.inst.mummyMoveAbleGrid[3];
        }
        if (targetPos != Vector2.zero && stepInx > 0){
            isMoving = true;
            transform.position = targetPos;   
            stepInx--;
            this.isStarted = true;
        }
        if (targetPos == Vector2.zero && stepInx > 0){
            isMoving = true;
            stepInx--;
            this.isStarted = true;
        }
    }
    public Vector2 LocationInGrid()
    {
        return currentPosGrid + Vector2.one * 2.5f;
    }

    public IEnumerator addStep(){
        yield return new WaitForSeconds(.2f);
        stepInx = step;
    }
    void LookAt(){
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - lookAtTarget.position );

        mummySprite.rotation = Quaternion.Lerp(transform.rotation, lookRotation,0.1f);
    }
}
