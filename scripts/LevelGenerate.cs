using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
using UnityEditor.AI;

using System;

public class LevelGenerate : MonoBehaviour
{
    private Transform playerPos;
    public GameObject wall;
    public GameObject campFire;

    public int gridDimensionXZDensity;

    private GameObject roomHold;
    public thing[] rooms;

    public int startRoomsNum;

    public thing[] things;
    public thing[] enemys;

    HashSet<KeyValuePair<float,float>> locations = new HashSet<KeyValuePair<float, float>>();

    private float length;
    private HashSet<Vector3> prevRooms = new HashSet<Vector3>();
    // Start is called before the first frame update

    private void Awake()
    {
        createLevel();
    }
    void OnLevelWasLoaded()
    {
        startRoomsNum++;
        createLevel();
    }


    void createLevel()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Generator");

        if (objs.Length > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
        Debug.Log("Destroyed");

        roomHold = GameObject.Find("RoomHolding");
        foreach (Transform child in roomHold.transform)
        {
            Destroy(child.gameObject);
        }

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        length = rooms[0].objectToPlace.transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.size.x;

        for (int x = 0; x < startRoomsNum; x++)
        {
            if (x == 0)
            {
                Debug.Log("true");
                placeSquares(new Vector3(0, 0, x * length * 2), true);
                //FIRST ROOMS
            }
            else if (x == startRoomsNum - 1)
            {
                Debug.Log("false");
                //LAST ROOMS
                placeSquares(new Vector3(0, 0, x * length * 2), false);
            }
            else
            {
                Debug.Log("null");
                //MIDDLE ROOMS
                placeSquares(new Vector3(0, 0, x * length * 2), null);

            }
        }


        //BAKE NAVMESH AFTER MAP COMPLETE
        /*Bounds bnds = default;
        NavMeshBuildSettings settings = default;
        UnityEngine.AI.NavMeshBuilder.UpdateNavMeshDataAsync(null, settings, null, bnds);
        */
        UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();

        //NOW PLACE AN OBJECT AT THE FAR END THAT IS A CAMPFIRE OR SOMETHING TO START NEXT LEVEL
    }

    void placeSquares(Vector3 addedPos, bool? pos = null) // for pos, true = first rooms, false = last rooms
    {


        prevRooms.Add(addedPos);

        GameObject room1 = rooms[0].objectToPlace;
        GameObject room2 = rooms[0].objectToPlace;
        GameObject room3 = rooms[0].objectToPlace;
        GameObject room4 = rooms[0].objectToPlace;
        
        if (pos == null)//IF A MIDDLE ROOM, ADD HIGHER ROOMS
        {
            int whichRoom = Random.Range(1, 4);

            switch (whichRoom)
            {
                case 1:
                    room1 = rooms[1].objectToPlace;
                    break;
                case 2:
                    room2 = rooms[1].objectToPlace;
                    break;
                case 3:
                    room3 = rooms[1].objectToPlace;
                    break;
                case 4:
                    room4 = rooms[1].objectToPlace;
                    break;
            }
        }

        room1 = Instantiate(room1, addedPos + new Vector3(length / 2, 0, length / 2), Quaternion.identity, roomHold.transform);
        placeObjects(room1, things, enemys);

        room2 = Instantiate(room2, addedPos + new Vector3(-length / 2, 0, length / 2), Quaternion.identity, roomHold.transform);
        placeObjects(room2, things, enemys);

        room3 = Instantiate(room3, addedPos + new Vector3(-length / 2, 0, -length / 2), Quaternion.identity, roomHold.transform);
        placeObjects(room3, things, enemys);

        room4 = Instantiate(room4, addedPos + new Vector3(length / 2, 0, -length / 2), Quaternion.identity, roomHold.transform);
        placeObjects(room4, things, enemys);


        Instantiate(wall, room1.transform.position, Quaternion.Euler(0, 180, 0)).transform.parent = roomHold.transform;//RIGHT SIDE WALLS
        Instantiate(wall, room4.transform.position, Quaternion.Euler(0, 180, 0)).transform.parent = roomHold.transform;

        Instantiate(wall, room2.transform.position, Quaternion.Euler(0, 0, 0)).transform.parent = roomHold.transform;//LEFT SIDE WALLS
        Instantiate(wall, room3.transform.position, Quaternion.Euler(0, 0, 0)).transform.parent = roomHold.transform;
        

        switch (pos)
        {
            case true:
                Instantiate(wall, room3.transform.position, Quaternion.Euler(0, -90, 0)).transform.parent = room3.transform;//CREATE BACK WALLS
                Instantiate(wall, room4.transform.position, Quaternion.Euler(0, -90, 0)).transform.parent = room4.transform;
                break;
            case false:
                Instantiate(wall, room1.transform.position, Quaternion.Euler(0, 90, 0)).transform.parent = room1.transform;
                Instantiate(wall, room2.transform.position, Quaternion.Euler(0, 90, 0)).transform.parent = room2.transform;
                Instantiate(campFire, new Vector3(0, 0, ((length * 2 * startRoomsNum) - (length + (length/2))) ), Quaternion.Euler(0, Random.Range(0, 360), 0)).transform.parent = roomHold.transform;
                break;
            default:
                break;
                //placeObjects(Instantiate(room, addedPos + new Vector3(-length / 2, 0, length / 2), Quaternion.identity, roomHold.transform), things, enemys);
                //placeObjects(Instantiate(room, addedPos + new Vector3(-length / 2, 0, -length / 2), Quaternion.identity, roomHold.transform), things, enemys);
                //placeObjects(Instantiate(room, addedPos + new Vector3(length / 2, 0, -length / 2), Quaternion.identity, roomHold.transform), things, enemys);

        }
    }
    //void placeOnAll(GameObject obj)
    //{
    //    for (int x = 0; x < roomHold.transform.childCount; x++) {
    //        GameObject floor = roomHold.transform.GetChild(x).GetChild(0).gameObject;
    //        Transform place = floor.transform;


