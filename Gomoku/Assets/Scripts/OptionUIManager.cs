using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class OptionUIManager : MonoBehaviour {

    public Slider MusicLevel;
    public Slider EffectLevel;

    private UIManager UM;
    private SoundManager sm;

	// Use this for initialization
	void Start () {
        try
        {
            MusicLevel = GameObject.Find("Music Slider").GetComponent<Slider>();
            if (MusicLevel == null)
               throw new NullReferenceException("Missing Music Slider");
            EffectLevel = GameObject.Find("Effect Slider").GetComponent<Slider>();
            if (EffectLevel == null)
                throw new NullReferenceException("Missing Effect Slider");
            MusicLevel.value = OptionManager.getInstance().getMusicLevel();
            EffectLevel.value = OptionManager.getInstance().getEffectLevel();
            UM = UIManager.getInstance();
            if (UM == null)
                throw new NullReferenceException("Missing the UIManager");
            sm = SoundManager.getInstance();
            if (sm == null)
                throw new NullReferenceException("Missing SoundManager");
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// Called when the Music's slider value change.
    /// Save the player preferences and change the volume of the music.
    /// </summary>
    public void onMusicSlide()
    {
        OptionManager.getInstance().setMusicLevel(MusicLevel.value);
        if (sm != null)
        sm.UpdateMusicLevel(MusicLevel.value);
    }

    /// <summary>
    /// Called when the Effect's slider value change.
    /// Save the player preferences and change the volume of the effects.
    /// </summary>
    public void onEffectSlide()
    {
        OptionManager.getInstance().setEffectLevel(EffectLevel.value);
        if (sm != null)
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
