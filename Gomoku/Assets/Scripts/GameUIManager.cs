using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    private Text pawnCountB;
    private Text pawnCountW;
    private Text rules;
    private Image Win;
    private Image paused;
    private UIManager UM;
    private GameManager gm;

    // Use this for initialization
    void Start () {
        pawnCountB = transform.GetChild(0).transform.GetChild(1).GetComponent<Text>();
        pawnCountW = transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();
        Win = transform.GetChild(2).GetComponent<Image>();
        Win.gameObject.SetActive(false);
        paused = transform.GetChild(3).GetComponent<Image>();
        paused.gameObject.SetActive(false);
        rules = transform.GetChild(4).GetComponent<Text>();
        try {
            UM = UIManager.getInstance();
            if (UM == null)
                throw new NullReferenceException("Missing the UIManager");
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(GameObject.Find("UIManager"));
        }
    }

    void OnEnable()
    {
        if (pawnCountB != null)
            pawnCountB.text = 0 + "\npawns";
        if (pawnCountW != null)
            pawnCountW.text = 0 + "\npawns";
        if (rules != null)
            rules.text = "";
        if (Win != null && Win.gameObject.activeInHierarchy)
            Win.gameObject.SetActive(false);
        if (paused != null && paused.gameObject.activeInHierarchy)
            paused.gameObject.SetActive(false);
    }
	
    public void UpdateCount(int[] count)
    {
        pawnCountB.text = count[0] + "\npawns";
        pawnCountW.text = count[1] + "\npawns";
    }
    
    public void WinScreen(int player)
    {
        Win.gameObject.SetActive(true);
        Win.transform.GetComponentInChildren<Text>().text = "Player " + player + " Win !";
    }

    public void UpdateRulesText(string s)
    {
        rules.text = s;
    }

    public void DrawScreen()
    {
        Win.gameObject.SetActive(true);
        Win.transform.GetComponentInChildren<Text>().text = "Draw !";
    }

    public void ToMenu()
    {
        UM.GameToggle();
        UM.MenuToggle();
        LevelManager.getInstance().LoadLevel("Menu");
    }

    public void Paused()
    {
        try
        {
        gm = GameObject.FindObjectOfType<GameManager>();
        if (gm == null)
            throw new NullReferenceException("Missing the GameManager");
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(GameObject.Find("UIManager"));
        }
        paused.gameObject.SetActive(!paused.gameObject.activeInHierarchy);
        gm.IsPaused = !gm.IsPaused;
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("escape"))
            Paused();
	}
}
