using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONSavingLoading : MonoBehaviour
{
    private List<WallData> wallsData;

    private string path = "";
    private string persistentPath = "";

    private Transform newTransform;

    public List<WallData> LoadedWallsData;

    
    void Start()
    {
        SetPaths();
        CreateWallsData();
        CreateWallsData();
        CreateWallsData();
        CreateWallsData();
    }

    private void CreateWallsData()
    {
        //load
        GameObject newWall = new GameObject();
        newWall.transform.position = new Vector3(1,2,3);
        newWall.transform.rotation = new Quaternion(3,2,1,2);
        
        //bind
        newTransform = newWall.transform;
        wallsData.Add(new WallData(newTransform));
    }

    private void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "WallMapData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "WallMapData.json";
    }
    
    private void SaveData()
    {
        string savePath = path;
        Debug.Log("Saving Data at:" + savePath);

        string  json = JsonUtility.ToJson(wallsData);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    private void LoadData()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        LoadedWallsData.Add(JsonUtility.FromJson<WallData>(json));
        Debug.Log(LoadedWallsData.ToString());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            LoadData();
        }
        
    }
}
