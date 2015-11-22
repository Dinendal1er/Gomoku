using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Cases
{
    black = 0,
    white = 1,
    empty = 2
};

public class GameManager : MonoBehaviour {

    public GameObject clickPointPrefab;
    private bool isPaused;

    private bool firstTurn;
    private bool playTurn;
    private bool finish;
    private Dictionary<Vector2, ClickPoint> usedPoint;
    private Cases[][] map;
    private int[] captured;
    private List<ArrayList> pWin;
    private GameUIManager gameUI;

    public bool IsPaused
    {
        get
        {
            return isPaused;
        }
        set
        {
            isPaused = value;
        }
    }

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

        firstTurn = true;
        finish = false;
        isPaused = false;
        playTurn = false;
        try
        {
            gameUI = GameObject.FindObjectOfType<GameUIManager>();
            if (gameUI == null)
                throw new NullReferenceException("Missing the gameUI");
        }
        catch (Exception e)
        {
            ErrorUI.error = e.Message;
            LevelManager.getInstance().LoadLevel("Error");
            Destroy(GameObject.Find("UIManager"));
        }
        usedPoint = new Dictionary<Vector2, ClickPoint>();
        pWin = new List<ArrayList>();
        map = new Cases[19][];
        captured = new int[2];
        captured[0] = 0;
        captured[1] = 0;
        for (x = 0; x != 19; ++x)
        {
            map[x] = new Cases[19];
            for (y = 0; y != 19; ++y)
            {
                map[x][y] = Cases.empty;
                Instantiate(clickPointPrefab, new Vector3(x, 0.5f, y), Quaternion.identity);
            }
        }
	
	}

    public void Played(ClickPoint point)
    {
        firstTurn = false;
        usedPoint.Add(new Vector2(point.transform.position.x, point.transform.position.z), point);
        map[Convert.ToInt32(point.transform.position.x)][Convert.ToInt32(point.transform.position.z)] = (Cases)Convert.ToInt32(playTurn);
        CheckCapture(Convert.ToInt32(point.transform.position.x), Convert.ToInt32(point.transform.position.z), (Cases)Convert.ToInt32(playTurn));
        CheckWin(Convert.ToInt32(point.transform.position.x), Convert.ToInt32(point.transform.position.z), (Cases)Convert.ToInt32(playTurn));
        playTurn = !playTurn;
    }

    private bool isCapturable(int x, int y, Cases pawn)
    {
        int h = 0;

        if ((x == 0 || x == 18) && (y == 0 || y == 18))
            return false;
        for (int i = 1; y + i < 18 && map[x][y + i] == pawn && h < 2; ++i)
            ++h;
        for (int i = -1; y + i > 0 && map[x][y + i] == pawn && h < 2; --i)
            ++h;
        if (h == 1)
        {
            if (x == 0 || x == 18)
            {
                if ((y + 1 == 18 && map[x][y + 1] == pawn) || (y - 1 == 0 && map[x][y - 1] == pawn))
                    return false;
                if ((y + 2 <= 18 && map[x][y + 1] != Cases.empty && map[x][y + 1] != pawn && map[x][y + 1] == map[x][y - 2]) ||
                    (y - 2 >= 0 && map[x][y - 1] != Cases.empty && map[x][y - 1] != pawn && map[x][y - 1] == map[x][y + 2]))
                    return false;
            }
            return true;
        }
        h = 0;
        for (int i = 1; x + i < 18 && map[x + i][y] == pawn && h < 2; ++i)
            ++h;
        for (int i = -1; x + i > 0 && map[x + i][y] == pawn && h < 2; --i)
            ++h;
        if (h == 1)
        {
            if (y == 0 || y == 18)
            {
                if ((x + 1 == 18 && map[x + 1][y] == pawn) || (x - 1 == 0 && map[x - 1][y] == pawn))
                    return false;
                if ((x - 2 >= 0 && map[x + 1][y] != Cases.empty && map[x + 1][y] != pawn && map[x + 1][y] == map[x - 2][y]) ||
                    (x + 2 <= 18 && map[x - 1][y] != Cases.empty && map[x - 1][y] != pawn && map[x - 1][y] == map[x + 2][y]))
                    return false;
            }
            return true;
        }
        if (x != 0 && x != 18 && y != 0 && y != 18)
        {
            h = 0;
            int k = 1;
            int j = 1;
            while (x + k < 18 && y + j < 18 && map[x + k][y + j] == pawn && h < 2)
            {
                ++k;
                ++j;
                ++h;
                if (h >= 2)
                    break;
            }
            k = -1;
            j = -1;
            while (x + k > 0 && y + j > 0 && map[x + k][y + j] == pawn && h < 2)
            {
                --k;
                --j;
                ++h;
                if (h >= 2)
                    break;
            }
            if (h == 1)
            {
                if ((x + 1 != 18 && y + 1 != 18) && (x - 1 != 0 && y - 1 != 0))
                    if (!((x - 2 >= 0 && y - 2 >= 0 && map[x + 1][y + 1] != Cases.empty && map[x + 1][y + 1] != pawn && map[x + 1][y + 1] == map[x - 2][y - 2]) ||
                        (x + 2 <= 18 && y + 2 <= 18 && map[x - 1][y - 1] != Cases.empty && map[x - 1][y - 1] != pawn && map[x - 1][y - 1] == map[x + 2][y + 2])))
                            return true;
            }
            h = 0;
            k = 1;
            j = -1;
            while (x + k < 18 && y + j > 0 && map[x + k][y + j] == pawn && h < 2)
            {
                ++k;
                --j;
                ++h;
                if (h >= 2)
                    break;
            }
            k = -1;
            j = 1;
            while (x + k > 0 && y + j < 18 && map[x + k][y + j] == pawn && h < 2)
            {
                --k;
                ++j;
                ++h;
            }
            if (h == 1)
            {
                if ((x - 1 != 0 && y + 1 != 18) && (x + 1 != 18 && y - 1 != 0))
                    if (!((x - 2 >= 0 && y + 2 <= 18 && map[x - 1][y + 1] != Cases.empty && map[x - 1][y + 1] != pawn && map[x - 1][y + 1] == map[x + 2][y - 2]) ||
                        (x + 2 <= 18 && y - 2 >= 0 && map[x + 1][y - 1] != Cases.empty && map[x + 1][y - 1] != pawn && map[x + 1][y - 1] == map[x - 2][y + 2])))
                        return true;
            }
        }
        return false;
    }

    private void CheckWin(int x, int y, Cases pawn)
    {
        if (usedPoint.Count == 361)
            gameUI.DrawScreen();
        if (captured[Convert.ToInt32(playTurn)] >= 10)
        {
            gameUI.WinScreen(Convert.ToInt32(playTurn) + 1);
            finish = true;
        }
        CheckFive(x, y, pawn);
        if (pWin.Count > 0)
        {
            foreach(ArrayList t in pWin)
            {
                ReCheck(t);
            }
        }

    }

    private void ReCheck(ArrayList points)
    {
        int l = 0;
        bool legit = false;
        int valid = 0;
        Cases pawn;
        Vector2 tmp = (Vector2)points[0];

        pawn = map[(int)tmp.x][(int)tmp.y];
        foreach (Vector2 t in points)
        {
            if (map[(int)t.x][(int)t.y] == pawn)
            {
                ++l;
                if (l >= 5)
                    legit = true;
                if (isCapturable((int)t.x, (int)t.y, pawn) == false)
                    ++valid;
                else
                    valid = 0;
            }
            else
                l = 0;
            if (valid >= 5)
            {
                gameUI.WinScreen(Convert.ToInt32(pawn) + 1);
                finish = true;
                return;
            }
        }
        if (!legit)
            pWin.Remove(points);
    }

    private void CheckFive(int x, int y, Cases pawn)
    {
        int l = 0;
        int tx = 0;
        int ty = 0;
        int valid = 0;
        ArrayList points;

        points = new ArrayList();
        for (int i = 0; x + i < 18 && map[x + i][y] == pawn; ++i)
            ++l;
        tx = x + (l - 1);
        l = 0;
        for(int i = 0; tx + i > 0 && map[tx + i][y] == pawn; --i)
        {
            ++l;
            points.Add(new Vector2(tx + i, y));
            if (isCapturable(tx + i, y, pawn) == false)
                valid++;
            else
                valid = 0;
            if (valid >= 5)
            {
                gameUI.WinScreen(Convert.ToInt32(pawn) + 1);
                finish = true;
                return;
            }
        }
        if (l >= 5 && !pWin.Contains(points))
            pWin.Add((ArrayList)points.Clone());
        points.Clear();
        l = 0;
        valid = 0;
        for (int i = 0; y + i < 18 && map[x][y + i] == pawn; ++i)
            ++l;
        ty = y + (l - 1);
        l = 0;
        for (int i = 0; ty + i > 0 && map[x][ty + i] == pawn; --i)
        {
            ++l;
            points.Add(new Vector2(x, ty + i));
            if (isCapturable(x, ty + i, pawn) == false)
                valid++;
            else
                valid = 0;
            if (valid >= 5)
            {
                gameUI.WinScreen(Convert.ToInt32(pawn) + 1);
                finish = true;
                return;
            }
        }
        if (l >= 5 && !pWin.Contains(points))
            pWin.Add((ArrayList)points.Clone());
        points.Clear();
        l = 0;
        valid = 0;
        for (int i = 0; x + i < 18 && y + i < 18 && map[x + i][y + i] == pawn; ++i)
            ++l;
        tx = x + (l - 1);
        ty = y + (l - 1);
        l = 0;
        for (int i = 0; tx + i > 0 && ty + i > 0 && map[tx + i][ty + i] == pawn; --i)
        {
            ++l;
            points.Add(new Vector2(tx + i, ty + i));
            if (isCapturable(tx + i, ty + i, pawn) == false)
                valid++;
            else
                valid = 0;
            if (valid >= 5)
            {
                gameUI.WinScreen(Convert.ToInt32(pawn) + 1);
                finish = true;
                return;
            }
        }
        if (l >= 5 && !pWin.Contains(points))
            pWin.Add((ArrayList)points.Clone());
        points.Clear();
        valid = 0;
        l = 0;
        int k = 0;
        int j = 0;
        while (x + k > 0 && y + j < 18 && map[x + k][y + j] == pawn)
        {
            --k;
            ++j;
            ++l;
        }
        tx = x - (l - 1);
        ty = y + (l - 1);
        l = 0;
        k = 0;
        j = 0;
        while (tx + k < 18 && ty + j > 0 && map[tx + k][ty + j] == pawn)
        {
            ++l;
            ++k;
            --j;
            points.Add(new Vector2(tx + k, ty + j));
            if (isCapturable(tx + k, ty + j, pawn) == false)
                valid++;
            else
                valid = 0;
            if (valid >= 5)
            {
                gameUI.WinScreen(Convert.ToInt32(pawn) + 1);
                finish = true;
                return;
            }
        }
        if (l >= 5 && !pWin.Contains(points))
            pWin.Add((ArrayList)points.Clone());
        points.Clear();
    }

    private void CheckCapture(int x, int y, Cases pawn)
    {
        if (x <= 15 &&
            ((map[x + 1][y] != pawn && map[x + 1][y] != Cases.empty) &&
            (map[x + 2][y] != pawn && map[x + 2][y] != Cases.empty) &&
            map[x + 3][y] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x + 1, y)].OnCapture();
            usedPoint.Remove(new Vector2(x + 1, y));
            usedPoint[new Vector2(x + 2, y)].OnCapture();
            usedPoint.Remove(new Vector2(x + 2, y));
            map[x + 1][y] = Cases.empty;
            map[x + 2][y] = Cases.empty;
        }
        if (x >= 3 &&
            ((map[x - 1][y] != pawn && map[x - 1][y] != Cases.empty) &&
            (map[x - 2][y] != pawn && map[x - 2][y] != Cases.empty) &&
            map[x - 3][y] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x - 1, y)].OnCapture();
            usedPoint.Remove(new Vector2(x - 1, y));
            usedPoint[new Vector2(x - 2, y)].OnCapture();
            usedPoint.Remove(new Vector2(x - 2, y));
            map[x - 1][y] = Cases.empty;
            map[x - 2][y] = Cases.empty;
        }
        if (y <= 15 &&
            ((map[x][y + 1] != pawn && map[x][y + 1] != Cases.empty) &&
            (map[x][y + 2] != pawn && map[x][y + 2] != Cases.empty) &&
            map[x][y + 3] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x, y + 1)].OnCapture();
            usedPoint.Remove(new Vector2(x, y + 1));
            usedPoint[new Vector2(x, y + 2)].OnCapture();
            usedPoint.Remove(new Vector2(x, y + 2));
            map[x][y + 1] = Cases.empty;
            map[x][y + 2] = Cases.empty;
        }
        if (y >= 3 &&
            ((map[x][y - 1] != pawn && map[x][y - 1] != Cases.empty) &&
            (map[x][y - 2] != pawn && map[x][y - 2] != Cases.empty) &&
            map[x][y - 3] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x, y - 1)].OnCapture();
            usedPoint.Remove(new Vector2(x, y - 1));
            usedPoint[new Vector2(x, y - 2)].OnCapture();
            usedPoint.Remove(new Vector2(x, y - 2));
            map[x][y - 1] = Cases.empty;
            map[x][y - 2] = Cases.empty;
        }
        if (y <= 15 && x <= 15 &&
            ((map[x + 1][y + 1] != pawn && map[x + 1][y + 1] != Cases.empty) &&
            (map[x + 2][y + 2] != pawn && map[x + 2][y + 2] != Cases.empty) &&
            map[x + 3][y + 3] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x + 1, y + 1)].OnCapture();
            usedPoint.Remove(new Vector2(x + 1, y + 1));
            usedPoint[new Vector2(x + 2, y + 2)].OnCapture();
            usedPoint.Remove(new Vector2(x + 2, y + 2));
            map[x + 1][y + 1] = Cases.empty;
            map[x + 2][y + 2] = Cases.empty;
        }
        if (y >= 3  &&  x <= 15 &&
            ((map[x + 1][y - 1] != pawn && map[x + 1][y - 1] != Cases.empty) &&
            (map[x + 2][y - 2] != pawn && map[x + 2][y - 2] != Cases.empty) &&
            map[x + 3][y - 3] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x + 1, y - 1)].OnCapture();
            usedPoint.Remove(new Vector2(x + 1, y - 1));
            usedPoint[new Vector2(x + 2, y - 2)].OnCapture();
            usedPoint.Remove(new Vector2(x + 2, y - 2));
            map[x + 1][y - 1] = Cases.empty;
            map[x + 2][y - 2] = Cases.empty;
        }
        if (y <= 15 && x >= 3 &&
            ((map[x - 1][y + 1] != pawn && map[x - 1][y + 1] != Cases.empty) &&
            (map[x - 2][y + 2] != pawn && map[x - 2][y + 2] != Cases.empty) &&
            map[x - 3][y + 3] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x - 1, y + 1)].OnCapture();
            usedPoint.Remove(new Vector2(x - 1, y + 1));
            usedPoint[new Vector2(x - 2, y + 2)].OnCapture();
            usedPoint.Remove(new Vector2(x - 2, y + 2));
            map[x - 1][y + 1] = Cases.empty;
            map[x - 2][y + 2] = Cases.empty;
        }
        if (y >= 3 && x >= 3 &&
            ((map[x - 1][y - 1] != pawn && map[x - 1][y - 1] != Cases.empty) &&
            (map[x - 2][y - 2] != pawn && map[x - 2][y - 2] != Cases.empty) &&
            map[x - 3][y - 3] == pawn))
        {
            captured[(int)pawn] += 2;
            usedPoint[new Vector2(x - 1, y - 1)].OnCapture();
            usedPoint.Remove(new Vector2(x - 1, y - 1));
            usedPoint[new Vector2(x - 2, y - 2)].OnCapture();
            usedPoint.Remove(new Vector2(x - 2, y - 2));
            map[x - 1][y - 1] = Cases.empty;
            map[x - 1][y - 2] = Cases.empty;
        }
        gameUI.UpdateCount(captured);
    }

    public int CheckDoubleInter(int x, int y, Cases pawn)
    {
        int spaces = 0;
        int three = 0;
        int h = 0;
        if (x != 0 && x != 18 && map[x - 1][y] == pawn && map[x + 1][y] == pawn)
        {
            for (int i = 1; x + i < 18 && map[x + i][y] == pawn; ++i)
                ++h;
            for (int i = -1; x + i > 0 && map[x + i][y] == pawn; --i)
                ++h;
            if (h == 2)
                ++three;
            h = 0;
        }
        else
        {
            for (int i = 0; x + i < 18; ++i)
            {
                if (map[x + i][y] == pawn && i != 0)
                    ++h;
                else if (map[x + i][y] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y] != Cases.empty && map[x + i][y] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
            spaces = 0;
            h = 0;
            for (int i = 0; x + i > 0; --i)
            {
                if (map[x + i][y] == pawn && i != 0)
                    ++h;
                else if (map[x + i][y] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y] != Cases.empty && map[x + i][y] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
        }
        h = 0;
        spaces = 0;
        if (y != 0 && y != 18 && map[x][y - 1] == pawn && map[x][y + 1] == pawn)
        {
            for (int i = 1; y + i < 18 && map[x][y + i] == pawn; ++i)
                ++h;
            for (int i = -1; y + i > 0 && map[x][y + i] == pawn; --i)
                ++h;
            if (h == 2)
                ++three;
            h = 0;
        }
        else
        {
            for (int i = 0; y + i < 18; ++i)
            {
                if (map[x][y + i] == pawn && i != 0)
                    ++h;
                else if (map[x][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x][y + i] != Cases.empty && map[x][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
            spaces = 0;
            h = 0;
            for (int i = 0; y + i > 0; --i)
            {
                if (map[x][y + i] == pawn && i != 0)
                    ++h;
                else if (map[x][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x][y + i] != Cases.empty && map[x][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
        }
        spaces = 0;
        h = 0;
        if (x != 0 && x != 18 && y != 0 && y != 18 && map[x - 1][y - 1] == pawn && map[x + 1][y + 1] == pawn)
        {
            for (int i = 1; x + i < 18 && y + i < 18 && map[x + i][y + i] == pawn; ++i)
            {
                ++h;
            }
            for (int i = -1; x + i > 0 && y + i > 0 && map[x + i][y + i] == pawn; --i)
                ++h;
            if (h == 2)
                ++three;
            h = 0;
        }
        else
        {
            for (int i = 0; x + i < 18 && y + i < 18; ++i)
            {
                if (map[x + i][y + i] == pawn && i != 0)
                    ++h;
                else if (map[x + i][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y + i] != Cases.empty && map[x + i][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
            spaces = 0;
            h = 0;
            for (int i = 0; x + i > 0 && y + i > 0; --i)
            {
                if (map[x + i][y + i] == pawn && i != 0)
                    ++h;
                else if (map[x + i][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y + i] != Cases.empty && map[x + i][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
        }
        h = 0;
        spaces = 0;
        if (x != 0 && x != 18 && y != 0 && y != 18 && map[x - 1][y + 1] == pawn && map[x + 1][y - 1] == pawn)
        {
            for (int i = 1; x + i < 18 && y - i > 0 && map[x + i][y - i] == pawn; ++i)
                ++h;
            for (int i = -1; x + i > 0 && y - i < 18 && map[x + i][y - i] == pawn; --i)
                ++h;
            if (h == 2)
                ++three;
            h = 0;
        }
        else
        {
            for (int i = 0; x + i < 18 && y - i > 0; ++i)
            {
                if (map[x + i][y - i] == pawn && i != 0)
                    ++h;
                else if (map[x + i][y - i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y - i] != Cases.empty && map[x + i][y - i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
            spaces = 0;
            h = 0;
            for (int i = 0; x + i > 0 && y - i < 18; --i)
            {
                if (map[x + i][y - i] == pawn && i != 0)
                    ++h;
                else if (map[x + i][y - i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y - i] != Cases.empty && map[x + i][y - i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
                ++three;
        }
        return three;
    }

    public bool CheckDouble(int x, int y, Cases pawn)
    {
        int spaces = 0;
        int three = 0;
        int potThree = 0;
        int h = 0;
        if (x != 0 && x != 18 && map[x - 1][y] == pawn && map[x + 1][y] == pawn)
        {
            for (int i = 1; x + i < 18 && map[x + i][y] == pawn; ++i)
            {
                potThree += CheckDoubleInter(x + i, y, pawn);
                ++h;
            }
            for (int i = -1; x + i > 0 && map[x + i][y] == pawn; --i)
            {
                potThree += CheckDoubleInter(x + i, y, pawn);
                ++h;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            h = 0;
            potThree = 0;
        }
        else
        {
            for (int i = 0; x + i < 18; ++i)
            {
                if (map[x + i][y] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x + i, y, pawn);
                }
                else if (map[x + i][y] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y] != Cases.empty && map[x + i][y] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            spaces = 0;
            h = 0;
            potThree = 0;
            for (int i = 0; x + i > 0; --i)
            {
                if (map[x + i][y] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x + i, y, pawn);
                }
                else if (map[x + i][y] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y] != Cases.empty && map[x + i][y] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
        }
        potThree = 0;
        h = 0;
        spaces = 0;
        if (y != 0 && y != 18 && map[x][y - 1] == pawn && map[x][y + 1] == pawn)
        {
            for (int i = 1; y + i < 18 && map[x][y + i] == pawn; ++i)
            {
                ++h;
                potThree += CheckDoubleInter(x, y + i, pawn);
            }
            for (int i = -1; y + i > 0 && map[x][y + i] == pawn; --i)
            {
                ++h;
                potThree += CheckDoubleInter(x, y + i, pawn);
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            h = 0;
            potThree = 0;
        }
        else
        {
            for (int i = 0; y + i < 18; ++i)
            {
                if (map[x][y + i] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x, y + i, pawn);
                }
                else if (map[x][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x][y + i] != Cases.empty && map[x][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            spaces = 0;
            h = 0;
            potThree = 0;
            for (int i = 0; y + i > 0; --i)
            {
                if (map[x][y + i] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x, y + i, pawn);
                }
                else if (map[x][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x][y + i] != Cases.empty && map[x][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
        }
        spaces = 0;
        h = 0;
        potThree = 0;
        if (x != 0 && x != 18 && y != 0 && y != 18 && map[x - 1][y - 1] == pawn && map[x + 1][y + 1] == pawn)
        {
            for (int i = 1; x + i < 18 && y + i < 18 && map[x + i][y + i] == pawn; ++i)
            {
                ++h;
                potThree += CheckDoubleInter(x + i, y + i, pawn);
            }
            for (int i = -1; x + i > 0 && y + i > 0 && map[x + i][y + i] == pawn; --i)
            {
                ++h;
                potThree += CheckDoubleInter(x + i, y + i, pawn);
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            h = 0;
            potThree = 0;
        }
        else
        {
            for (int i = 0; x + i < 18 && y + i < 18; ++i)
            {
                if (map[x + i][y + i] == pawn)
                { 
                    ++h;
                    potThree += CheckDoubleInter(x + i, y + i, pawn);
                }
                else if (map[x + i][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y + i] != Cases.empty && map[x + i][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            spaces = 0;
            h = 0;
            potThree = 0;
            for (int i = 0; x + i > 0 && y + i > 0; --i)
            {
                if (map[x + i][y + i] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x + i, y + i, pawn);
                }
                else if (map[x + i][y + i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y + i] != Cases.empty && map[x + i][y + i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
        }
        h = 0;
        spaces = 0;
        potThree = 0;
        if (x != 0 && x != 18 && y != 0 && y != 18 && map[x - 1][y + 1] == pawn && map[x + 1][y - 1] == pawn)
        {
            for (int i = 1; x + i < 18 && y - i > 0 && map[x + i][y - i] == pawn; ++i)
            {
                ++h;
                potThree += CheckDoubleInter(x + i, y - i, pawn);
            }
            for (int i = -1; x + i > 0 && y - i < 18 && map[x + i][y - i] == pawn; --i)
            {
                ++h;
                potThree += CheckDoubleInter(x + i, y - i, pawn);
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            h = 0;
            potThree = 0;
        }
        else
        {
            for (int i = 0; x + i < 18 && y - i > 0; ++i)
            {
                if (map[x + i][y - i] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x + i, y - i, pawn);
                }
                else if (map[x + i][y - i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y - i] != Cases.empty && map[x + i][y - i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
            spaces = 0;
            h = 0;
            potThree = 0;
            for (int i = 0; x + i > 0 && y - i < 18; --i)
            {
                if (map[x + i][y - i] == pawn)
                {
                    ++h;
                    potThree += CheckDoubleInter(x + i, y - i, pawn);
                }
                else if (map[x + i][y - i] == Cases.empty && i != 0)
                    ++spaces;
                else if (map[x + i][y - i] != Cases.empty && map[x + i][y - i] != pawn && spaces == 0)
                {
                    h = 0;
                    break;
                }
                if (spaces > 1)
                    break;
            }
            if (h == 2)
            {
                ++three;
                three += potThree;
            }
        }
        if (three >= 2)
            return true;
        return false;
    }

    private void ClearRules()
    {
        gameUI.UpdateRulesText("");
    }

    public bool isPlayable(ClickPoint point)
    {
        if (finish == true)
            return false;
        if (firstTurn == true && (point.transform.position.x != 9 || point.transform.position.z != 9))
        {
            gameUI.UpdateRulesText("First player must play at the middle");
            Invoke("ClearRules", 5f);
            return false;
        }
        if (CheckDouble((int)point.transform.position.x, (int)point.transform.position.z, (Cases)Convert.ToInt32(playTurn)) == true)
        {
            gameUI.UpdateRulesText("Double-Three are forbidden");
            Invoke("ClearRules", 5f);
            return false;
        }
        return true;
    }

}
