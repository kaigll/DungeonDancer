using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    public Vector2Int roomCoords;
    public Dictionary<string, Room> neighbors;

    public int roomNum;

    private string[,] population;

    private Dictionary<string, GameObject> nameToPrefab;

    private int xScale = 19;
    private int yScale = 9;

    public Room(int x, int y)
    {
        this.roomCoords = new Vector2Int(x, y);
        this.neighbors = new Dictionary<string, Room>();
        this.population = new string[xScale, yScale];
        for (int xIndex = 0; xIndex < xScale; xIndex++)
        {
            for (int yIndex = 0; yIndex < yScale; yIndex++)
            {
                this.population[xIndex, yIndex] = "";
            }
        }
        this.population[10, 5] = "Player";
        nameToPrefab = new Dictionary<string, GameObject>();
    }

    public Room(Vector2Int roomCoords)
    {
        this.roomCoords = roomCoords;
        this.neighbors = new Dictionary<string, Room>();
        this.population = new string[xScale, yScale];
        for (int xIndex = 0; xIndex < xScale; xIndex++)
        {
            for (int yIndex = 0; yIndex < yScale; yIndex++)
            {
                this.population[xIndex, yIndex] = "";
            }
        }
        this.population[10, 5] = "Player";
        nameToPrefab = new Dictionary<string, GameObject>();
    }

    public void PopulateObstacles(int numOfObstacles, Vector2Int[] possibleSizes)
    {
        for (int obstacleIndex = 0; obstacleIndex < numOfObstacles; obstacleIndex+=1)
        {
            int sizeIndex = Random.Range(0, possibleSizes.Length);
            Vector2Int regionSize = possibleSizes[sizeIndex];
            List<Vector2Int> region = FindFreeRegion(regionSize);
            foreach (Vector2Int coords in region)
            {
                this.population[coords.x, coords.y] = "Obstacle";
            }
        }
    }

    public void PopulatePrefabs (int numOfPrefabs, GameObject[] possiblePrefabs)
    {
        for (int prefabIndex = 0; prefabIndex < numOfPrefabs; prefabIndex++)
        {
            int choiceIndex = Random.Range(0, possiblePrefabs.Length);
            GameObject prefab = possiblePrefabs[choiceIndex];
            List<Vector2Int> region = FindFreeRegion(new Vector2Int(1, 1));

            this.population[region[0].x, region[0].y] = prefab.name;
            this.nameToPrefab[prefab.name] = prefab;
        }
    }
    public void PopulatePrefabs(GameObject prefab)
    {
        List<Vector2Int> region = FindFreeRegion(new Vector2Int(1, 1));

        this.population[region[0].x, region[0].y] = prefab.name;
        this.nameToPrefab[prefab.name] = prefab;
    }

    private List<Vector2Int> FindFreeRegion(Vector2Int sizeInTiles)
    {
        List<Vector2Int> region = new List<Vector2Int>();
        do
        {
            region.Clear();
            Vector2Int centerTile = new Vector2Int(Random.Range(2, xScale - 3), Random.Range(2, yScale - 3));
            region.Add(centerTile);
            int initialXCoordinate = (centerTile.x - (int)Mathf.Floor(sizeInTiles.x / 2));
            int initialYCoordinate = (centerTile.y - (int)Mathf.Floor(sizeInTiles.y / 2));
            for (int xCoordinate = initialXCoordinate; xCoordinate < initialXCoordinate + sizeInTiles.x; xCoordinate += 1)
            {
                for (int yCoordinate = initialYCoordinate; yCoordinate < initialYCoordinate + sizeInTiles.y; yCoordinate += 1)
                {
                    region.Add(new Vector2Int(xCoordinate, yCoordinate));
                }
            }
        } while (!IsFree(region));
        return region;
    }

    private bool IsFree(List<Vector2Int> region)
    {
        foreach (Vector2Int tile in region)
        {
            if (this.population[tile.x, tile.y] != "") return false;
        }
        return true;
    }

    public void AddPopulationToTilemap(Tilemap tilemap, TileBase obstacleTile)
    {
        // This is where the instantiation of obstacles and enemies occurs.
        for (int x = 0; x < xScale; x++)
        {
            for (int y = 0; y < yScale; y++)
            {
                if (this.population[x, y] == "Obstacle")
                {
                    tilemap.SetTile(new Vector3Int(x - 10, y - 5, 0), obstacleTile);
                } else if (this.population [x,y] != "" && this.population [x,y] != "Player")
                {
                    GameObject prefab = Object.Instantiate(this.nameToPrefab[this.population[x, y]]);
                    if (prefab.GetComponent<Enemy_Main>() != null) prefab.GetComponent<Enemy_Main>().region = new Vector2Int(x, y);
                    if (prefab.transform != null) prefab.transform.position = new Vector2(x - 9 + 0.5f, y - 5 + 0.5f);
                }
            }
        }
    }

    public void Depopulate(Vector2Int region)
    {
        population[region.x, region.y] = "";
    }

    public List<Vector2Int> NeigborCoords()
    {
        List<Vector2Int> neighborCoordinates = new List<Vector2Int>();
        neighborCoordinates.Add(new Vector2Int(this.roomCoords.x, this.roomCoords.y + 1));
        neighborCoordinates.Add(new Vector2Int(this.roomCoords.x + 1, this.roomCoords.y));
        neighborCoordinates.Add(new Vector2Int(this.roomCoords.x, this.roomCoords.y - 1));
        neighborCoordinates.Add(new Vector2Int(this.roomCoords.x - 1, this.roomCoords.y));

        return neighborCoordinates;
    }

    public void Connect(Room neighbor)
    {
        string direction = "";
        if (neighbor.roomCoords.y > this.roomCoords.y) direction = "N";
        if (neighbor.roomCoords.x > this.roomCoords.x) direction = "E";
        if (neighbor.roomCoords.y < this.roomCoords.y) direction = "S";
        if (neighbor.roomCoords.x < this.roomCoords.x) direction = "W";

        this.neighbors.Add(direction, neighbor);
    }

    public string PrefabName()
    {
        string name = "Room_";
        foreach (KeyValuePair<string,Room> neighborPair in neighbors)
        {
            name += neighborPair.Key;
        }
        return name;
    }

    public Room Neighbor(string dir)
    {
        return this.neighbors[dir];
    }
}