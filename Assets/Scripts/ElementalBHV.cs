using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBHV : MonoBehaviour {


    private TextMesh textMesh;
    private int level = 1;
    public int Level
    {
        get => return level;
        set => level = value;
    }

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMesh>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LevelUp()
    {
        level++;
        textMesh.text = level.ToString();
    }


}
