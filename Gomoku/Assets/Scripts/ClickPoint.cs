using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClickPoint : MonoBehaviour {

    public GameObject[] pawnTPrefabs;
    public GameObject[] pawnPrefabs;

    private GameObject pawn = null;
    private GameManager gameManager;
    private bool taken = false;
    private PawnPool pool;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager == null)
            Debug.Log("Missing the GameManager"); //throw error
        pool = GameObject.FindObjectOfType<PawnPool>();
        if (pool == null)
            Debug.Log("Missing the PawnPool");
    }

    void OnMouseEnter()
    {
        if (pawn == null && !taken)
        {
            pawn = pool.useTpawn(gameManager.PlayTurn);
            pawn.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }

    void OnMouseExit()
    {
        if (pawn != null && !taken)
        {
            pool.ReleaseTpawn(pawn);
            pawn = null;
        }
    }

    void OnMouseDown()
    {
        if (!taken && gameManager.isPlayable(this) && pawn != null)
        {
            pool.ReleaseTpawn(pawn);
            Vector3 position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
            pawn = pool.usePawn(gameManager.PlayTurn);
            pawn.transform.position = position;
            taken = true;
            gameManager.Played(this);
        }
    }
}
