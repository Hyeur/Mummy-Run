using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager inst;
    public Tilemap tilemap;
    Grid grid;
    private Vector2 playGroundPivotPos = new Vector2(-3, 3);
    private int playGroundSize = 6;

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

    Vector3Int exitWay;

    public Tile green;
    public Tile yellow;
    public Tile tile1;
    public Tile tile2;
    public Tile exit;
    public Tile exit_left;
    public Tile exit_right;
    public Tile exit_down;

    bool canUpdate = true;
    bool canMark = true;
    void Start()
    {
        GridManager.inst = this;
        grid = GetComponent<Grid>();

        transform.position = Vector3.zero;
        grid.cellSize = Vector3.one;

        rightWall = playGroundPivotPos.x + playGroundSize;
        bottomWall = playGroundPivotPos.y - playGroundSize;
        leftWall = playGroundPivotPos.x;
        topWall = playGroundPivotPos.y;

        // createExit(new Vector3(-4, 2));
    }

    public void createExit(Vector3 pos)
    {
        if (pos != null)
        {
            exitWay = tilemap.WorldToCell(pos + new Vector3(.5f,.5f,0));
            if (pos.x == playGroundPivotPos.x - 1)
            {

                tilemap.SetTile(exitWay, exit_left);
            }
            else if (pos.x == Math.Abs(playGroundPivotPos.x))
            {
                tilemap.SetTile(exitWay, exit_right);
            }
            else if (pos.y == -4)
            {
                tilemap.SetTile(exitWay, exit_down);
            }
            else if (pos.y == 3)
            {
                tilemap.SetTile(exitWay, exit);
            }
        }
        else Debug.Log("exit gate position is null!!");
    }

    private int SortByDistane(Vector3Int a, Vector3Int b)
    {
        if (a == null)
        {
            if (b == null)
            {
                return 0;
            }
            else return -1;
        }
        else
        {
            if (b == null)
            {
                return 1;
            }
            else
            {
                if (Vector3Int.Distance(a, exitWay) == Vector3Int.Distance(b, exitWay))
                {
                    return 0;
                }
                else {
                    if (Vector3Int.Distance(a,exitWay) > Vector3Int.Distance(b, exitWay)){
                        return 1;
                    }
                    else return -1;
                }
            }
        }
    }
    public void markBy()
    {
        if (!canUpdate) return;

        List<Vector3Int> order = new List<Vector3Int>();
        for (int i = 0; i < playerMoveAbleGrid.Length; i++)
        {
            if (tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[i].x, playerMoveAbleGrid[i].y, 0)) == exitWay) continue;

            if (playerMoveAbleGrid[i] == Vector2.zero)
            {
                Vector2 direction = playerMoveAbleGrid[i] - playerManager.currentPosGrid;
                RaycastHit2D hit = Physics2D.Raycast(playerManager.currentPosGrid, direction.normalized, direction.magnitude);

                if (hit.collider == null && Vector2.Distance(playerManager.currentPosGrid,playerMoveAbleGrid[i]) <= 1)
                {
                    order.Add(tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[i].x, playerMoveAbleGrid[i].y, 0)));
                }
            }
            else order.Add(tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[i].x, playerMoveAbleGrid[i].y, 0)));
        }

        order.Sort(SortByDistane);
        
        for (int i = 0; i < order.Count; i++)
        {
            if (i == 0){
                tilemap.SetTile(order[i],green);
            }
            else tilemap.SetTile(order[i],yellow);
        }
    }

    public void unMark()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;
            for (int i = 0; i < playerMoveAbleGrid.Length; i++)
            {
                if (tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[i].x, playerMoveAbleGrid[i].y, 0)) != pos &&
                    tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[i].x, playerMoveAbleGrid[i].y, 0)) != exitWay &&
                    pos.x >= -3 && pos.x <= 2 && pos.y >= -3 && pos.y <= 2)
                {
                    if (Math.Abs(pos.x) % 2 == 0)
                    {
                        tilemap.SetTile(pos, tile1);
                    }
                    else
                    {
                        tilemap.SetTile(pos, tile2);
                    }
                }
            }
        }
    }

    void Update()
    {
        // tilemap.SetColor(tilemap.WorldToCell(Input.mousePosition), Color.green);
        // tilemap.SetTileFlags(tilemap.WorldToCell(PlayerManager.inst.currentPosGrid),TileFlags.None);
        // Debug.Log(tilemap.WorldToCell(PlayerManager.inst.currentPosGrid));
        // tilemap.SetColor(tilemap.WorldToCell(PlayerManager.inst.currentPosGrid),Color.black);

        if (tilemap.WorldToCell(playerManager.currentPosGrid) == exitWay)
        {
            canUpdate = false;
            GameManager.inst.reloadScene();
        }

        unMark();
        markBy();
    }

    public void UpdateMoveAbleGridForPlayer()
    {
        if (!canUpdate)
        {
            Array.Clear(playerMoveAbleGrid, 0, playerMoveAbleGrid.Length);
            return;
        }

        playerMoveAbleGrid[0] = playerManager.currentPosGrid + Vector2.right;
        playerMoveAbleGrid[1] = playerManager.currentPosGrid + Vector2.down;
        playerMoveAbleGrid[2] = playerManager.currentPosGrid + Vector2.left;
        playerMoveAbleGrid[3] = playerManager.currentPosGrid + Vector2.up;

        if (playerMoveAbleGrid[0].x > rightWall && tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[0].x, playerMoveAbleGrid[0].y, 0)) != exitWay)
        {
            playerMoveAbleGrid[0] = Vector2.zero;
        }
        if (playerMoveAbleGrid[1].y < bottomWall && tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[1].x, playerMoveAbleGrid[1].y, 0)) != exitWay)
        {
            playerMoveAbleGrid[1] = Vector2.zero;
        }
        if (playerMoveAbleGrid[2].x < leftWall && tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[2].x, playerMoveAbleGrid[2].y, 0)) != exitWay)
        {
            playerMoveAbleGrid[2] = Vector2.zero;
        }
        if (playerMoveAbleGrid[3].y > topWall && tilemap.WorldToCell(new Vector3(playerMoveAbleGrid[3].x, playerMoveAbleGrid[3].y, 0)) != exitWay)
        {
            playerMoveAbleGrid[3] = Vector2.zero;
        }

        for (int i = 0; i < playerMoveAbleGrid.Length; i++)
        {
            Vector2 direction = playerMoveAbleGrid[i] - playerManager.currentPosGrid;
            RaycastHit2D hit = Physics2D.Raycast(playerManager.currentPosGrid, direction.normalized, direction.magnitude);

            if (hit.collider != null)
            {
                Debug.DrawRay(playerManager.currentPosGrid, direction, Color.red);
                playerMoveAbleGrid[i] = Vector2.zero;
            }
            else Debug.DrawRay(playerManager.currentPosGrid, direction, Color.green);

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

        // HighLightTilesByDijkstra();

        for (int i = 0; i < mummyMoveAbleGrid.Length; i++)
        {
            Vector2 direction = mummyMoveAbleGrid[i] - mummyManager.currentPosGrid;
            RaycastHit2D hit = Physics2D.Raycast(mummyManager.currentPosGrid, direction.normalized, direction.magnitude);

            if (hit.collider != null)
            {
                Debug.DrawRay(mummyManager.currentPosGrid, direction, Color.red);
                mummyMoveAbleGrid[i] = Vector2.zero;
            }
            else Debug.DrawRay(mummyManager.currentPosGrid, direction, Color.green);
        }

    }

    private void HighLightTilesByDijkstra()
    {
        throw new NotImplementedException();
    }

    // void printSolution(int[] dist, int n)
    // {
    //     Console.Write("Vertex     Distance "
    //                   + "from Source\n");
    //     for (int i = 0; i < V; i++)
    //         Console.Write(i + " \t\t " + dist[i] + "\n");
    // }

    // Function that implements Dijkstra's
    // single source shortest path algorithm
    // for a graph represented using adjacency
    // matrix representation
    // int minDistance(int[] dist,
    //                 bool[] sptSet)
    // {
    //     // Initialize min value
    //     int min = int.MaxValue, min_index = -1;

    //     for (int v = 0; v < V; v++)
    //         if (sptSet[v] == false && dist[v] <= min) {
    //             min = dist[v];
    //             min_index = v;
    //         }

    //     return min_index;
    // }
    // int V;
    // void dijkstra(int[, ] graph, int src)
    // {
    //     int[] dist = new int[V];
    //     bool[] sptSet = new bool[V];


    //     for (int i = 0; i < V; i++) {
    //         dist[i] = int.MaxValue;
    //         sptSet[i] = false;
    //     }

    //     dist[src] = 0;


    //     for (int count = 0; count < V - 1; count++) {

    //         int u = minDistance(dist, sptSet);


    //         sptSet[u] = true;

    //         for (int v = 0; v < V; v++)

    //             if (!sptSet[v] && graph[u, v] != 0 && 
    //                  dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
    //                 dist[v] = dist[u] + graph[u, v];
    //     }

    //     printSolution(dist, V);
    // }
    void FloydWarshall(int[,] graph, int verticesCount)
    {
        int[,] distance = new int[verticesCount, verticesCount];

        for (int i = 0; i < verticesCount; ++i)
            for (int j = 0; j < verticesCount; ++j)
                distance[i, j] = graph[i, j];

        for (int k = 0; k < verticesCount; ++k)
        {
            for (int i = 0; i < verticesCount; ++i)
            {
                for (int j = 0; j < verticesCount; ++j)
                {
                    if (distance[i, k] + distance[k, j] < distance[i, j])
                        distance[i, j] = distance[i, k] + distance[k, j];
                }
            }
        }
    }
}
