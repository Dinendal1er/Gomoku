using UnityEngine;
using System.Collections;

public class MenuUIManager : MonoBehaviour {

    private UIManager UM;
    private SoundManager sm;

	// Use this for initialization
	void Start () {
        UM = UIManager.getInstance();
        if (UM == null)
            Debug.LogError("Missing the UIManager"); //throw exception.
        sm = SoundManager.getInstance();
        if (sm == null)
            Debug.LogError("Missing SoundManager");
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
