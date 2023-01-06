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
    MummyManager mummyManager;

    public Transform playerSprite;

    bool isMoving = false;

    float inputTimer = 0;
    float moveTimer = 0;

    public float moveTimeInterval = .5f;
    public Transform myTransform;

    float speed;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        speed = moveSpeed;
        playerSprite.parent = null;

    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            inputTimer += Time.deltaTime;
        }
        else
        {
            inputTimer = 0;
        }


        moveTimer += Time.deltaTime;

        playerSprite.position = Vector3.MoveTowards(playerSprite.position, transform.position + new Vector3(0, 0.15f, -0.2f), speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, playerSprite.position) <= 0.16f)
        {
            GridManager.inst.UpdateMoveAbleGridForPlayer();
            isMoving = false;
            // Debug.Log(playerManager.LocationInGrid());
        }
        else
        {
            isMoving = true;
        }
    }

    public void HandleMovement()
    {
        if (isMoving)
            return;

        Vector2 targetPos = Vector2.zero;
        if (movement == Vector2.right)
        {
            targetPos = GridManager.inst.playerMoveAbleGrid[0];
        }
        else if (movement == Vector2.down)
        {
            targetPos = GridManager.inst.playerMoveAbleGrid[1];
        }
        else if (movement == Vector2.left)
        {
            targetPos = GridManager.inst.playerMoveAbleGrid[2];
        }
        else if (movement == Vector2.up)
        {
            targetPos = GridManager.inst.playerMoveAbleGrid[3];
        }
        if (targetPos != Vector2.zero && moveTimer >= moveTimeInterval)
        {
            isMoving = true;
            transform.position = targetPos;
            PlayerManager.inst.isStarted = true;
            moveTimer = 0;
            StartCoroutine(mummyManager.addStep());
            mummyManager.toggleMove = true;
        }
        // if (targetPos == Vector2.zero && moveTimer >= moveTimeInterval){
        //     isMoving = true;
        //     mummyManager.stepInx += 2;
        //     moveTimer = 0;
        // }

    }
}
