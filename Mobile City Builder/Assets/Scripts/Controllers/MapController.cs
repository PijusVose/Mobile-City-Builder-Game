using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapController : MonoBehaviour {

	public GameObject tilePrefab;
	public Transform tilePlaceHolder;
	public GameController gameController;
	public LayerMask raycastMask;

	public List<Tile> tiles;
	public TileMapData tileMapData;

	private const int tileMapSizeX = 10;
	private const int tileMapSizeZ = 10;
	private const string TILE_MAP_PARENT = "TileMap";
	private const string DATA_PATH = @"Assets/Data/";
	private const string FILE_NAME = "TileData.asset";

	// Use this for initialization
	private void Start () {
		SetInitialReferences ();
	}

	private void SetInitialReferences() {
		gameController = GameController.Instance;
		if (gameController == null)
			gameController = GameObject.FindObjectOfType<GameController> ();

		gameController.eventController.EventLoadTileMap += LoadTileMap;
	}

	private void OnDisable() {
		gameController.eventController.EventLoadTileMap -= LoadTileMap;
	}

	// Locates and loads the saved TileMap;
	private void LoadTileMap() {
		tileMapData = (TileMapData)AssetDatabase.LoadAssetAtPath (DATA_PATH + FILE_NAME, typeof(TileMapData));

		// If the ScriptableObject is not found create a new one.
		if (tileMapData == null) {
			tileMapData = ScriptableObject.CreateInstance<TileMapData> ();
			AssetDatabase.CreateAsset (tileMapData, DATA_PATH + FILE_NAME);
			AssetDatabase.SaveAssets ();
		} else {
			tiles = tileMapData.data;
		}
		GameController.Instance.isTileMapLoaded = true;

		GenerateTileMap ();
	}

    // Temporary maybe? Might add an event which is called from GameController.
    private void OnApplicationQuit() {
        SaveTileMap();
    }

    public void ResetTileSave() {
        tileMapData = (TileMapData)AssetDatabase.LoadAssetAtPath(DATA_PATH + FILE_NAME, typeof(TileMapData));
        tileMapData.data.Clear();
        tileMapData.data = new List<Tile>();
    }

    private void SaveTileMap() {
		tileMapData = (TileMapData)AssetDatabase.LoadAssetAtPath (DATA_PATH + FILE_NAME, typeof(TileMapData));

		// If the ScriptableObject is not found create a new one.
		if (tileMapData == null) {
			tileMapData = ScriptableObject.CreateInstance<TileMapData> ();
			AssetDatabase.CreateAsset (tileMapData, DATA_PATH + FILE_NAME);
			AssetDatabase.SaveAssets ();
		} 

		tileMapData.data = tiles;
	}

    private void ResetTileObjects() {
        // Might need to change if statement, when the size will depend on Graphics.
        if (tilePlaceHolder.childCount > 0 && tilePlaceHolder.GetChild(0).transform.localScale.x != tilePrefab.transform.localScale.x)
        {
            int childCount = tilePlaceHolder.childCount;
                for (int i = childCount; i > 0; i--) {
                    #if UNITY_EDITOR
                        DestroyImmediate(tilePlaceHolder.GetChild(i).gameObject);
                    #else
                        Destroy(tilePlaceHolder.GetChild(i).gameObject);
                    #endif
                }
        }
    }

	// Generates a TileMap.
    // TODO: Check if tiles are same size in current TileMap, reset if not.
	public void GenerateTileMap() {
        


        // Need to make if statements, to check if old tilemap is different. Also, make sure data is reset when needed.
                for (int x = 0; x < tileMapSizeX; x++)
                {
                    for (int y = 0; y < tileMapSizeZ; y++)
                    {
                        Transform newTile = Instantiate(tilePrefab).transform;
                        newTile.parent = tilePlaceHolder;
                        newTile.name = "Tile_" + x + y;
                        newTile.localPosition = new Vector3(x * newTile.localScale.x, 0f, y * newTile.localScale.z);

                        Tile tileData = FindTileAtXY(newTile.name);
                        if (tileData == null)
                        {
                            tileData = FindTileAtPosition(newTile.localPosition);
                            if (tileData == null)
                            {
                                tileData = new Tile(newTile.name, newTile.localPosition);
                                tiles.Add(tileData);
                            }
                        }

                        // TODO: Spawn a Building on a Tile. First finish up the Building Database.
                        if (tileData.Building != null)
                        {
                            gameController.buildController.PlaceBuildingOnTile(tileData.Building, tileData);
                        }
                    }
                }
	}

	// Gets an empty Tile, by Raycast, then by elimination and then by choosing the first Tile.
	public Tile GetEmptyTile() {
		Tile emptyTile = CheckForTileAtCenter ();
		if (emptyTile == null) {
			for (int t = 0; t < tiles.Count; t++) {
				if (tiles [t].Building == null) {
					emptyTile = tiles [t];
					break;
				}
			}

			if (emptyTile == null) {
				emptyTile = tiles [0];
			}

			Debug.Log ("Random Tile Choosen");
		}

		return emptyTile;
	}

	// TODO: Finish this up.
	public Tile CheckForTileAtCenter() {
		Tile newTile = null;

		RaycastHit hit;
		Ray newRay = Camera.main.ViewportPointToRay (new Vector3(0.5f, 0.5f, 0f));
		if (Physics.Raycast(newRay, out hit, 1000f, raycastMask)) {
			Vector3 tilePosition = hit.transform.position;

			newTile = FindTileAtPosition (tilePosition);
		}

		return newTile;
	}

	public Tile CheckForTile() {
		Tile newTile = null;

		RaycastHit hit;
		Ray newRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast(newRay, out hit, 1000f, raycastMask)) {
			Vector3 tilePosition = hit.transform.position;

			newTile = FindTileAtPosition (tilePosition);
		}

		return newTile;
	}

	public Tile CheckForObjectOnTile(Vector3 position) {
		for (int t = 0; t < tiles.Count; t++) {
			if (tiles [t].Position.x == position.x && tiles [t].Position.z == position.z) {
				return tiles [t];
			}
		}

		return null;
	}

	public Building CheckIfTileOccupied(Vector3 position) {
		Tile checkTile = FindTileAtPosition (position);

		if (checkTile != null) {
			if (checkTile.Building != null) {
				return checkTile.Building;
			}
		}

		return null;
	}

	private Tile FindTileAtPosition(Vector3 position) {
		for (int t = 0; t < tiles.Count; t++) {
			if (tiles[t].Position.x == position.x && tiles[t].Position.z == position.z) {
				return tiles [t];
			}
		}

		return null;
	}

    private Tile FindTileAtXY(string name)
    {
        for (int t = 0; t < tiles.Count; t++)
        {
            if (tiles[t].Name == name)
            {
                return tiles[t];
            }
        }

        return null;
    }

}
