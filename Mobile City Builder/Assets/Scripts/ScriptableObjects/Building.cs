using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/New Building")]
public class Building : ScriptableObject {

	[SerializeField]
	private string _name;

	[SerializeField]
	private int _numberOfUpgrades;

	[SerializeField]
	private GameObject _prefab;

	public string Name {
		get { return _name; }
		set { _name = value; }
	}

	public int NumberOfUpgrades {
		get { return _numberOfUpgrades; }
	}

	public GameObject Prefab {
		get { return _prefab; }
		set { _prefab = value; }
	}

}
