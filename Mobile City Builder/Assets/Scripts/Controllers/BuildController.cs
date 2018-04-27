using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BuildController : MonoBehaviour {

	public List<Building> buildings;
	public GameController gameController;
	public LayerMask raycastMask;
	public GameObject previewObjectPrefab;
	public Sprite positiveOutline;
	public Sprite negativeOutline;

	public Building buildingToPlace;
	private Transform previewObject;
	private Transform currentBuildingPreview;
	private Tile currentTile;
	private Tile tileToPlaceOn;
	private bool isDragging = false;

	// Use this for initialization
	private void Start () {
		SetInitialReferences ();
	}

	private void OnDisable() {
		UnsubscribeEvents ();
	}

	// Sets variables and subscribes to events.
	private void SetInitialReferences() {
		gameController = GameController.Instance;
		if (gameController == null)
			gameController = GameObject.FindObjectOfType<GameController> ();

		gameController.eventController.EventLoadGameData += LoadAllBuildingsT;
		gameController.eventController.EventStartBuildMode += StartBuildMode;
		gameController.eventController.EventEndBuildMode += PlaceBuilding;
		gameController.eventController.EventBuildMode += OnBuildMode;
	}

	// Unsubscribes events that were subscribed before.
	private void UnsubscribeEvents() {
		gameController.eventController.EventLoadGameData -= LoadAllBuildingsT;
		gameController.eventController.EventStartBuildMode -= StartBuildMode;
		gameController.eventController.EventEndBuildMode -= PlaceBuilding;
		gameController.eventController.EventBuildMode -= OnBuildMode;
	}


	private void Update() {
		if (gameController.stateController.gameState != State.LOADING || gameController.stateController.gameState != State.MENU) {
			currentTile = GameController.Instance.mapController.CheckForTile ();
		}

		// TODO: All of this works, but needs better optimization.
		if (gameController.stateController.gameState == State.BUILD) {
			if (currentTile != null) {
				if (Input.GetMouseButtonDown (0) && !isDragging) {
					RaycastHit hit;
					Ray newBuildingRay = Camera.main.ScreenPointToRay (Input.mousePosition);

					if (Physics.Raycast (newBuildingRay, out hit, 1000f, raycastMask) && hit.collider.tag == "PrePlacement") {
						isDragging = true;

						Debug.Log ("Started dragging.");
					}
				} 
					
				if (isDragging && gameController.mapController.CheckForObjectOnTile(previewObject.position) != currentTile) {
					SetOutlineColor (currentTile, true);

					gameController.eventController.CallBuildMode (currentTile.Position);
				}
			}

			if (Input.GetMouseButtonUp (0) && isDragging) {
				isDragging = false;
				tileToPlaceOn = currentTile;

				Debug.Log ("Stopped dragging.");
			}
		}
	}
		
	public void OnBuildMode(Vector3 position) {
		previewObject.position = position + Vector3.up * 0.25f;
	}

	// Loads all Building assets from Resource folder.
	// TODO: doesn't get called for some reason.
	public void LoadAllBuildingsT() {
		Debug.Log ("LoadAllBuildings has been initiated.");

		Building[] loadedBuildings = Resources.LoadAll("Buildings", typeof(Building)).Cast<Building>().ToArray();
		foreach (Building building in loadedBuildings) {
			buildings.Add (building);

			Debug.Log ("Building Has been loaded: " + building.Name);
		}
	}
		
	// Places a building on a specified tile.
	public void PlaceBuilding(Building toPlace) {
		if (toPlace != null && tileToPlaceOn != null) {
			Transform newBuilding = Instantiate (toPlace.Prefab).transform;
			newBuilding.position = tileToPlaceOn.Position + Vector3.up * 0.25f;

			tileToPlaceOn.Building = toPlace;
			if (previewObject != null) {
				Destroy(previewObject.Find("FloatingObject").GetChild(0).gameObject);

				previewObject.gameObject.SetActive (false);
			}
			buildingToPlace = null;

			gameController.eventController.CallChangeState ("GAME");
		}
	}

	public void PlaceBuildingOnTile(Building toPlace, Tile tile) {
		if (toPlace != null) {
			Transform newBuilding = Instantiate (toPlace.Prefab).transform;
            newBuilding.position = tile.Position + Vector3.up * 0.25f;

            tile.Building = toPlace;
        }
	}

	public Building FindBuildingByName(string name) {
		for (int a = 0; a < buildings.Count; a++) {
			if (buildings[a].Name == name) {
				return buildings [a];
			}
		}
		return null;
	}

	public void StartPlacementAnimation() {
		Animator buildingUIAnim = previewObject.GetComponent<Animator> ();
		buildingUIAnim.SetTrigger ("Place");

		SetOutlineColor (tileToPlaceOn, false);
		StartCoroutine (CheckAnimationState (buildingUIAnim));
	}

	IEnumerator CheckAnimationState(Animator anim) {
		yield return new WaitForSeconds (anim.GetCurrentAnimatorStateInfo(0).length / 3.5f);

		gameController.eventController.CallEndBuildMode (buildingToPlace);
	}

	// Better optimisation with preview?
	public void StartBuildMode(Building building) {
		if (buildingToPlace == null) {
			gameController.eventController.CallChangeState ("BUILD");

			Tile newEmptyTile = GameController.Instance.mapController.GetEmptyTile ();

			buildingToPlace = building;
			if (previewObject == null) {
				previewObject = Instantiate (previewObjectPrefab).transform;
				previewObject.position = newEmptyTile.Position + Vector3.up * 0.25f;
			}
			previewObject.gameObject.SetActive (true);
			SetOutlineColor (newEmptyTile, true);

            // This part might need some changes.
			currentBuildingPreview = Instantiate (buildingToPlace.Prefab.transform.Find ("Graphics")).transform;
			currentBuildingPreview.SetParent (previewObject.Find ("FloatingObject"));
            currentBuildingPreview.localPosition = Vector3.zero;
            currentBuildingPreview.localScale = Vector3.one;

			tileToPlaceOn = newEmptyTile;
			gameController.eventController.CallBuildMode (tileToPlaceOn.Position);
		}
	}

	private void SetOutlineColor(Tile tile, bool active) {
		Image previewImage = previewObject.Find ("OutlineUI").GetComponentInChildren<Image> ();

		if (active) {
			previewObject.Find ("OutlineUI").gameObject.SetActive(true);

			if (tile.Building == null) {
				previewImage.sprite = positiveOutline;
			} else {
				previewImage.sprite = negativeOutline;
			}
		} else {
			previewObject.Find ("OutlineUI").gameObject.SetActive(false);
		}
	}
		
}
