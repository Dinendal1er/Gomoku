using UnityEngine;
using System;
using System.Collections;

public class MenuUIManager : MonoBehaviour {

    private UIManager UM;
    private SoundManager sm;

	// Use this for initialization
	void Start () {

        try
        {
            UM = UIManager.getInstance();
            if (UM == null)
                throw new NullReferenceException("Missing the UIManager");
            sm = SoundManager.getInstance();
            if (sm == null)
                throw new NullReferenceException("Missing the SoundManager");
        }
        catch(Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Sets off the Menu's UI and on the GameOption's UI.
    /// </summary>
    public void ToGameOption()
    {
        sm.playButtonEffect();
        UM.MenuToggle();
        UM.GameOptionToggle();
    }

    /// <summary>
    /// Sets off the Menu's UI and on the Option's UI.
    /// </summary>
    public void ToOption()
    {
        sm.playButtonEffect();
        UM.MenuToggle();
        UM.OptionToggle();
    }
}
