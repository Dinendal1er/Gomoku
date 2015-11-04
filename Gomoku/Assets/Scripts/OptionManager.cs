using UnityEngine;
using System.Collections;

public class OptionManager : MonoBehaviour {

    static private OptionManager instance = null;

    private string MUSIC_LEVEL = "MUSIC_LEVEL";
    private string EFFECT_LEVEL = "EFFECT_LEVEL";
	
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
    /// Return the current instance of the OptionManager singleton or null if there is none.
    /// </summary>
    /// <returns>Instance of the current instance.</returns>
    static public OptionManager getInstance()
    {
        if (instance != null)
            return instance;
        return null;
    }

    /// <summary>
    /// Set the value of the "MUSIC_LEVEL" key of the player preferences (PlayerPrefs)
    /// </summary>
    /// <param name="level">value of the Music level as float</param>
    public void setMusicLevel(float level)
    {
        PlayerPrefs.SetFloat(MUSIC_LEVEL, level);
    }

    /// <summary>
    /// Get the "MUSIC_LEVEL" key of the player preferences (PlayerPrefs)
    /// </summary>
    /// <returns>Return a float, value of the Music level</returns>
    public float getMusicLevel()
    {
        return PlayerPrefs.GetFloat(MUSIC_LEVEL, 1f);
    }

    /// <summary>
    /// Set the value of the "EFFECT_LEVEL" key of the player preferences (PlayerPrefs)
    /// </summary>
    /// <param name="level">value of the Effect level as float</param>
    public void setEffectLevel(float level)
    {
        PlayerPrefs.SetFloat(EFFECT_LEVEL, level);
    }

    /// <summary>
    /// Get the value of the "EFFECT_LEVEL" key of the player preferences (PlayerPrefs)
    /// </summary>
    /// <returns>Return a flaot, value of the Effect level</returns>
    public float getEffectLevel()
    {
        return PlayerPrefs.GetFloat(EFFECT_LEVEL, 1f);
    }
}
