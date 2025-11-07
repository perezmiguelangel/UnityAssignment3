using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector2 playerPosition;
    public Tilemap skyTileMap;
    void Update()
    {
        //Camera's Position is updated with players position every frame
        playerPosition = player.transform.position;
        Follow();
    }

    void Follow()
    {
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 2, -10);
        skyTileMap.transform.position = new Vector3(playerPosition.x, 0, 4);
    }
}
