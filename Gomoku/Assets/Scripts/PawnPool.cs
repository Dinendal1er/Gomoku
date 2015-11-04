using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

public class PawnPool : MonoBehaviour {

    public GameObject[] prefabs;

    private List<KeyValuePair<bool, GameObject>> pawnsBPool;
    private List<KeyValuePair<bool, GameObject>> pawnsWPool;
    private GameObject[] pawnsTPool;
    private Vector3 Origin;
    private KeyValuePair<bool, GameObject> toFind;

	// Use this for initialization
	void Start () {
        Origin = new Vector3(100, 100, 100);
        pawnsTPool = new GameObject[2];
        pawnsTPool[0] = (GameObject)Instantiate(prefabs[2], Origin, prefabs[2].transform.localRotation);
        pawnsTPool[1] = (GameObject)Instantiate(prefabs[3], Origin, prefabs[3].transform.localRotation);
        pawnsBPool = new List<KeyValuePair<bool, GameObject>>();
        pawnsWPool = new List<KeyValuePair<bool, GameObject>>();
        for (int i = 0; i != 60; ++i)
        {
            pawnsBPool.Add(new KeyValuePair<bool, GameObject>(true,
            (GameObject)Instantiate(prefabs[0], Origin, prefabs[0].transform.localRotation)));
            pawnsWPool.Add(new KeyValuePair<bool, GameObject>(true,
            (GameObject)Instantiate(prefabs[1], Origin, prefabs[1].transform.localRotation)));
        }
    }
	
    /// <summary>
    /// Returns an transparent pawn, his color depends on the playerturn(boolean)
    /// </summary>
    /// <param name="playerturn">Boolean, false if it's black player, true if it's white player</param>
    /// <returns>Instance of a transparent pawn (GameObject)</returns>
    public GameObject useTpawn(bool playerturn)
    {
        return (pawnsTPool[Convert.ToInt32(playerturn)]);
    }

    public void ReleaseTpawn(GameObject pawn)
    {
        pawn.transform.position = Origin;
    }

    /// <summary>
    /// Returns the first available pawn his color depends on the boolean playerturn.
    /// </summary>
    /// <param name="playerturn">boolean </param>
    /// <returns></returns>
    public GameObject usePawn(bool playerturn)
    {
        int i = 0;
        if (playerturn == false)
        {
            foreach (KeyValuePair<bool, GameObject> t in pawnsBPool)
            {
                if (t.Key == true)
                {
                    pawnsBPool[i] = new KeyValuePair<bool, GameObject>(false, t.Value);
                    return t.Value;
                }
                ++i;
            }
        }
        else
        {
            foreach (KeyValuePair<bool, GameObject> t in pawnsWPool)
            {
                if (t.Key == true)
                {
                    pawnsWPool[i] = new KeyValuePair<bool, GameObject>(false, t.Value);
                    return t.Value;
                }
                ++i;
            }
        }
        return null;
    }

    public void ReleasePawn(GameObject pawn, bool playerTurn)
    {
        toFind = new KeyValuePair<bool, GameObject>(false, pawn);
        if (playerTurn == true)
        {
            int i = pawnsBPool.FindIndex(
                delegate (KeyValuePair<bool, GameObject> pair)
                {
                    return pair.Value == toFind.Value;
                }
                );
            pawnsBPool[i] = new KeyValuePair<bool, GameObject>(true, pawnsBPool[i].Value);
            pawnsBPool[i].Value.transform.position = Origin;
        }
        else
        {
            int i = pawnsWPool.FindIndex(
                delegate (KeyValuePair<bool, GameObject> pair)
                {
                    return pair.Value == toFind.Value;
                }
                );
            pawnsWPool[i] = new KeyValuePair<bool, GameObject>(true, pawnsWPool[i].Value);
            pawnsWPool[i].Value.transform.position = Origin;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
