using UnityEngine;
using System.Collections;
using System;

public class LevelManager : MonoBehaviour {

    static private LevelManager instance = null;


    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// Used to return the current instance of the LevelManager singleton.
    /// </summary>
    /// <returns>return a LevelManager instance</returns>
    static public LevelManager getInstance()
    {
        if (instance != null)
            return instance;
        return null;
    }

    public void LoadLevel(int index)
    {
        if (Application.CanStreamedLevelBeLoaded(index))
            Application.LoadLevel(index);
        else
        {
            Debug.LogError("Level index can't be loaded");
            //throw Exception("Level index can't  be loaded");
        }
    }

    public void LoadLevel(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
            Application.LoadLevel(levelName);
        else
        {
            Debug.LogError("Level name can't be loaded");
            //throw Exception("Level name can't  be loaded");
        }
    }

    public void LoadLevelAdditive(int index)
    {
        if (Application.CanStreamedLevelBeLoaded(index))
            Application.LoadLevelAdditive(index);
        else
        {
            Debug.LogError("Level index can't be loaded");
            //throw Exception("Level index can't  be loaded");
        }
    }

    public void LoadLevelAdditive(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
            Application.LoadLevelAdditive(levelName);
        else
        {
            Debug.LogError("Level name can't be loaded");
            //throw Exception("Level name can't  be loaded");
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
