using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;
    public Vector2 currentPosGrid;
    Vector2 movement;
    Vector2 startPos = new Vector3(0,-3);

    Vector2 truePos = new Vector2(.5f,.5f);

    

    void Start(){
        playerMovement = GetComponent<PlayerMovement>();


        playerMovement.movePoint.position = startPos + truePos;
        currentPosGrid = playerMovement.movePoint.position;
        GridManager.inst.UpdateMoveAbleGrid();

    }

    void FixedUpdate(){

       currentPosGrid = playerMovement.movePoint.position;

       if (currentPosGrid != null) {
            
            playerMovement.HandleMovement();
       }
    }

    public Vector2 PlayerLocationInGrid()
    {
        return new Vector2(currentPosGrid.x + 2.5f, currentPosGrid.y - 2.5f);
    }
}
