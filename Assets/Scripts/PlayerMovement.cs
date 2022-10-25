using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1;
    Vector2 movement;

    [SerializeField]
    PlayerManager playerManager;

    [SerializeField]
    GridManager gridManager;
    Rigidbody2D rgd_player;
    
    bool isMoving = false;

    float inputTimer = 0;
    float moveTimer = 0;

    float moveTimeInterval = .8f;
    

    float moveAmount = 1f;
    float speed;

    void Start(){
        rgd_player = GetComponent<Rigidbody2D>();
        playerManager = GetComponent<PlayerManager>();
        speed = moveSpeed;

        
        
    }

    void Update(){
        moveTimer+= Time.deltaTime;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        Debug.Log(movement);
        if (movement == Vector2.zero && moveTimer > moveTimeInterval)
        {
            isMoving = false;
        }
        else
        {
            inputTimer+= Time.deltaTime;
            if (inputTimer > moveTimeInterval)
            {
                isMoving = false;
                inputTimer = 0;
            }
        }
    }
    
    public void HandleMovement(){
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

        if (targetPos != Vector2.zero && !isMoving){
            isMoving = true;
    
            rgd_player.velocity = movement * speed * Time.fixedDeltaTime;
            if (Vector2.Distance(playerManager.currentPosGrid,targetPos) <= 0.01f)
            {
                rgd_player.velocity = Vector2.zero;
                rgd_player.position = targetPos;
            }
            moveTimer = 0;
        }
    }
}
