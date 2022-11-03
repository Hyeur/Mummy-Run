using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallData
{
    public Transform spawnPoint {get;set;}

    public WallData(Transform t){
        spawnPoint = t;
    }
}
