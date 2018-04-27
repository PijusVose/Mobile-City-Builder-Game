using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingSubClass {

	[SerializeField]
	private Building _mainBuilding;

	public Building Main {
		get { return _mainBuilding; }
		set { _mainBuilding = value; }
	}

}
