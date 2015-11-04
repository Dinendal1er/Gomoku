using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionUIManager : MonoBehaviour {

    public Slider MusicLevel;
    public Slider EffectLevel;

    private UIManager UM;
    private SoundManager sm;

	// Use this for initialization
	void Start () {
        MusicLevel = GameObject.Find("Music Slider").GetComponent<Slider>();
        if (MusicLevel == null)
            Debug.LogError("Missing Music Slider"); //throw exception
        EffectLevel = GameObject.Find("Effect Slider").GetComponent<Slider>();
        if (EffectLevel == null)
            Debug.LogError("Missing Effect Slider"); //throw exception
        MusicLevel.value = OptionManager.getInstance().getMusicLevel();
        EffectLevel.value = OptionManager.getInstance().getEffectLevel();
        UM = UIManager.getInstance();
        if (UM == null)
            Debug.LogError("Missing the UIManager");
        sm = SoundManager.getInstance();
        if (sm == null)
            Debug.LogError("Missing SoundManager");
    }

    /// <summary>
    /// Called when the Music's slider value change.
    /// Save the player preferences and change the volume of the music.
    /// </summary>
    public void onMusicSlide()
    {
        OptionManager.getInstance().setMusicLevel(MusicLevel.value);
        sm.UpdateMusicLevel(MusicLevel.value);
    }

    /// <summary>
    /// Called when the Effect's slider value change.
    /// Save the player preferences and change the volume of the effects.
    /// </summary>
    public void onEffectSlide()
    {
        OptionManager.getInstance().setEffectLevel(EffectLevel.value);
        sm.UpdateEffectLevel(EffectLevel.value);
    }

    /// <summary>
    /// Set off Option UI, set on Menu UI.
    /// </summary>
    public void ToMenu()
    {
        sm.playButtonEffect();
        UM.OptionToggle();
        UM.MenuToggle();
    }


}
