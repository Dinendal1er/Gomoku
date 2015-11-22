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
        try
        {
            if (Application.CanStreamedLevelBeLoaded(index))
                Application.LoadLevel(index);
            else
            {
                throw new IndexOutOfRangeException("Level index can't be loaded");
            }
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void LoadLevel(string levelName)
    {
        try
        {
            if (Application.CanStreamedLevelBeLoaded(levelName))
                Application.LoadLevel(levelName);
            else
            {
                throw new IndexOutOfRangeException(levelName + " can't be loaded");
            }
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void LoadLevelAdditive(int index)
    {
        try {
        if (Application.CanStreamedLevelBeLoaded(index))
            Application.LoadLevelAdditive(index);
        else
        {
            throw new IndexOutOfRangeException("Level index can't be loaded");
        }
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void LoadLevelAdditive(string levelName)
    {
        try {
            if (Application.CanStreamedLevelBeLoaded(levelName))
                Application.LoadLevelAdditive(levelName);
            else
            {
                throw new IndexOutOfRangeException("Level name can't be loaded");
            }
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
