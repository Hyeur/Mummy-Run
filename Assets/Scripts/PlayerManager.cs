using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerMovement playerMovement;

    Rigidbody2D playerRigid;
    public Vector2 currentPosGrid;
    Vector2 movement;
    Vector2 startPos = new Vector3(0,-3);

    Vector2 truePos = new Vector2(.5f,.5f);

    

    void Start(){
        playerMovement = GetComponent<PlayerMovement>();
        playerRigid = GetComponent<Rigidbody2D>();



        playerRigid.MovePosition(startPos + truePos);

    }

    void FixedUpdate(){

       currentPosGrid = playerRigid.position;

       if (currentPosGrid != null) {
            
            playerMovement.HandleMovement();     
       }
    }
}
