using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    void Awake() { instance = this; }

    private MapType mapType = MapType.None;
    public MapType GetMapType() { return mapType; }
    public void SetMapType(MapType mapType) { this.mapType = mapType; }

    private Dictionary<Vector2Int, GameObject> tileLibrary = new Dictionary<Vector2Int, GameObject>();
    public GameObject GetTileAtCoords(Vector2Int coords)
    {
        if (!tileLibrary.ContainsKey(coords)) { return null; }
        return tileLibrary[coords];
    }

    private Dictionary<Vector2Int, Portal> portalLibrary = new Dictionary<Vector2Int, Portal>();
    public Portal GetPortalAtCoords(Vector2Int coords)
    {
        if (!portalLibrary.ContainsKey(coords)) { return null; }
        return portalLibrary[coords];
    }

    void Start()
    {
        // LoadMap("Overworld_0", Vector2Int.zero);
        // LoadMap("Building_0", Vector2Int.zero);
        LoadMap("Building_1", Vector2Int.zero);
    }

    public void LoadMap(string mapName, Vector2Int playerLocation)
    {
        foreach (Transform child in GameObject.FindGameObjectWithTag("MapObjects").transform) { Destroy(child.gameObject); }
        tileLibrary.Clear();
        portalLibrary.Clear();
        
        TextAsset mapText = Resources.Load<TextAsset>("Maps/" + mapName);
        string[] mapLines = mapText.text.Split('\n');
        
        // Get map size.
        string mapSizeText = mapLines[0];
        string[] mapSizeSplit = mapSizeText.Split(',');
        Vector2Int size = new Vector2Int(int.Parse(mapSizeSplit[0]), int.Parse(mapSizeSplit[1]));

        // Parse map tags.
        Vector2Int spawn = Vector2Int.zero;

        string mapTagsText = mapSizeSplit[2];
        string[] mapTagsSplit = mapTagsText.Split(';');
        foreach (string mapTag in mapTagsSplit)
        {
            string[] tag = mapTag.Split('|');
            if ("spawn".Equals(tag[0]))
            {
                string[] spawnText = tag[1].Split('/');
                spawn = new Vector2Int(int.Parse(spawnText[0]), int.Parse(spawnText[1]));
            }
            else if ("door".Equals(tag[0]))
            {
                string[] doorCoordText = tag[1].Split('/');
                Vector2Int doorCoord = new Vector2Int(int.Parse(doorCoordText[0]), int.Parse(doorCoordText[1]));
                string doorLocation = tag[2];
                string[] doorSpawnCoordsText = tag[3].Split('/');
                Vector2Int doorSpawnCoords = new Vector2Int(int.Parse(doorSpawnCoordsText[0]), int.Parse(doorSpawnCoordsText[1]));
                Portal newPortal = new Portal();
                newPortal.SetMapName(doorLocation);
                newPortal.SetSpawnCoords(doorSpawnCoords);
                portalLibrary.Add(doorCoord, newPortal);
            }
            else if ("type".Equals(tag[0]))
            {
                MapType loadedMapType = (MapType)Enum.Parse(typeof(MapType), tag[1]);
                SetMapType(loadedMapType);
            }
        }

        // Create map.
        for (int xIndex = 0; xIndex < size.x; xIndex++)
        {
            for (int zIndex = 0; zIndex < size.y; zIndex++)
            {
                string[] mapLine = mapLines[zIndex + 1].Split(',');
                char mapChar = mapLine[xIndex][0];

                GameObject newTileObject = Instantiate(
                    Resources.Load<GameObject>("Prefabs/TilePrefab"),
                    new Vector3(xIndex, 0, zIndex * -1),
                    Quaternion.identity
                );
                newTileObject.transform.SetParent(GameObject.FindGameObjectWithTag("MapObjects").transform);
                newTileObject.name = "Tile_(" + xIndex + ", " + zIndex + ")";
                newTileObject.GetComponent<Tile>().SetCoords(new Vector2Int(xIndex, zIndex));

                TileType newType = TileType.None;
                if ('_'.Equals(mapChar)) // Empty tile
                {
                    if (MapType.Residential.Equals(GetMapType()))
                    {
                        newType = TileType.Interior_Floor;
                    }
                    else if (MapType.Overworld.Equals(GetMapType()))
                    {
                        newType = TileType.Grass;
                    }
                }
                else if ('-'.Equals(mapChar)) // Roof
                {
                    newType = TileType.Building_Roof;
                }
                else if ('|'.Equals(mapChar)) // Wall
                {
                    if (MapType.Residential.Equals(GetMapType()))
                    {
                        newType = TileType.Interior_Wall;
                    }
                    else if (MapType.Overworld.Equals(GetMapType()))
                    {
                        newType = TileType.Building_Wall;
                    }
                }
                else if ('O'.Equals(mapChar)) // Door
                {
                    if (MapType.Residential.Equals(GetMapType()))
                    {
                        newType = TileType.Interior_Door;
                    }
                    else if (MapType.Overworld.Equals(GetMapType()))
                    {
                        newType = TileType.Building_Door;
                    }
                }
                else if ('+'.Equals(mapChar)) // Window
                {
                    if (MapType.Residential.Equals(GetMapType()))
                    {
                        newType = TileType.Interior_Oven;
                    }
                    else if (MapType.Overworld.Equals(GetMapType()))
                    {
                        newType = TileType.Building_Window;
                    }
                }
                else if ('='.Equals(mapChar)) // Doormat
                {
                    newType = TileType.Interior_Doormat;
                }
                else if ('B'.Equals(mapChar)) // Bed
                {
                    newType = TileType.Interior_Bed;
                }
                else if ('D'.Equals(mapChar)) // Dresser
                {
                    newType = TileType.Interior_Dresser;
                }
                else if ('S'.Equals(mapChar)) // Stool
                {
                    newType = TileType.Interior_Stool;
                }
                else if ('T'.Equals(mapChar)) // Table
                {
                    newType = TileType.Interior_Table;
                }
                else if ('C'.Equals(mapChar)) // Cabinet
                {
                    newType = TileType.Interior_Cabinet;
                }
                else if ('U'.Equals(mapChar)) // Sink
                {
                    newType = TileType.Interior_Sink;
                }
                else if ('F'.Equals(mapChar)) // Fridge
                {
                    newType = TileType.Interior_Fridge;
                }
                newTileObject.GetComponent<Tile>().SetTileType(newType);

                // Link to existing tiles.
                Vector2Int checkTile = new Vector2Int(xIndex, zIndex - 1);
                if (tileLibrary.ContainsKey(checkTile))
                {
                    GameObject tileObject = tileLibrary[checkTile];
                    tileObject.GetComponent<Tile>().AddNeighbor(Direction.Down, newTileObject);
                    newTileObject.GetComponent<Tile>().AddNeighbor(Direction.Up, tileObject);
                }
                
                checkTile = new Vector2Int(xIndex - 1, zIndex);
                if (tileLibrary.ContainsKey(checkTile))
                {
                    GameObject tileObject = tileLibrary[checkTile];
                    tileObject.GetComponent<Tile>().AddNeighbor(Direction.Right, newTileObject);
                    newTileObject.GetComponent<Tile>().AddNeighbor(Direction.Left, tileObject);
                }

                // Add to tile library.
                tileLibrary.Add(new Vector2Int(xIndex, zIndex), newTileObject);
            }
        }

        // Generate structures.
        foreach (GameObject tile in tileLibrary.Values) {
            tile.GetComponent<Tile>().GenerateStructure();
        }

        // Spawn player.
        Vector2Int spawnLocation = Vector2Int.zero.Equals(playerLocation) ? spawn : playerLocation;
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject playerObject = Instantiate(
                Resources.Load<GameObject>("Prefabs/PlayerObject"),
                new Vector3(spawnLocation.x, 0, spawnLocation.y * -1),
                Quaternion.identity
            );
            playerObject.name = "PlayerObject";
            playerObject.GetComponent<PlayerMovementController>().SetCoords(spawnLocation);
        }
        else
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            playerObject.transform.position = new Vector3(spawnLocation.x, 0, spawnLocation.y * -1);
            playerObject.GetComponent<PlayerMovementController>().SetCoords(spawnLocation);
        }
    }
}