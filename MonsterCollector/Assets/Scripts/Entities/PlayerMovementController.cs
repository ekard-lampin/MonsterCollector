using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private Vector2Int coords;
    public void SetCoords(Vector2Int coords) { this.coords = coords; }

    [SerializeField]
    private bool moving = false;
    [SerializeField]
    private float moveTimer = 0;
    [SerializeField]
    private GameObject targetTileObject;

    void Update()
    {
        HandleMovement();
        HandleSpriteSortingOrder();
    }

    private void HandleMovement()
    {
        if (!moving)
        {
            if (Vector2Int.zero.Equals(InputManager.instance.GetMovementInput())) { return; }

            Vector2Int targetTile = coords;
            if (InputManager.instance.GetMovementInput().x < 0) { targetTile.x--; }
            else if (InputManager.instance.GetMovementInput().x > 0) { targetTile.x++; }
            else if (InputManager.instance.GetMovementInput().y < 0) { targetTile.y++; }
            else if (InputManager.instance.GetMovementInput().y > 0) { targetTile.y--; }
 
            if (MapManager.instance.GetTileAtCoords(targetTile) == null) { return; }
            if (!IsTileWalkable(MapManager.instance.GetTileAtCoords(targetTile).GetComponent<Tile>().GetTileType())) { return; }

            targetTileObject = MapManager.instance.GetTileAtCoords(targetTile);
            moveTimer = 0;
            moving = true;
        }
        else
        {
            moveTimer += Time.deltaTime;

            float moveRatio = Mathf.Clamp(moveTimer / GameManager.instance.GetPlayerMoveSpeed(), 0, 1);
            transform.position = Vector3.Lerp(MapManager.instance.GetTileAtCoords(coords).transform.position, targetTileObject.transform.position, moveRatio);
            
            if (moveTimer < GameManager.instance.GetPlayerMoveSpeed()) { return; }
            transform.position = targetTileObject.transform.position;
            coords = targetTileObject.GetComponent<Tile>().GetCoords();
            
            moving = false;
            moveTimer = 0;
            targetTileObject = null;

            // Check for door.
            if (MapManager.instance.GetPortalAtCoords(coords) == null) { return; }
            Portal portal = MapManager.instance.GetPortalAtCoords(coords);
            Debug.Log("Entered door to " + portal.GetMapName());
            MapManager.instance.LoadMap(portal.GetMapName(), portal.GetSpawnCoords());
        }
    }

    private bool IsTileWalkable(TileType tileType)
    {
        if (TileType.Grass.Equals(tileType)) { return true; }
        if (TileType.GrassOvergrown.Equals(tileType)) { return true; }
        if (TileType.Building_Door.Equals(tileType)) { return true; }
        if (TileType.Interior_Floor.Equals(tileType)) { return true; }
        if (TileType.Interior_Door.Equals(tileType)) { return true; }
        if (TileType.Interior_Stool.Equals(tileType)) { return true; }
        if (TileType.Interior_Doormat.Equals(tileType)) { return true; }
        if (TileType.Path.Equals(tileType)) { return true; }
        if (TileType.EncounterGrass.Equals(tileType)) { return true; }
        if (TileType.Industrial_Floor.Equals(tileType)) { return true; }

        return false;
    }

    private void HandleSpriteSortingOrder()
    {
        // transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt((coords.y + 0.5f) * 10);
        transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(Mathf.Abs(transform.position.z) * 10);
    }
}