    //        for (int i = 0; i < pillarDensity; i++)
    //        {
    //            Instantiate(obj, place.position + new Vector3(Random.Range(length/2, -length/2),0, Random.Range(length / 2, -length / 2)), Quaternion.identity);
    //            Debug.Log("Placed Pillar");
    //        }
    //    }
    //}
    void placeObjects(GameObject room, thing[] thingsPlace, thing[] enemysToPlace)
    {
        GameObject floor = room.transform.GetChild(0).gameObject;
        Transform place = floor.transform;

        var locations = new HashSet<Tuple<float, float>>();

        //GOES THROUGH EACH ITEM AND PLACES IT
        for (int x = 0; x < thingsPlace.Length; x++)
        {
            //PLACES THE ITEM BASED ON THE DENSITY VARIABLE SET IN EDITOR
            for (int i = 0; i < thingsPlace[x].density; i++)
            {
                int rotation = 45 * Random.Range(1,5);

                //GOES THROUGH RANDOM POSITIONS UNTIL IT GETS ONE THAT HASN'T BEEN PLACED
                float positionx;
                float positionz;
                //positionx = Random.Range(length / 2, -length / 2);
                //positionz = Random.Range(length / 2, -length / 2);
                do
                {
                    positionx = (Random.Range(1, gridDimensionXZDensity) * (length/ gridDimensionXZDensity))  - length/2;
                    positionz = (Random.Range(1, gridDimensionXZDensity) * (length/ gridDimensionXZDensity))  - length/2;
                } while (locations.Contains(new Tuple<float, float>(positionx, positionz)));
                locations.Add(new Tuple<float, float> (positionx, positionz));

                //locations.Add(new KeyValuePair<float, float>(positionx, positionz));
                Vector3 positionAdd = new Vector3(positionx, 0, positionz);

                //CREATES VARIOUS ITEMS ON IN ROOMS, THEN SETS THE ROOM AS A PARENT
                Instantiate(thingsPlace[x].objectToPlace, place.position + positionAdd, Quaternion.Euler(0, rotation, 0)).transform.parent = room.transform;
            }
        }
        //BAKE NAVMESH AFTER MAP COMPLETE
        //UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
        //UnityEditor.AI.NavMeshBuilder.BuildNavMesh();


        //GOES THROUGH EACH ENEMY AND PLACES IT
        for (int x = 0; x < enemysToPlace.Length; x++)
        {
            //PLACES THE ENEMY BASED ON THE DENSITY VARIABLE SET IN EDITOR
            for (int i = 0; i < enemysToPlace[x].density; i++)
            {
                int rotation = 45 * Random.Range(0, 8);

                //GOES THROUGH RANDOM POSITIONS UNTIL IT GETS ONE THAT HASN'T BEEN PLACED
                float positionx;
                float positionz;
                //positionx = Random.Range(length / 2, -length / 2);
                //positionz = Random.Range(length / 2, -length / 2);
                do
                {
                    positionx = (Random.Range(1, gridDimensionXZDensity) * (length / gridDimensionXZDensity)) - length / 2;
                    positionz = (Random.Range(1, gridDimensionXZDensity) * (length / gridDimensionXZDensity)) - length / 2;
                } while (locations.Contains(new Tuple<float, float>(positionx, positionz)));
                locations.Add(new Tuple<float, float>(positionx, positionz));

                //locations.Add(new KeyValuePair<float, float>(positionx, positionz));
                Vector3 positionAdd = new Vector3(positionx, 0, positionz);

                //CREATES VARIOUS ITEMS ON IN ROOMS, THEN SETS THE ROOM AS A PARENT
                Instantiate(enemysToPlace[x].objectToPlace, place.position + positionAdd, Quaternion.Euler(0, rotation, 0)).transform.parent = room.transform;
            }
        }        
    }

    /*private void Update()
    {
        //GETS THE PLAYER POSITION TO GENERATE MORE CHUNKS
        if (Math.Abs(playerPos.transform.position.z) >= length/2) //|| Math.Abs(playerPos.transform.position.x) >= length)
        {
            //CHECKS HASHMAP TO SEE IF A ROOM EXISTS IN THAT LOCATION
            if (!prevRooms.Contains(new Vector3(0, 0, length * 2)))
            {
                placeSquares(new Vector3(0, 0, length * 2));
            }

        }
    }*/

    [Serializable]
    public class thing
    {
        public GameObject objectToPlace;
        public int density;

    }
}