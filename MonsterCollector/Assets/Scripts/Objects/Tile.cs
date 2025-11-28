using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TileType tileType = TileType.None;
    public void SetTileType(TileType tileType) { this.tileType = tileType; InitializeMesh(); }
    public TileType GetTileType() { return tileType; }
    
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
        if (MapType.Residential.Equals(MapManager.instance.GetMapType()) && !TileType.None.Equals(tileType))
        {
            transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/interior_floor");
        }
        else if (MapType.Overworld.Equals(MapManager.instance.GetMapType()) && !TileType.None.Equals(tileType))
        {
            transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/grass");
        }
    }

    public void GenerateStructure() {
        if (MapType.Residential.Equals(MapManager.instance.GetMapType()))
        {
            GenerateBuildingStructures();
        }
        else if (MapType.Overworld.Equals(MapManager.instance.GetMapType()))
        {
            GenerateOverworldStructures();
        }
    }

    private void GenerateBuildingStructures()
    {
        if (TileType.Interior_Wall.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            // sprite.sprite = Resources.Load<Sprite>("Textures/interior_wall-base-full");

            // GameObject newStructureTop = Instantiate(
            //     Resources.Load<GameObject>("Prefabs/StructurePrefab"),
            //     transform.position + new Vector3(0, 0, 1),
            //     Quaternion.identity
            // );
            // SpriteRenderer spriteTop = newStructureTop.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            // newStructureTop.transform.SetParent(transform);

            // spriteTop.sprite = Resources.Load<Sprite>("Textures/interior_wall-top-full");
            // spriteTop.sortingOrder = 2;

            Vector2Int leftCoord = new Vector2Int(coords.x - 1, coords.y);
            Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
            Vector2Int bottomCoord = new Vector2Int(coords.x, coords.y + 1);
            Vector2Int topCoord = new Vector2Int(coords.x, coords.y - 1);
            GameObject leftTileObject = MapManager.instance.GetTileAtCoords(leftCoord);
            GameObject rightTileObject = MapManager.instance.GetTileAtCoords(rightCoord);
            GameObject bottomTileObject = MapManager.instance.GetTileAtCoords(bottomCoord);
            GameObject topTileObject = MapManager.instance.GetTileAtCoords(topCoord);
            TileType leftType = leftTileObject == null ? TileType.None : leftTileObject.GetComponent<Tile>().GetTileType();
            TileType rightType = rightTileObject == null ? TileType.None : rightTileObject.GetComponent<Tile>().GetTileType();
            TileType bottomType = bottomTileObject == null ? TileType.None : bottomTileObject.GetComponent<Tile>().GetTileType();
            TileType topType = topTileObject == null ? TileType.None : topTileObject.GetComponent<Tile>().GetTileType();

            // |
            // |  Wall top
            if (TileType.Interior_Wall.Equals(bottomType))
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_wall-top-full");
            }
            else
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_wall-base-full");

                if (!TileType.None.Equals(topType))
                {
                    GameObject newStructureTop = Instantiate(
                        Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                        transform.position + new Vector3(0, 0, 1),
                        Quaternion.identity
                    );
                    SpriteRenderer spriteTop = newStructureTop.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
                    newStructureTop.transform.SetParent(transform);

                    spriteTop.sprite = Resources.Load<Sprite>("Textures/interior_wall-top-short");
                    spriteTop.sortingOrder = 2;
                }
            }

            // Add wall tops to the top of the map.
            if (TileType.None.Equals(topType))
            {
                GameObject newStructureTop = Instantiate(
                    Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                    transform.position + new Vector3(0, 0, 1),
                    Quaternion.identity
                );
                SpriteRenderer spriteTop = newStructureTop.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
                newStructureTop.transform.SetParent(transform);

                spriteTop.sprite = Resources.Load<Sprite>("Textures/interior_wall-top-full");
                spriteTop.sortingOrder = 2;
            }
        }
        else if (TileType.Interior_Table.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);
            
            Vector2Int leftCoord = new Vector2Int(coords.x - 1, coords.y);
            Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
            Vector2Int bottomCoord = new Vector2Int(coords.x, coords.y + 1);
            Vector2Int topCoord = new Vector2Int(coords.x, coords.y - 1);
            GameObject leftTileObject = MapManager.instance.GetTileAtCoords(leftCoord);
            GameObject rightTileObject = MapManager.instance.GetTileAtCoords(rightCoord);
            GameObject bottomTileObject = MapManager.instance.GetTileAtCoords(bottomCoord);
            GameObject topTileObject = MapManager.instance.GetTileAtCoords(topCoord);
            TileType leftType = leftTileObject == null ? TileType.None : leftTileObject.GetComponent<Tile>().GetTileType();
            TileType rightType = rightTileObject == null ? TileType.None : rightTileObject.GetComponent<Tile>().GetTileType();
            TileType bottomType = bottomTileObject == null ? TileType.None : bottomTileObject.GetComponent<Tile>().GetTileType();
            TileType topType = topTileObject == null ? TileType.None : topTileObject.GetComponent<Tile>().GetTileType();
            
            //  *
            // *T*
            //  *  Solo table
            if (!TileType.Interior_Table.Equals(topType)
                && !TileType.Interior_Table.Equals(leftType)
                && !TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table");
            }
            //  *
            // *TT
            //  T  Deep top corner
            else if (!TileType.Interior_Table.Equals(topType)
                && !TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-corner-rear");
            }
            //  *
            // TT*
            //  T  Deep top corner flipped
            else if (!TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && !TileType.Interior_Table.Equals(rightType)
                && TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-corner-rear");
                sprite.flipX = true;
            }
            //  T
            // *TT
            //  *  Deep bottom corner
            else if (TileType.Interior_Table.Equals(topType)
                && !TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-corner-front");
            }
            //  T
            // TT*
            //  *  Deep bottom corner flipped
            else if (TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && !TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-corner-front");
                sprite.flipX = true;
            }
            //  *
            // TTT
            //  T  Deep rear center
            else if (!TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-center-rear");
            }
            //  T
            // *TT
            //  T  Deep side center
            else if (TileType.Interior_Table.Equals(topType)
                && !TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-center-rear");
                newStructure.transform.Find("Mesh").localRotation = Quaternion.Euler(newStructure.transform.Find("Mesh").localEulerAngles + new Vector3(0, -90, 0));
            }
            //  T
            // TT*
            //  T  Deed side center flipped
            else if (TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && !TileType.Interior_Table.Equals(rightType)
                && TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-center-rear");
                newStructure.transform.Find("Mesh").localRotation = Quaternion.Euler(newStructure.transform.Find("Mesh").localEulerAngles + new Vector3(0, 90, 0));
            }
            //  T
            // TTT
            //  *  Deep front center
            else if (TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-center-front");
            }
            //  T
            // TTT
            //  T  Deep center
            else if (TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-center");
            }
            //  *
            // *TT
            //  *  Narrow
            else if (!TileType.Interior_Table.Equals(topType)
                && !TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-corner-front");
            }
            //  *
            // TT*
            //  *  Narrow flipped
            else if (!TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && !TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-corner-front");
                sprite.flipX = true;
            }
            //  *
            // TTT
            //  *  Narrow center
            else if (!TileType.Interior_Table.Equals(topType)
                && TileType.Interior_Table.Equals(leftType)
                && TileType.Interior_Table.Equals(rightType)
                && !TileType.Interior_Table.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_table-deep-center-front");
            }
        }
        else if (TileType.Interior_Stool.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            sprite.sprite = Resources.Load<Sprite>("Textures/interior_stool");
        }
        else if (TileType.Interior_Dresser.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            sprite.sprite = Resources.Load<Sprite>("Textures/interior_dresser-bottom");

            GameObject newStructureTop = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position + new Vector3(0, 0, 1),
                Quaternion.identity
            );
            SpriteRenderer spriteTop = newStructureTop.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructureTop.transform.SetParent(transform);

            spriteTop.sprite = Resources.Load<Sprite>("Textures/interior_dresser-top");
            spriteTop.sortingOrder = 2;
        }
        else if (TileType.Interior_Doormat.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            Vector2Int leftCoord = new Vector2Int(coords.x - 1, coords.y);
            Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
            GameObject leftTileObject = MapManager.instance.GetTileAtCoords(leftCoord);
            GameObject rightTileObject = MapManager.instance.GetTileAtCoords(rightCoord);
            TileType leftType = leftTileObject == null ? TileType.None : leftTileObject.GetComponent<Tile>().GetTileType();
            TileType rightType = rightTileObject == null ? TileType.None : rightTileObject.GetComponent<Tile>().GetTileType();

            // *== edge
            if (!TileType.Interior_Doormat.Equals(leftType)
                && TileType.Interior_Doormat.Equals(rightType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_doormat-edge");
            }
            // ==* edge flipped
            else if (TileType.Interior_Doormat.Equals(leftType)
                && !TileType.Interior_Doormat.Equals(rightType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_doormat-edge");
                sprite.flipX = true;
            }
            // === middle
            else if (TileType.Interior_Doormat.Equals(leftType)
                && TileType.Interior_Doormat.Equals(rightType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_doormat-center");
            }
        }
        else if (TileType.Interior_Door.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            sprite.sprite = Resources.Load<Sprite>("Textures/interior_door");
        }
        else if (TileType.Interior_Bed.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            Vector2Int bottomCoord = new Vector2Int(coords.x, coords.y + 1);
            Vector2Int topCoord = new Vector2Int(coords.x, coords.y - 1);
            GameObject bottomTileObject = MapManager.instance.GetTileAtCoords(bottomCoord);
            GameObject topTileObject = MapManager.instance.GetTileAtCoords(topCoord);
            TileType bottomType = bottomTileObject == null ? TileType.None : bottomTileObject.GetComponent<Tile>().GetTileType();
            TileType topType = topTileObject == null ? TileType.None : topTileObject.GetComponent<Tile>().GetTileType();

            // B
            // B  
            // * bed bottom
            if (TileType.Interior_Bed.Equals(topType)
                && !TileType.Interior_Bed.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_bed-bottom");
            }
            // *
            // B
            // B bed top
            else if (!TileType.Interior_Bed.Equals(topType)
                && TileType.Interior_Bed.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/interior_bed-top");
            }
        }
        else if (TileType.Interior_Cabinet.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            sprite.sprite = Resources.Load<Sprite>("Textures/interior_cabinet-base");

            GameObject newStructureTop = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position + new Vector3(0, 0, 1),
                Quaternion.identity
            );
            SpriteRenderer spriteTop = newStructureTop.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructureTop.transform.SetParent(transform);

            spriteTop.sprite = Resources.Load<Sprite>("Textures/interior_cabinet-top");
            spriteTop.sortingOrder = 2;
        }
        else if (TileType.Interior_Sink.Equals(tileType))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            sprite.sprite = Resources.Load<Sprite>("Textures/interior_sink");

            GameObject newStructureTop = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position + new Vector3(0, 0, 1),
                Quaternion.identity
            );
            SpriteRenderer spriteTop = newStructureTop.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructureTop.transform.SetParent(transform);

            spriteTop.sprite = Resources.Load<Sprite>("Textures/interior_cabinet-top");
            spriteTop.sortingOrder = 2;
        }
    }

    private void GenerateOverworldStructures()
    {
        if (tileType.Equals(TileType.Building_Roof))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            Vector2Int leftCoord = new Vector2Int(coords.x - 1, coords.y);
            Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
            Vector2Int bottomCoord = new Vector2Int(coords.x, coords.y + 1);
            Vector2Int topCoord = new Vector2Int(coords.x, coords.y - 1);
            GameObject leftTileObject = MapManager.instance.GetTileAtCoords(leftCoord);
            GameObject rightTileObject = MapManager.instance.GetTileAtCoords(rightCoord);
            GameObject bottomTileObject = MapManager.instance.GetTileAtCoords(bottomCoord);
            GameObject topTileObject = MapManager.instance.GetTileAtCoords(topCoord);
            TileType leftType = leftTileObject == null ? TileType.None : leftTileObject.GetComponent<Tile>().GetTileType();
            TileType rightType = rightTileObject == null ? TileType.None : rightTileObject.GetComponent<Tile>().GetTileType();
            TileType bottomType = bottomTileObject == null ? TileType.None : bottomTileObject.GetComponent<Tile>().GetTileType();
            TileType topType = topTileObject == null ? TileType.None : topTileObject.GetComponent<Tile>().GetTileType();

            //  *
            // *--
            //  *  Roof corner shallow
            if (!TileType.Building_Roof.Equals(topType)
                && !TileType.Building_Roof.Equals(leftType)
                && TileType.Building_Roof.Equals(rightType)
                && !TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-corner");
            }
            //  *
            // ---
            //  *  Roof center shallow
            else if (!TileType.Building_Roof.Equals(topType)
                && TileType.Building_Roof.Equals(leftType)
                && TileType.Building_Roof.Equals(rightType)
                && !TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-center");
            }
            //  *
            // --*
            //  *  Roof corner shallow flipped
            else if (!TileType.Building_Roof.Equals(topType)
                && TileType.Building_Roof.Equals(leftType)
                && !TileType.Building_Roof.Equals(rightType)
                && !TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-corner");
                sprite.flipX = true;
            }
            //  *
            // *--
            //  -  Roof deep corner rear
            else if (!TileType.Building_Roof.Equals(topType)
                && !TileType.Building_Roof.Equals(leftType)
                && TileType.Building_Roof.Equals(rightType)
                && TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-deep-corner-rear");
            }
            //  *
            // ---
            //  -  Roof deep center rear
            else if (!TileType.Building_Roof.Equals(topType)
                && TileType.Building_Roof.Equals(leftType)
                && TileType.Building_Roof.Equals(rightType)
                && TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-deep-center-rear");
            }
            //  *
            // --*
            //  -  Roof deep corner rear flipped
            else if (!TileType.Building_Roof.Equals(topType)
                && TileType.Building_Roof.Equals(leftType)
                && !TileType.Building_Roof.Equals(rightType)
                && TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-deep-corner-rear");
                sprite.flipX = true;
            }
            //  -
            // *--
            //  *  Roof deep corner front
            else if (TileType.Building_Roof.Equals(topType)
                && !TileType.Building_Roof.Equals(leftType)
                && TileType.Building_Roof.Equals(rightType)
                && !TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-deep-corner-front");
            }
            //  -
            // ---
            //  *  Roof deep center front
            else if (TileType.Building_Roof.Equals(topType)
                && TileType.Building_Roof.Equals(leftType)
                && TileType.Building_Roof.Equals(rightType)
                && !TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-deep-center-front");
            }
            //  -
            // --*
            //  *  Roof deep corner front flipped
            else if (TileType.Building_Roof.Equals(topType)
                && TileType.Building_Roof.Equals(leftType)
                && !TileType.Building_Roof.Equals(rightType)
                && !TileType.Building_Roof.Equals(bottomType)
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_roof-deep-corner-front");
                sprite.flipX = true;
            }
        }
        else if (tileType.Equals(TileType.Building_Wall))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            Vector2Int leftCoord = new Vector2Int(coords.x - 1, coords.y);
            Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
            Vector2Int bottomCoord = new Vector2Int(coords.x, coords.y + 1);
            GameObject leftTileObject = MapManager.instance.GetTileAtCoords(leftCoord);
            GameObject rightTileObject = MapManager.instance.GetTileAtCoords(rightCoord);
            GameObject bottomTileObject = MapManager.instance.GetTileAtCoords(bottomCoord);
            TileType leftType = leftTileObject == null ? TileType.None : leftTileObject.GetComponent<Tile>().GetTileType();
            TileType rightType = rightTileObject == null ? TileType.None : rightTileObject.GetComponent<Tile>().GetTileType();
            TileType bottomType = bottomTileObject == null ? TileType.None : bottomTileObject.GetComponent<Tile>().GetTileType();

            // *|B
            //  *  Corner
            if (!(TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && !(TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_ground-edge");
            }
            // B|B
            //  *  Bottom edge
            else if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && !(TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_ground-center");
            }
            // B|*
            //  *  Corner
            else if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && !(TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && !(TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_ground-edge");
                sprite.flipX = true;
            }
            // *|B
            //  B  Edge
            else if (!(TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && (TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_wall-edge");
            }
            // B|B
            //  B  Center
            else if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && (TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_wall-center");
            }
            // B|*
            //  B  Edge
            else if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && !(TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && (TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_wall-edge");
                sprite.flipX = true;
            }
        }
        else if (tileType.Equals(TileType.Building_Window))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);

            Vector2Int leftCoord = new Vector2Int(coords.x - 1, coords.y);
            Vector2Int rightCoord = new Vector2Int(coords.x + 1, coords.y);
            Vector2Int bottomCoord = new Vector2Int(coords.x, coords.y + 1);
            GameObject leftTileObject = MapManager.instance.GetTileAtCoords(leftCoord);
            GameObject rightTileObject = MapManager.instance.GetTileAtCoords(rightCoord);
            GameObject bottomTileObject = MapManager.instance.GetTileAtCoords(bottomCoord);
            TileType leftType = leftTileObject == null ? TileType.None : leftTileObject.GetComponent<Tile>().GetTileType();
            TileType rightType = rightTileObject == null ? TileType.None : rightTileObject.GetComponent<Tile>().GetTileType();
            TileType bottomType = bottomTileObject == null ? TileType.None : bottomTileObject.GetComponent<Tile>().GetTileType();

            // *|B
            //  *  Corner
            if (!(TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && !(TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_window-ground-corner");
            }

            // B|B
            //  *  Bottom edge
            if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && !(TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_window-ground-center");
            }

            // B|*
            //  *  Corner
            if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && !(TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && !(TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_window-ground-corner");
                sprite.flipX = true;
            }

            // *|B
            //  B  Edge
            if (!(TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && (TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_window-edge");
            }

            // B|B
            //  B  Center
            if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && (TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && (TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_window-center");
            }

            // B|*
            //  B  Edge
            if ((TileType.Building_Wall.Equals(leftType) || TileType.Building_Door.Equals(leftType) || TileType.Building_Window.Equals(leftType))
                && !(TileType.Building_Wall.Equals(rightType) || TileType.Building_Door.Equals(rightType) || TileType.Building_Window.Equals(rightType))
                && (TileType.Building_Wall.Equals(bottomType) || TileType.Building_Door.Equals(bottomType) || TileType.Building_Window.Equals(bottomType))
            )
            {
                sprite.sprite = Resources.Load<Sprite>("Textures/building_window-edge");
                sprite.flipX = true;
            }
        }
        else if (tileType.Equals(TileType.Building_Door))
        {
            GameObject newStructure = Instantiate(
                Resources.Load<GameObject>("Prefabs/StructurePrefab"),
                transform.position,
                Quaternion.identity
            );
            SpriteRenderer sprite = newStructure.transform.Find("Mesh").Find("Sprite").gameObject.GetComponent<SpriteRenderer>();
            newStructure.transform.SetParent(transform);
            sprite.sprite = Resources.Load<Sprite>("Textures/building_door");
        }
    }
}