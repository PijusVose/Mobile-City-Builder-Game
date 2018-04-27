using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
	LOADING = 0,
	GAME = 1,
	BUILD = 2,
	MENU = 3
}

public class StateController : MonoBehaviour {

	public State gameState;

	private EventController eventController;

	private void Start() {
		SetInitialReferences ();

		eventController.CallChangeState ("LOADING");
	}

	private void SetInitialReferences() {
		eventController = GameController.Instance.eventController;
		if (eventController == null)
			eventController = GameObject.FindObjectOfType<EventController> ();

		eventController.EventChangeState += ChangeState;
	}

	private void OnDisable() {
		eventController.EventChangeState -= ChangeState;
	}

	public void ChangeState(string stateName) {
		gameState = (State)System.Enum.Parse (typeof(State), stateName);
	}

}
