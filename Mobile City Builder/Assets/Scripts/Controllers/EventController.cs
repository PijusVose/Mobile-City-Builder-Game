using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour {

	public delegate void DefaultEventHandler();
	public event DefaultEventHandler EventLoadGameData;
	public event DefaultEventHandler EventLoadTileMap;

	public delegate void GameStateHandler(string state);
	public event GameStateHandler EventChangeState;

	public delegate void BuildStateHandler(Building building);
	public event BuildStateHandler EventStartBuildMode;
	public event BuildStateHandler EventEndBuildMode;

	public delegate void BuildUIHandler(Vector3 position);
	public event BuildUIHandler EventBuildMode;

	public void CallLoadGameData() {
		if (EventLoadGameData != null) {
			EventLoadGameData ();
		}
	}

	public void CallLoadTileMap() {
		if (EventLoadTileMap != null) {
			EventLoadTileMap ();
		}
	}

	public void CallChangeState(string state) {
		if (EventChangeState != null) {
			EventChangeState (state);
		}
	}

	public void CallStartBuildMode(Building building) {
		if (EventStartBuildMode != null) {
			EventStartBuildMode (building);
		}
	}

	public void CallEndBuildMode(Building building) {
		if (EventEndBuildMode != null) {
			EventEndBuildMode (building);
		}
	}

	public void CallBuildMode(Vector3 position) {
		if (EventBuildMode != null) {
			EventBuildMode (position);
		}
	}

}
