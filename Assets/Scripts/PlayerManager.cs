using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager inst = null;
    PlayerMovement playerMovement;
    public Vector2 currentPosGrid;
    Vector2 movement;
    [SerializeField]
    public Vector2 startPos;

    Vector2 truePos = new Vector2(-2.5f,-2.5f);

    public Transform lookAtTarget;

    public bool isStarted = false;
    

    void Start(){

        PlayerManager.inst = this;

        playerMovement = GetComponent<PlayerMovement>();

        
        // GridManager.inst.UpdateMoveAbleGridForPlayer();

    }

    void Update(){

        if(!this.isStarted){
            playerMovement.transform.position = startPos + truePos;
            currentPosGrid = transform.position;
        }

       currentPosGrid = transform.position;

       if (currentPosGrid != null) {
            
            playerMovement.HandleMovement();
            LookAt();
       }
    }

    public Vector2 LocationInGrid()
    {
        return currentPosGrid + Vector2.one * 2.5f;
    }

    void LookAt(){
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - lookAtTarget.position );

        playerMovement.playerSprite.rotation = Quaternion.Lerp(transform.rotation, lookRotation,0.1f);
    }
    
}
