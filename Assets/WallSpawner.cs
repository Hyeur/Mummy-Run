using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject wallPrefab;

    public TextAsset jsonData;
    private int mapSize = 6;
    public float[,] verticalWalls;
    public float[,] horizontalWalls;

    private bool isSpawned = false;
    private bool canSpawn = false;

    private List<Wall> listVWalls = new List<Wall>();
    private List<Wall> listHWalls = new List<Wall>();

    void Start()
    {
        // if (GridManager.inst.playGroundSize == 0) return;
        // mapSize = GridManager.inst.playGroundSize;

        //load date from JSON to verticalWalls and horizontalWalls
        int selectedLevel = (int)GameManager.inst.level;

        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<LevelManager>(jsonData.text);

        if (data != null){
            Debug.Log(data);
            verticalWalls = data.levelArray[selectedLevel].wallData2DArray.vertical;
            horizontalWalls = data.levelArray[selectedLevel].wallData2DArray.horizontal;
            int[,] playerStartPos = data.levelArray[selectedLevel].playerStart;
            int[,] mummyStartPos = data.levelArray[selectedLevel].mummyStart;
            int[,] exitWay = data.levelArray[selectedLevel].exitWay;
            if (playerStartPos != null){
                Debug.Log(playerStartPos[0,0]);
                Debug.Log(playerStartPos[0,1]);
                Debug.Log(mummyStartPos[0,0]);
                Debug.Log(mummyStartPos[0,1]);
                Vector2 pos = new Vector2(playerStartPos[0,0],playerStartPos[0,1]);
                PlayerManager.inst.startPos = pos;
                Debug.Log(pos);
            }
            if (mummyStartPos != null){
                Vector2 pos = new Vector2(mummyStartPos[0,0],mummyStartPos[0,1]);
                MummyManager.inst.startPos = pos;
            }
            if (exitWay != null) {
                GridManager.inst.createExit(new Vector3(exitWay[0,0],exitWay[0,1],0));
            }
        }

        // verticalWalls = new float[6, 5] {{1,1,1,1,1},
        //                                 {1,1,1,1,1},
        //                                 {1,1,1,1,1},
        //                                 {1,1,1,1,1},
        //                                 {1,1,1,1,1},
        //                                 {1,1,1,1,1}
        //                                 };
        // horizontalWalls = new float[5, 6] {{1,0,0,0,0,0},
        //                                     {0,1,0,0,0,0},
        //                                     {0,0,1,0,0,0},
        //                                     {0,0,0,1,0,0},
        //                                     {0,0,0,0,1,0}};
        //then
        NormalizeDimension(verticalWalls, horizontalWalls);
        checkDataStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && !isSpawned)
        {
            foreach (Wall item in listVWalls)
            {
                Instantiate(wallPrefab, new Vector3(item._x, item._y, 0), Quaternion.identity);
            }
            foreach (Wall item in listHWalls)
            {
                Instantiate(wallPrefab, new Vector3(item._x, item._y, 0), transform.rotation * Quaternion.Euler(0f, 0, 90f));
            }
            isSpawned = true;
        }
    }

    public void checkDataStatus()
    {
        if (listVWalls != null && listHWalls != null)
        {
            canSpawn = true;
        }
    }
    public void NormalizeDimension(float[,] vertical, float[,] horizontal)
    {
        if (vertical.Length < (mapSize * (mapSize - 1)) || horizontal.Length < (mapSize * (mapSize - 1)))
        {
            return;
        }
        for (int x = 0; x < vertical.GetLength(0); x++)
        {
            for (int y = 0; y < vertical.GetLength(1); y++)
            {

                if (vertical[x, y] != 0)
                {
                    listVWalls.Add(new Wall(y - 2f,x - 2.5f));

                }

            }
        }
        for (int x = 0; x < horizontal.GetLength(0); x++)
        {
            for (int y = 0; y < horizontal.GetLength(1); y++)
            {

                if (horizontal[x, y] != 0)
                {
                    listHWalls.Add(new Wall(y - 2.5f,x - 2f));
                }

            }
        }
    }
}
