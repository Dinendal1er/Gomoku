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

    public GameObject Pawn
    {
        get
        {
            return pawn;
        }
    }

    void Start()
    {
        try
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();
            if (gameManager == null)
                throw new NullReferenceException("Missing the GameManager"); //throw error
            pool = GameObject.FindObjectOfType<PawnPool>();
            if (pool == null)
                throw new NullReferenceException("Missing the PawnPool");
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(GameObject.Find("UIManager"));
        }
    }

    void OnMouseEnter()
    {
        if (gameManager != null && !gameManager.IsPaused && pawn == null && !taken)
        {
            pawn = pool.useTpawn(gameManager.PlayTurn);
            pawn.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
    }

    void OnMouseExit()
    {
        if (gameManager != null && !gameManager.IsPaused && pawn != null && !taken)
        {
            pool.ReleaseTpawn(pawn);
            pawn = null;
        }
    }

    void OnMouseDown()
    {
        if (gameManager != null && (!gameManager.IsPaused) && (!taken) && gameManager.isPlayable(this) == true && pawn != null)
        {
            pool.ReleaseTpawn(pawn);
            Vector3 position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
            pawn = pool.usePawn(gameManager.PlayTurn);
            pawn.transform.position = position;
            taken = true;
            gameManager.Played(this);
        }
    }

    public void OnCapture()
    {
        pool.ReleasePawn(pawn, gameManager.PlayTurn);
        pawn = null;
        taken = false;
    }
}
