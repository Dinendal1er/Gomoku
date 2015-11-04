using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public GameObject clickPointPrefab;

    private bool playTurn = false;
    private Dictionary<Vector2, ClickPoint> usedPoint;
    private Dictionary<Vector2, bool> pawnPosition;

    public bool PlayTurn
    {
        get
        {
            return playTurn;
        }
    }

	// Use this for initialization
	void Start () {
        int x;
        int y;

        usedPoint = new Dictionary<Vector2, ClickPoint>();
        pawnPosition = new Dictionary<Vector2, bool>();
        for (x = 0; x != 19; ++x)
        {
            for (y = 0; y != 19; ++y)
            {
                Instantiate(clickPointPrefab, new Vector3(x, 0.5f, y), Quaternion.identity);
            }
        }
	
	}

    public void Played(ClickPoint point)
    {
        usedPoint.Add(new Vector2(point.transform.position.x, point.transform.position.z), point);
        pawnPosition.Add(new Vector2(point.transform.position.x, point.transform.position.z), playTurn);
        playTurn = !playTurn;

    }

    public bool isPlayable(ClickPoint point)
    {
        return true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
