using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    void Awake() { instance = this; }

    private Dictionary<Vector2Int, GameObject> tileLibrary = new Dictionary<Vector2Int, GameObject>();
    public GameObject GetTileAtCoords(Vector2Int coords)
    {
        if (!tileLibrary.ContainsKey(coords)) { return null; }
        return tileLibrary[coords];
    }

    void Start()
    {
        LoadMap("Overworld_0", Vector2Int.zero);
    }

    public void LoadMap(string mapName, Vector2Int playerLocation)
    {
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
                newTileObject.name = "Tile_(" + xIndex + ", " + zIndex + ")";
                newTileObject.GetComponent<Tile>().SetCoords(new Vector2Int(xIndex, zIndex));

                if ('_'.Equals(mapChar))
                {
                    newTileObject.GetComponent<Tile>().SetTileType(TileType.Grass);
                }

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

        // Spawn player.
        Vector2Int spawnLocation = Vector2Int.zero.Equals(playerLocation) ? spawn : playerLocation;
        GameObject playerObject = Instantiate(
            Resources.Load<GameObject>("Prefabs/PlayerObject"),
            new Vector3(spawnLocation.x, 0, spawnLocation.y),
            Quaternion.identity
        );
        playerObject.name = "PlayerObject";
        playerObject.GetComponent<PlayerMovementController>().SetCoords(spawnLocation);
    }
}