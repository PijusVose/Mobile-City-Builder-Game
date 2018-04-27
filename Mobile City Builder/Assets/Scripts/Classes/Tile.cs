using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile {

    [SerializeField]
    private string _name;

	[SerializeField]
	private Vector3 _position;

	[SerializeField]
	private Building _building;

	public Tile(string name, Vector3 pos) {
        _name = name;
		_position = pos;
	}

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public Vector3 Position {
		get { return _position; }
		set { _position = value; }
	}

	public Building Building {
		get { return _building; }
		set { _building = value; }
	}
}
