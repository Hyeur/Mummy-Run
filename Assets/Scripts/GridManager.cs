using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager inst;

    Grid grid;

    Vector2 playGroundPivotPos = new Vector2(-3,3);
    int playGroundSize = 6;

    [SerializeField]
    PlayerManager playerManager;
    [SerializeField]
    MummyManager mummyManager;

    // public LayerMask Walls;

    public Vector2[] playerMoveAbleGrid = new Vector2[4];
    public Vector2[] mummyMoveAbleGrid = new Vector2[4];

    float leftWall;
    float rightWall;
    float topWall;
    float bottomWall;


    void Start(){
        GridManager.inst = this;
        grid = GetComponent<Grid>();

        transform.position = Vector3.zero;
        grid.cellSize = Vector3.one;
        
        rightWall = playGroundPivotPos.x + playGroundSize;
        bottomWall = playGroundPivotPos.y - playGroundSize;
        leftWall = playGroundPivotPos.x;
        topWall = playGroundPivotPos.y;
        
        
    }

    public void UpdateMoveAbleGridForPlayer()
    {
        playerMoveAbleGrid[0] = playerManager.currentPosGrid + Vector2.right;
        playerMoveAbleGrid[1] = playerManager.currentPosGrid + Vector2.down;
        playerMoveAbleGrid[2] = playerManager.currentPosGrid + Vector2.left;
        playerMoveAbleGrid[3] = playerManager.currentPosGrid + Vector2.up;

        if (playerMoveAbleGrid[0].x > rightWall)
        {
            playerMoveAbleGrid[0] = Vector2.zero;
        }
        if (playerMoveAbleGrid[1].y < bottomWall)
        {
            playerMoveAbleGrid[1] = Vector2.zero;
        }
        if (playerMoveAbleGrid[2].x < leftWall)
        {
            playerMoveAbleGrid[2] = Vector2.zero;
        }
        if (playerMoveAbleGrid[3].y > topWall)
        {
            playerMoveAbleGrid[3] = Vector2.zero;
        }

        for (int i = 0; i < playerMoveAbleGrid.Length; i++)
        {
            Vector2 direction = playerMoveAbleGrid[i] - playerManager.currentPosGrid;
            RaycastHit2D hit= Physics2D.Raycast(playerManager.currentPosGrid,direction.normalized,direction.magnitude);

            if (hit.collider != null)
            {
                Debug.DrawRay(playerManager.currentPosGrid,direction,Color.red);
                playerMoveAbleGrid[i] = Vector2.zero;
            }
            else Debug.DrawRay(playerManager.currentPosGrid,direction,Color.green);
        }
    }

    public void UpdateMoveAbleGridForMummy()
    {
        mummyMoveAbleGrid[0] = mummyManager.currentPosGrid + Vector2.right;
        mummyMoveAbleGrid[1] = mummyManager.currentPosGrid + Vector2.down;
        mummyMoveAbleGrid[2] = mummyManager.currentPosGrid + Vector2.left;
        mummyMoveAbleGrid[3] = mummyManager.currentPosGrid + Vector2.up;

        if (playerMoveAbleGrid[0].x > rightWall)
        {
            playerMoveAbleGrid[0] = Vector2.zero;
        }
        if (playerMoveAbleGrid[1].y < bottomWall)
        {
            playerMoveAbleGrid[1] = Vector2.zero;
        }
        if (playerMoveAbleGrid[2].x < leftWall)
        {
            playerMoveAbleGrid[2] = Vector2.zero;
        }
        if (playerMoveAbleGrid[3].y > topWall)
        {
            playerMoveAbleGrid[3] = Vector2.zero;
        }


        for (int i = 0; i < mummyMoveAbleGrid.Length; i++)
        {
            Vector2 direction = mummyMoveAbleGrid[i] - mummyManager.currentPosGrid;
            RaycastHit2D hit= Physics2D.Raycast(mummyManager.currentPosGrid,direction.normalized,direction.magnitude);

            if (hit.collider != null)
            {
                Debug.DrawRay(mummyManager.currentPosGrid,direction,Color.red);
                mummyMoveAbleGrid[i] = Vector2.zero;
            }
            else Debug.DrawRay(mummyManager.currentPosGrid,direction,Color.green);
        }

    }
}
