using UnityEngine;
using System.Collections;

public class GameOptionUIManager : MonoBehaviour {

    private UIManager UM;
    private SoundManager sm;

    // Use this for initialization
    void Start()
    {
        UM = UIManager.getInstance();
        if (UM == null)
            Debug.LogError("Missing the UIManager"); //throw exception.
        sm = SoundManager.getInstance();
        if (sm == null)
            Debug.LogError("Missing SoundManager instance"); //throw error
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

    //TODO fonction pour aller en jeu.
}
