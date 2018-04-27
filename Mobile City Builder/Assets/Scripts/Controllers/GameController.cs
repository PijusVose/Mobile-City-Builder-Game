using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

	// All controllers
	public MapController mapController;
	public EventController eventController;
	public UIController uiController;
	public StateController stateController;
	public BuildController buildController;

	private static GameController _instance;

	public static GameController Instance {
		get { 
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(GameController)) as GameController;
			}
			return _instance;
		}
	}

	public bool isLoaded = false;
	public bool isTileMapLoaded = false;

	// Use this for initialization
	private void Start () {
		SetInitialReference ();

		// Check if GameData has been loaded.
		if (!isLoaded) {
			eventController.CallLoadGameData ();
		}

        Screen.orientation = ScreenOrientation.AutoRotation;
	}
		
	// Subscribes methods to the right event and sets variables.
	private void SetInitialReference() {
		eventController.EventLoadGameData += LoadGameSave;
	}

	private void OnDisable() {
		eventController.EventLoadGameData -= LoadGameSave;
	}

    public void LaunchGame() {
        
    }

	// Loads the GameData.
	public void LoadGameSave() {
		isLoaded = true;

		// After GameData has been loaded, start loading the TileMap.
		eventController.CallLoadTileMap ();

		Debug.Log ("Game has loaded.");
	}

}
