using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Main", menuName = "Buildings/New Main")]
public class MainBuilding : Building {

	[SerializeField]
	private int _cashCapacity;

	public int CashCapacity {
		get { return _cashCapacity; }
		set { _cashCapacity = value; }
	}
		
}
