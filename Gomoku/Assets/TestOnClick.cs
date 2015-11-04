using UnityEngine;
using System.Collections;

public class TestOnClick : MonoBehaviour {

    public GameObject prefab;

    private bool taken = false;
    private GameObject pion;

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        if (taken == false)
        {
            taken = true;
            pion = (GameObject)Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            taken = false;
            Destroy(pion);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
