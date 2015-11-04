using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public  GameObject menuUI;
    public  GameObject optionUI;
    public  GameObject gameOptionUI;

    private CanvasGroup menuGroup;
    private CanvasGroup optionGroup;
    private CanvasGroup gameOptionGroup;
    private bool menuToggle;
    private bool optionToggle;
    private bool gameOptionToggle;
    static private UIManager instance = null;

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
    /// gets the current instance of the UIManager singleton, if there is none, returns null.
    /// </summary>
    /// <returns>Return a UIManager instance.</returns>
    static public UIManager getInstance()
    {
        if (instance != null)
            return instance;
        return null;
    }

	// Use this for initialization
	void Start () {
        menuGroup = menuUI.GetComponent<CanvasGroup>();
        if (menuGroup == null)
            Debug.LogError("Missing the CanvasGroup component on the Menu's UI"); //throw error
        optionGroup = optionUI.GetComponent<CanvasGroup>();
        if (optionGroup == null)
            Debug.LogError("Missing the CanvasGroup component on the Option's UI");
        gameOptionGroup = gameOptionUI.GetComponent<CanvasGroup>();
        if (gameOptionGroup == null)
            Debug.LogError("Missing the CanvasGroup component on the Option's UI");
        menuToggle = false;
        optionToggle = false;
        gameOptionToggle = false;
        MenuToggle();
        //OptionToggle();
	}
	
    /// <summary>
    /// Set On or Off the Menu's UI based on the value of the menuToggle boolean.
    /// </summary>
    public void MenuToggle()
    {
        menuToggle = !menuToggle;
        menuGroup.alpha = (menuToggle == true) ? 1 : 0;
        menuUI.SetActive(menuToggle);
    }

    /// <summary>
    /// Set On or Off the Option's UI based on the value of the optionToggle boolean.
    /// </summary>
    public void OptionToggle()
    {
        optionToggle = !optionToggle;
        optionGroup.alpha = (optionToggle == true) ? 1 : 0;
        optionUI.SetActive(optionToggle);
    }

    /// <summary>
    /// Set On or Off the Option's UI based on the value of the optionToggle boolean.
    /// </summary>
    public void GameOptionToggle()
    {
        gameOptionToggle = !gameOptionToggle;
        gameOptionGroup.alpha = (gameOptionToggle == true) ? 1 : 0;
        gameOptionUI.SetActive(gameOptionToggle);
    }
}
