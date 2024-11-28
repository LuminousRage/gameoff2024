using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnPoint : MonoBehaviour
{
    public Tilemap tilemap1;
    public Tilemap tilemap2;
    
    public Tilemap tilemap3;
    public TilePlus tile;

    public void changeSpawn(bool alive, int avatar) {
        Debug.Log($"changeSpawn {alive} for avatar {avatar}");
        tile.alive = alive;
        switch (avatar){
            case 1:
                tilemap1.RefreshAllTiles();
                break;
            case 2:
                tilemap2.RefreshAllTiles();
                break;
            case 3:
                tilemap3.RefreshAllTiles();
                break;
        }
        tile.alive = false;
    }
}
