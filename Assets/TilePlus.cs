using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "New Tile Plus", menuName = "Tiles/Tile Plus")]
public class TilePlus : Tile
{
    public Sprite aliveSprite;
    public Sprite deadSprite;
    public bool alive;
    public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData) {
        base.GetTileData(location, tileMap, ref tileData);
        if (alive) {
            tileData.sprite = aliveSprite;
        } else {
            tileData.sprite = deadSprite;
        }
    }
}
