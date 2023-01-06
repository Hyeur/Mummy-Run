using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public class _startPoint
    {
        public float x{get;set;}
        public float y{get;set;}
    }


    public int id {get;set;}
    public int[,] exitWay {get;set;}
    public int[,] mummyStart {get;set;}
    public int[,] playerStart {get;set;}

    public WallData wallData2DArray {get;set;}

}
