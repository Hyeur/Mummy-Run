using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst = null;
    public enum LevelSelector { Test, lv1, lv2, lv3, lv4, lv5, lv6, };

    [SerializeField]
    public LevelSelector level;

    void Awake(){
        GameManager.inst = this;
    }
    void Start()
    {
        // level = LevelSelector.Test;
    }

    void Update()
    {
        if (Vector2.Distance(PlayerManager.inst.currentPosGrid,MummyManager.inst.mummySprite.position) <= 0.1) {
            reloadScene();
        }  
    }
    public void reloadScene(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
