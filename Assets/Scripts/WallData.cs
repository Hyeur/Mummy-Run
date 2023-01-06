using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallData
{
    public float[,] vertical;
    public float[,] horizontal;
    
    public WallData(float[,] _vertical,float[,] _horizontal)
    {
        this.vertical = _vertical;
        this.horizontal = _horizontal;
    }
}
