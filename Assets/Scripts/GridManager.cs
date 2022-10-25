using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager inst;

    Grid grid;

    Vector2 playGroundPivotPos = new Vector2(-3,3);
    int playGroundSize = 6;
    BoxCollider2D boxCollider2D;

    [SerializeField]
    PlayerManager playerManager;

    public Vector2[] moveAbleGrid = new Vector2[4];

    float leftWall;
    float rightWall;
    float topWall;
    float bottomWall;


    void Start(){
        GridManager.inst = this;
        grid = GetComponent<Grid>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        transform.position = Vector3.zero;
        grid.cellSize = Vector3.one;
        boxCollider2D.size = Vector2.one * playGroundSize;
        
        rightWall = playGroundPivotPos.x + playGroundSize;
        bottomWall = playGroundPivotPos.y - playGroundSize;
        leftWall = playGroundPivotPos.x;
        topWall = playGroundPivotPos.y;
    }

    void Update(){
        moveAbleGrid[0] = playerManager.currentPosGrid + Vector2.right;
        moveAbleGrid[1] = playerManager.currentPosGrid + Vector2.down;
        moveAbleGrid[2] = playerManager.currentPosGrid + Vector2.left;
        moveAbleGrid[3] = playerManager.currentPosGrid + Vector2.up;



        if (moveAbleGrid[0].x > rightWall)
        {
            moveAbleGrid[0] = Vector2.zero;
        }
        if (moveAbleGrid[1].y < bottomWall)
        {
            moveAbleGrid[1] = Vector2.zero;
        }
        if (moveAbleGrid[2].x < leftWall)
        {
            moveAbleGrid[2] = Vector2.zero;
        }
        if (moveAbleGrid[3].y > topWall)
        {
            moveAbleGrid[3] = Vector2.zero;
        }
    }
}
