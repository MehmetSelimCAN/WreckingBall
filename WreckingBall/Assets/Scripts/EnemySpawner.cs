using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner Instance { private set; get; }

    private float smallBugSpawnTimer;
    private float smallBugSpawnTimerMax = 3f;
    private float bigBugSpawnTimer;
    private float bigBugSpawnTimerMax = 8f;
    private BugTypeListSO bugTypeList;

    private Tilemap tilemap;
    private List<Vector3> availablePlaces;
    private List<int> randomSmallBugNumbers;

    private void Awake() {
        Instance = this;
        bugTypeList = Resources.Load<BugTypeListSO>("BugTypeListSO");
        randomSmallBugNumbers = new List<int>(){0, 1, 2, 3};
    }

    private void Start() {
        tilemap = GameObject.Find("border").GetComponent<Tilemap>();
        availablePlaces = new List<Vector3>();

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++) {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++) {
                Vector3Int localPlace = new Vector3Int(n, p, (int)tilemap.transform.position.y);
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (!tilemap.HasTile(localPlace)) {
                    availablePlaces.Add(place);
                }
            }
        }

        for (int i = 0; i < 8; i++) {
            SpawnSmallBug();
        }
    }

    private void Update() {
        smallBugSpawnTimer -= Time.deltaTime;
        bigBugSpawnTimer -= Time.deltaTime;

        if (smallBugSpawnTimer < 0f) {
            SpawnSmallBug();
            smallBugSpawnTimer = smallBugSpawnTimerMax;
        }

        if (bigBugSpawnTimer < 0f) {
            SpawnBigBug();
            bigBugSpawnTimer = bigBugSpawnTimerMax;
        }
    }

    public void SpawnSmallBug() {
        if (randomSmallBugNumbers.Count == 0) {
            randomSmallBugNumbers = new List<int>() { 0, 1, 2, 3 };
        }

        int randomPlaceNumber = Random.Range(0, availablePlaces.Count);
        int randomBugType = Random.Range(0,randomSmallBugNumbers.Count);

        Instantiate(bugTypeList.list[randomSmallBugNumbers[randomBugType]].prefab, availablePlaces[randomPlaceNumber], Quaternion.identity);

        randomSmallBugNumbers.RemoveAt(randomBugType);
    }

    private void SpawnBigBug() {
        int randomPlaceNumber = Random.Range(0, availablePlaces.Count);
        int randomBugType = Random.Range(4, 6);

        Instantiate(bugTypeList.list[randomBugType].prefab, availablePlaces[randomPlaceNumber], Quaternion.identity);
    }
}
