using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1;
    Vector2 movement;

    [SerializeField]
    PlayerManager playerManager;

    public Transform movePoint;
    
    bool isMoving = false;

    float inputTimer = 0;
    float moveTimer = 0;

    public float moveTimeInterval = .5f;
    public Transform myTransform;
    
    float speed;

    void Start(){
        playerManager = GetComponent<PlayerManager>();
        speed = moveSpeed;
        movePoint.parent = null;
    }

    void Update(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        if (movement != Vector2.zero)
        {
            inputTimer+= Time.deltaTime;
        }
        else
        {
            inputTimer = 0;
        }


        moveTimer+= Time.deltaTime;

        
        transform.position = Vector3.MoveTowards(transform.position,movePoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movePoint.position) == 0)
        {
            myTransform = transform;
            GridManager.inst.UpdateMoveAbleGrid();
            isMoving = false;
            Debug.Log(playerManager.PlayerLocationInGrid());

        }
        else
        {
            isMoving = true;
        }
    }
    
    public void HandleMovement(){
        if (isMoving)
            return;

        Vector2 targetPos = Vector2.zero;
        if (movement == Vector2.right) {
            targetPos = GridManager.inst.moveAbleGrid[0];
        } else if (movement == Vector2.down) {
            targetPos = GridManager.inst.moveAbleGrid[1];
        } else if (movement == Vector2.left) {
            targetPos = GridManager.inst.moveAbleGrid[2];
        }  else if (movement == Vector2.up) {
            targetPos = GridManager.inst.moveAbleGrid[3];
        }
        if (targetPos != Vector2.zero && moveTimer >= moveTimeInterval){
            isMoving = true;
            movePoint.position = targetPos;
            moveTimer = 0;
        }
    }
}
