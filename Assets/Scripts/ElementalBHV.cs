using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalBHV : MonoBehaviour {

    public enum Element
    {
        FIRE,
        WATER,
        AIR,
        EARTH
    }

    private TextMesh textMesh;
    public Element type;
    private int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            Debug.Log("Changing Level");
        }
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
        Level++;
        textMesh.text = Level.ToString();
    }

    public bool Matches(ElementalBHV other)
    {
        return other.Level == Level && other.type == type;
    }

    public bool Overwhelms (ElementalBHV other)
    {
        bool strongElement = false;
        switch (other.type)
        {
            case Element.FIRE:
                if (type == Element.WATER)
                {
                    strongElement = true;
                }
                break;
            case Element.WATER:
                if (type == Element.EARTH)
                {
                    strongElement = true;
                }
                break;
            case Element.EARTH:
                if (type == Element.AIR)
                {
                    strongElement = true;
                }
                break;
            case Element.AIR:
                if (type == Element.FIRE)
                {
                    strongElement = true;
                }
                break;
        }
        return other.Level < Level && strongElement;
    }


}
