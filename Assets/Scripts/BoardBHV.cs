using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBHV : MonoBehaviour {


    public int boardSize = 4;
    public float step = 1.1f;
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
		for (int i = 0; i < 3; i++)
        {
            GameObject obj = Instantiate(unitPrefabs[Random.Range(0, unitPrefabs.Length)], transform);
            bool tileChosen = false;
            while (!tileChosen)
            {
                int x = Random.Range(0, boardSize);
                int y = Random.Range(0, boardSize);
                if (boardUnits[x][y] == null)
                {
                    boardUnits[x][y] = obj.GetComponent<ElementalBHV>();
                    obj.transform.localPosition = new Vector2(x * step, y * step);
                    tileChosen = true;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        // 0 to horizontal axis; 1 to vertical axis
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUnits(1, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveUnits(1, -1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveUnits(0, 1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveUnits(0, -1);
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
                Debug.Log("i: " + i + "-  j: " + j);
                int index1 = (axis == 0) ? i : j;
                int index2 = (axis == 0) ? j : i;
                if (boardUnits[index1][index2] != null)
                {
                    Vector3 offset = (axis == 0) ? Vector3.right * dir * step : Vector3.up * dir * step;
                    MoveUnit(index1, index2, offset);
                } 
            }
        }
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
        else if (boardUnits[newIndex1][newIndex2])
        
    }
}
