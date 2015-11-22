using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorUI : MonoBehaviour {

    static public string error;

    private Text errorText;
   

	// Use this for initialization
	void Start ()
    {
        error = "";
        errorText = GameObject.Find("ErrorText").GetComponent<Text>();
        if (errorText == null)
            Application.Quit();
        errorText.text = error;
	}

}
