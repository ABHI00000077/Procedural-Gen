using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public GameObject playerPrefab;

    public int dungeonWidth = 10;
    public int dungeonHeight = 10;
    public int roomSizeMin = 3;
    public int roomSizeMax = 6;

    private List<Rect> rooms = new List<Rect>();
    private GameObject player;

    void Start()
    {
        GenerateDungeon();
        SpawnPlayer();
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < 10; i++)
        {
            int width = Random.Range(roomSizeMin, roomSizeMax);
            int height = Random.Range(roomSizeMin, roomSizeMax);

            int x = Random.Range(0, dungeonWidth - width);
            int y = Random.Range(0, dungeonHeight - height);

            Rect newRoom = new Rect(x, y, width, height);

            bool overlaps = false;
            foreach (var room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
                CreateRoom(newRoom);

                if (rooms.Count > 1)
                {
                    Vector2 centerA = newRoom.center;
                    Vector2 centerB = rooms[rooms.Count - 2].center;

                    CreateCorridor(centerA, centerB);
                }
            }
        }
    }

    void CreateRoom(Rect room)
    {
        GameObject roomInstance = Instantiate(roomPrefab);
        float randomOffsetX = Random.Range(-1f, 1f);
        float randomOffsetZ = Random.Range(-1f, 1f);
        roomInstance.transform.position = new Vector3(room.x + room.width / 2 + randomOffsetX, 0, room.y + room.height / 2 + randomOffsetZ);

        float randomScaleX = Random.Range(0.8f, 1.2f);
        float randomScaleZ = Random.Range(0.8f, 1.2f);
        roomInstance.transform.localScale = new Vector3(room.width * randomScaleX, 1, room.height * randomScaleZ);
    }

    void CreateCorridor(Vector2 pointA, Vector2 pointB)
    {
        Vector2 current = pointA;
        while ((int)current.x != (int)pointB.x)
        {
            Vector2 direction = new Vector2(Mathf.Sign(pointB.x - current.x), 0);
            CreateCorridorSegment(current);
            current += direction;
        }
        while ((int)current.y != (int)pointB.y)
        {
            Vector2 direction = new Vector2(0, Mathf.Sign(pointB.y - current.y));
            CreateCorridorSegment(current);
            current += direction;
        }
    }

    void CreateCorridorSegment(Vector2 position)
    {
        GameObject corridorInstance = Instantiate(corridorPrefab);
        corridorInstance.transform.position = new Vector3(position.x, 0, position.y);
        corridorInstance.transform.localScale = new Vector3(1, 1, 1);
    }

    void SpawnPlayer()
    {
        if (rooms.Count > 0)
        {
            Rect firstRoom = rooms[0];
            Vector3 spawnPosition = new Vector3(firstRoom.center.x, 1, firstRoom.center.y);
            player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            Camera.main.GetComponent<CameraFollow>().target = player.transform;
        }
    }
}

