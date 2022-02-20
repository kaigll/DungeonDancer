using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private int numRooms;
    [SerializeField] private Grid gridObj;
    [SerializeField] private TileBase obstacleTile;
    [SerializeField] private int numberOfObstacles;
    private Vector2Int[] possibleObstacleSize;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private GameObject[] possibleEnemies;

    [SerializeField] private GameObject goalPrefab;


    private Room[,] rooms;

    private Room currentRoom;

    private Room finalRoom;

    public static WorldGeneration instance;

    private static Vector3 playerStartPos = new Vector3(0.5f, 0.5f, 0);


    private void Awake()
    {
        possibleObstacleSize = new Vector2Int[1];
        possibleObstacleSize[0] = new Vector2Int(1,1);

        // initialises the dungeon, caching the instance in order to ensure that the rooms already generated are reloaded properly
        if (instance == null)
        {
            DontDestroyOnLoadManager.DontDestroyOnLoad(this.gameObject);
            instance = this;
            this.currentRoom = GenerateDungeon();
        } else
        {
            string roomPrefabName = instance.currentRoom.PrefabName();
            GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
            roomObject.transform.SetParent(gridObj.transform);
            Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap>();
            instance.currentRoom.AddPopulationToTilemap(tilemap, instance.obstacleTile);
            SetPlayerPos();
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        string roomPrefabName = this.currentRoom.PrefabName();
        GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
        roomObject.transform.SetParent(gridObj.transform);
        Tilemap tilemap = roomObject.GetComponentInChildren<Tilemap>();
        this.currentRoom.AddPopulationToTilemap(tilemap, this.obstacleTile);
        SetPlayerPos();
    }

    private Room GenerateDungeon()
    {
        int gridSize = 3 * numRooms;

        rooms = new Room[gridSize, gridSize];

        Vector2Int initRoomCoord = new Vector2Int((gridSize / 2) - 1, (gridSize / 2) - 1);
        Queue<Room> roomsToCreate = new Queue<Room>();
        roomsToCreate.Enqueue(new Room(initRoomCoord.x, initRoomCoord.y));
        List<Room> createdRooms = new List<Room>();

        // iterates to add neighbours to each room in the list
        while(roomsToCreate.Count > 0 && createdRooms.Count < numRooms)
        {
            currentRoom = roomsToCreate.Dequeue();
            this.rooms[currentRoom.roomCoords.x, currentRoom.roomCoords.y] = currentRoom;
            createdRooms.Add(currentRoom);
            AddNeighbors(currentRoom, roomsToCreate);
        }

        int maximumDistanceToInitialRoom = 0;
        finalRoom = null;

        // Connects each room and populates them with obstacles and enemies
        foreach (Room room in createdRooms)
        {
            List<Vector2Int> neighborCoords = room.NeigborCoords();
            foreach (Vector2Int coords in neighborCoords)
            {
                Room neighbor = this.rooms[coords.x, coords.y];
                if (neighbor != null)
                {
                    room.Connect(neighbor);
                }
            }
            room.PopulateObstacles(this.numberOfObstacles, this.possibleObstacleSize);
            room.PopulatePrefabs(this.numberOfEnemies, this.possibleEnemies);

            int distanceToInitialRoom = Mathf.Abs(room.roomCoords.x - initRoomCoord.x) + Mathf.Abs(room.roomCoords.y - initRoomCoord.y);
            if (distanceToInitialRoom > maximumDistanceToInitialRoom)
            {
                maximumDistanceToInitialRoom = distanceToInitialRoom;
                finalRoom = room;
            }
        }

        finalRoom.PopulatePrefabs(goalPrefab);
        return this.rooms [initRoomCoord.x, initRoomCoord.y];
    }

    public void AddNeighbors(Room currentRoom, Queue<Room> roomsToCreate)
    {
        // randomly generates neighbours for each room, this is where the majority of the procedural nature of the dungeon comes from

        List<Vector2Int> neighborCoords = currentRoom.NeigborCoords();
        List<Vector2Int> avaliableNeighbors = new List<Vector2Int>();
        
        foreach (Vector2Int coord in neighborCoords)
        {
            if (this.rooms [coord.x, coord.y] == null)
            {
                avaliableNeighbors.Add(coord);
            }
        }

        int numberOfNeighbors = (int)Random.Range(1, avaliableNeighbors.Count);

        for (int neighborIndex = 0; neighborIndex < numberOfNeighbors; neighborIndex++)
        {
            float randNum = Random.value;
            float roomFrac = 1f / (float)avaliableNeighbors.Count;
            Vector2Int chosenNeighbor = new Vector2Int(0, 0);

            foreach (Vector2Int coordinate in avaliableNeighbors)
            {
                if (randNum < roomFrac)
                {
                    chosenNeighbor = coordinate;
                    break;
                } else {
                    roomFrac += 1f / (float)avaliableNeighbors.Count;
                }
            }
            roomsToCreate.Enqueue(new Room(chosenNeighbor));
            avaliableNeighbors.Remove(chosenNeighbor);
        }

    }

    private void PrintGrid()
    {
        // just to debug in console for room generation
        for (int rowIndex = 0; rowIndex < this.rooms.GetLength(1); rowIndex++)
        {
            string row = "";
            for (int columnIndex = 0; columnIndex < this.rooms.GetLength(0); columnIndex++)
            {
                if (this.rooms[columnIndex, rowIndex] == null) { 
                    row += "X";
                }
                else
                    row += "R";
            }
            Debug.Log(row);
        }
    }


    public void MoveToRoom(Room room)
    {
        this.currentRoom = room;
    }

    public Room CurrentRoom()
    {
        return this.currentRoom;
    }

    public void ResetDungeon()
    {
        this.currentRoom = GenerateDungeon();
    }

    public void SetPlayerPos()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 pos = player.GetComponent<PlayerMovement>().DoorPosition();
        player.transform.position = pos;
        GameObject playerMovePoint = GameObject.FindGameObjectWithTag("PlayerMovePoint");
        playerMovePoint.transform.position = pos;
    }
} 