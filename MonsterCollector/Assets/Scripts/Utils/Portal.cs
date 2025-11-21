using UnityEngine;

public class Portal
{
    private string mapName;
    public void SetMapName(string mapName) { this.mapName = mapName; }
    public string GetMapName() { return mapName; }

    private Vector2Int spawnCoords;
    public void SetSpawnCoords(Vector2Int spawnCoords) { this.spawnCoords = spawnCoords; }
    public Vector2Int GetSpawnCoords() { return spawnCoords; }
}