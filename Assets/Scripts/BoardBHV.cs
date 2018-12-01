using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBHV : MonoBehaviour {

    [Header("General Settings")]
    public int boardSize = 4;
    public float step = 1.1f;
    public int GAME_MODE = 0;
    [Header("Game Mode 2")]
    public float summonDelay = 2.0f;
    [Header("References")]
    public GameObject[] unitPrefabs;
    private ElementalBHV[][] boardUnits = null;

    private void Awake()
    {
        boardUnits = new ElementalBHV[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            boardUnits[i] = new ElementalBHV[boardSize];
            for (int j = 0; j < boardSize; j++)
            {
                boardUnits[i][j] = null;
            }
        }
    }

    // Use this for initialization
    void Start () {
        if (GAME_MODE == 0 || GAME_MODE == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                Spawn();
            }
        }
		else if (GAME_MODE == 2)
        {
            StartCoroutine(KeepSummoning());
        }
	}

	
	// Update is called once per frame
	void Update () {
        // 0 to horizontal axis; 1 to vertical axis
		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveUnits(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveUnits(1, -1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            MoveUnits(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            MoveUnits(0, -1);
        }
    }

    private void Spawn()
    {
        
        bool tileChosen = false;
        int x = -1;
        int y = -1;
        int tries = 0; //FIXME: find a more elegant way to handle it when the board is full
        while (!tileChosen && tries < 100000)
        {
            tries++;
            x = Random.Range(0, boardSize);
            y = Random.Range(0, boardSize);
            if (boardUnits[x][y] == null)
            {
                
                tileChosen = true;
            }
        }
        if (tileChosen)
        {
            GameObject obj = Instantiate(unitPrefabs[Random.Range(0, unitPrefabs.Length)], transform);
            boardUnits[x][y] = obj.GetComponent<ElementalBHV>();
            obj.transform.localPosition = new Vector2(x * step, y * step);
        }
    }

    private void MoveUnits (int axis, int dir)
    {
        int startIndex = (dir == 1) ? boardSize-1 : 0;
        int endIndex = (dir == 1) ? -1 : boardSize;
        for (int i = startIndex; i != endIndex; i -= dir)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int index1 = (axis == 0) ? i : j;
                int index2 = (axis == 0) ? j : i;
                if (boardUnits[index1][index2] != null)
                {
                    Vector3 offset = (axis == 0) ? Vector3.right * dir * step : Vector3.up * dir * step;
                    MoveUnit(index1, index2, offset);
                } 
            }
        }
        DebugPrint();
        if (GAME_MODE == 0 || GAME_MODE == 1) Spawn(); // temporary location: check game design
        DebugPrint();
    }

    private void MoveUnit(int index1, int index2, Vector3 offset)
    {
        int newIndex1 = index1 + (int)offset.x;
        int newIndex2 = index2 + (int)offset.y;
        if (newIndex1 < 0 || newIndex2 < 0 || newIndex1 >= boardSize || newIndex2 >= boardSize) // trying to move beyond borders
        {
            return;
        }
        if (boardUnits[newIndex1][newIndex2] == null)
        {
            boardUnits[index1][index2].transform.position += offset;
            boardUnits[newIndex1][newIndex2] = boardUnits[index1][index2];
            boardUnits[index1][index2] = null;
        }
        else if (boardUnits[newIndex1][newIndex2].Matches(boardUnits[index1][index2]))
        {
            Destroy(boardUnits[index1][index2].gameObject);
            boardUnits[index1][index2] = null;
            boardUnits[newIndex1][newIndex2].LevelUp();
        }
        else if (GAME_MODE != 0 && boardUnits[index1][index2].Overwhelms(boardUnits[newIndex1][newIndex2])){
            Destroy(boardUnits[newIndex1][newIndex2].gameObject);
            boardUnits[index1][index2].transform.position += offset;
            boardUnits[newIndex1][newIndex2] = boardUnits[index1][index2];
            boardUnits[index1][index2] = null;
        }
    }

    private void DebugPrint()
    {
        string s = "";
        for (int j = boardSize-1; j >=0; j--)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (boardUnits[i][j] == null) s += "null";
                s += boardUnits[i][j] + " ";
            }
            s += " == ";
        }
        Debug.Log(s);
    }

    private IEnumerator KeepSummoning()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(summonDelay);
        }
    }
}
