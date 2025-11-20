using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileType tileType = TileType.None;
    public void SetTileType(TileType tileType) { this.tileType = tileType; InitializeMesh(); }
    
    private Vector2Int coords;
    public void SetCoords(Vector2Int coords) { this.coords = coords; }
    public Vector2Int GetCoords() { return coords; }

    private Dictionary<Direction, GameObject> neighborTiles;
    public void AddNeighbor(Direction direction, GameObject tileObject)
    {
        if (neighborTiles == null) { neighborTiles = new Dictionary<Direction, GameObject>(); }
        neighborTiles.Add(direction, tileObject);
    }

    private void InitializeMesh()
    {
        transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/grass");
    }
}