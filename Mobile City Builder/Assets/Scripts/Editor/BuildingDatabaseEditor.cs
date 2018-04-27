using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class BuildingDatabaseEditor : EditorWindow {

	private enum State {
		LIST,
		EDIT
	}

	private State state;

	private const string DATABASE_PATH = @"Assets/Data/BuildingDB.asset";

	[MenuItem("Window/Building Database")]
	static void Init () {
		BuildingDatabaseEditor window = (BuildingDatabaseEditor)EditorWindow.GetWindow (typeof(BuildingDatabaseEditor));
		window.Show ();
	}

	private void OnEnable () {
		state = State.LIST;
	}

	private void LoadDatabase () {

	}

	private void CreateDatabase () {
		
	}

	private void OnGUI () {
		DisplayEditor ();
	}

	private void DisplayEditor () {
		switch (state) {
		case State.LIST:
			DisplayListArea ();
			break;
		case State.EDIT:
			DisplayEditArea ();
			break;
		}
	}

	private void DisplayListArea() {

	}

	private void DisplayEditArea() {
		
	}
		
}
