using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIController : MonoBehaviour {

	/// <summary>
	/// BUildingAccept checks if tile is occuppied. [Last thing I did.]
	/// </summary>

	public GameController gameController;

	public GameObject buildingUIPrefab;
	public GameObject currentBuildingUI;

	public string acceptName = "AcceptButton";
	public string cancelName = "CancelButton";

	private void Start() {
		SetInitialReferences ();
	}

	private void SetInitialReferences() {
		gameController = GameController.Instance;
		if (gameController == null)
			gameController = GameObject.FindObjectOfType<GameController> ();

		gameController.eventController.EventBuildMode += PlaceBuildingUI;
	}

	private void OnDisable() {
		gameController.eventController.EventBuildMode -= PlaceBuildingUI;
	}

	public void PlaceBuildingUI (Vector3 position) {
		if (currentBuildingUI == null) {
			currentBuildingUI = Instantiate (buildingUIPrefab);

			Button[] buttons = currentBuildingUI.GetComponentsInChildren<Button> ();
			for (int b = 0; b < buttons.Length; b++) {
				if (buttons[b].gameObject.name == acceptName) {
					buttons [b].onClick.AddListener (() => BuildingUIAccept());
				} else if (buttons[b].gameObject.name == cancelName) {
					buttons [b].onClick.AddListener (() => BuildingUICancel());
				}
			}
		} 

		if (currentBuildingUI.activeSelf == false)
			currentBuildingUI.gameObject.SetActive (true);

		currentBuildingUI.transform.position = position;
	}

	private void BuildingUIAccept() {
		if (gameController.mapController.CheckIfTileOccupied (currentBuildingUI.transform.position) == null) {
			currentBuildingUI.SetActive (false);

			gameController.buildController.StartPlacementAnimation ();
		}
	}

	void BuildingUICancel() {
		Debug.Log ("Building cancelled.");
	}

}
