using UnityEngine;
using System;
using System.Collections;

public class GameOptionUIManager : MonoBehaviour {

    private UIManager UM;
    private SoundManager sm;

    // Use this for initialization
    void Start()
    {
        try
        {
            UM = UIManager.getInstance();
            if (UM == null)
                throw new NullReferenceException("Missing the UIManager");
            sm = SoundManager.getInstance();
            if (sm == null)
                throw new NullReferenceException("Missing the SoundManager");
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Sets off the GameOption's UI and on the Menu's UI.
    /// </summary>
    public void ToMenu()
    {
        sm.playButtonEffect();
        UM.GameOptionToggle();
        UM.MenuToggle();
    }

    
    public void ToGame()
    {
        sm.playButtonEffect();
        UM.GameOptionToggle();
        UM.GameToggle();
        LevelManager.getInstance().LoadLevel("Game_1v1");
    }

    //TODO fonction pour aller en jeu.
}
