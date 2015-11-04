using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    static private SoundManager instance = null;
    private AudioSource musicPlayer;
    private AudioSource effectPlayer;

    public AudioClip[] musics;
    public AudioClip[] effects;


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
    /// Return the current instance of the SoundManager singleton or null is there is none.
    /// </summary>
    /// <returns>SoundMannager singleton</returns>
    static public SoundManager getInstance()
    {
        if (instance != null)
            return instance;
        return null;
    }

    void Start()
    {
        AudioSource[] tmp = gameObject.GetComponents<AudioSource>();
        if (tmp == null || tmp.Length != 2)
            Debug.LogError("Missing audio source component"); //TODO throw exception
        musicPlayer = tmp[0];
        effectPlayer = tmp[1];
        UpdateMusicLevel(OptionManager.getInstance().getMusicLevel());
        UpdateEffectLevel(OptionManager.getInstance().getEffectLevel());
        playMenuMusic();
    }
	
    /// <summary>
    /// Play the MenuMusic on the musicPlayer's AudioSource.
    /// </summary>
    public void playMenuMusic()
    {
        musicPlayer.clip = musics[0];
        musicPlayer.Play();
    }

    /// <summary>
    /// Play the button effect from the effectPlayer's AudioSource
    /// </summary>
    public void playButtonEffect()
    {
        if (effectPlayer.clip != effects[1])
            effectPlayer.clip = effects[1];
        effectPlayer.Play();
    }

    /// <summary>
    /// Update the volume of the Music.
    /// </summary>
    /// <param name="level">value of the volume as a float.</param>
    public void UpdateMusicLevel(float level)
    {
        musicPlayer.volume = level;
    }

    /// <summary>
    /// Update the volume of the effects.
    /// </summary>
    /// <param name="level">value of the volume as a float</param>
    public void UpdateEffectLevel(float level)
    {
        effectPlayer.volume = level;
    }
}
