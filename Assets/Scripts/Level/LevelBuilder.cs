using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class LevelBuilder : MonoBehaviour
{
     //Different types of rooms prefabs
    public Room startRoomPrefab, endRoomPrefab;
    
    //List of normal rooms or "corridors"
    public List<Room> roomPrefabs = new List<Room>();
    
    //Range for amount of rooms in the level
    public Vector2 iterantionRange = new Vector2(3, 10);

    public RawImage loading;
    public bool isBgOn;

    List<Doorway> availableDoorways = new List<Doorway>();
    public NavMeshSurface[] surfaces;
    private NavMeshSurface roomNavMesh;


    //Initialization
    StartRoom startRoom;
    EndRoom endRoom;
    List<Room> placedRooms = new List<Room>();

    public LayerMask roomLayerMask;
    //Added by Rush 7/3/19
    public UnityEvent levelComplete;
    public int iterations;
    public static List<Room> rooms = new List<Room>();


    void Start()
    {
        /*roomLayerMask = LayerMask.GetMask("Room");
        StartCoroutine("GenerateLevel");

         loading.enabled = true;
         isBgOn = true;
         */
        
    }

    IEnumerator GenerateLevel()
    {
        WaitForFixedUpdate startup = new WaitForFixedUpdate();
        WaitForFixedUpdate interval = new WaitForFixedUpdate();

        yield return startup;

        //Start Room
        PlaceStartRoom();
        yield return interval;

        iterations = Random.Range((int)iterantionRange.x, (int)iterantionRange.y);

        for (int i = 0; i < iterations; i++)
        {
            //Random room from list
            PlaceRoom();
            yield return interval;
        }

        //End room
        PlaceEndRoom();

        if (CheckRoomOverlapPostGeneration())
        {
            ResetLevelGenerator();
        }

        // Added by Michelle Zhong to set objects to active after generation
        startRoom.ShowRoom();
        foreach (Room room in placedRooms) {
            room.ShowRoom();
        }

        endRoom.ShowRoom();

        // Once level generation has finished, populate each room with its enemies
        foreach (Room room in placedRooms) {
            room.Populate();
            room.HideMe.GetComponentInChildren<EscapePrevention>().Load(room);
        }

        endRoom.Populate();
        endRoom.HideMe.GetComponentInChildren<EscapePrevention>().Load(endRoom, true);

        yield return interval;
        loading.enabled = false;
        isBgOn = false;
        loading.gameObject.SetActive(isBgOn);

        // Unpause music once level generation is finished
        SoundController.Instance.SetMusicPlaying(true);

    }

    void PlaceStartRoom()
    {
        startRoom = Instantiate(startRoomPrefab) as StartRoom;
        startRoom.transform.parent = this.transform;

        // Pause music until level generation has finished
        SoundController.Instance.SetMusicPlaying(false);

        AddDoorwaysToList(startRoom, ref availableDoorways);

        startRoom.transform.position = Vector3.zero;
        startRoom.transform.rotation = Quaternion.identity;
        startRoom.ShowRoom();
        //added Rush 7/3/19--need to add start room to room list to have a chance to spawn one in start room
        //Remooved 8/3/19 because we never want a grail to spawn in this room we decided. Can uncomment out later if we change mind.
        rooms.Add(startRoom);
    }

    void AddDoorwaysToList(Room room, ref List<Doorway> list)
    {
        foreach (Doorway doorway in room.doorways)
        {
            int r = Random.Range(0, list.Count);
            list.Insert(r, doorway);
        }
    }

    void PlaceRoom()
    {

        //Instantiate the room
        Room currentRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)]) as Room;
        currentRoom.transform.parent = this.transform;

        //Creates the list of doorways the function uses to loop over.
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        List<Doorway> currentRoomDoorways = new List<Doorway>();
        AddDoorwaysToList(currentRoom, ref currentRoomDoorways);

        //Gets the doorways from current room and randomly adds them to the list of available doorways
        AddDoorwaysToList(currentRoom, ref availableDoorways);

        bool roomPlaced = false;

        //Try all doorways
        foreach (Doorway availableDoorway in allAvailableDoorways)
        {

            foreach (Doorway currentDoorway in currentRoomDoorways)
            {
                 //Position of the room determined here
                PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);

                //Checks for room overlap
                if (CheckRoomOverlap(currentRoom))
                {
                    continue;

                }
                if ((currentRoom.transform.position - startRoom.transform.position).magnitude <= 5)
                {
                    ResetLevelGenerator();
                    break;
                }

                roomPlaced = true;

                //Adds the room to the list
                placedRooms.Add(currentRoom);

                //Removes occupied doorways from the list
                currentDoorway.gameObject.GetComponent<Doorway>().turnOnDoor();
                currentDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(currentDoorway);

                availableDoorway.gameObject.GetComponent<Doorway>().turnOnDoor();
                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);

                //Called when the room is placed correctly. Exits loop
                break;
            }
            //Called when the room is placed correctly. Exits loop
            if (roomPlaced)
            {
                roomNavMesh = currentRoom.GetComponentInChildren<NavMeshSurface>();
                roomNavMesh.BuildNavMesh();
                //Nick made changes here 6/6/19
                currentRoom.ShowRoom();
                //Rush Change on 7/3 to add validated rooms to a list to iterate through for grail spawning later
                rooms.Add(currentRoom);
                break;

            }
        }

        //Room couldn't be placed. Restarts the generator
        if (!roomPlaced) {
            Destroy(currentRoom.gameObject);
            ResetLevelGenerator();
        }
    }

    void PositionRoomAtDoorway(ref Room room, Doorway roomDoorway, Doorway targetDoorway)
    {
        //Reset room position and rotation
        room.transform.position = Vector3.zero;
        room.transform.rotation = Quaternion.identity;

        //Fun math. Haha, he go.
        Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
        Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
        float deltaAngle = Mathf.DeltaAngle(roomDoorwayEuler.y, targetDoorwayEuler.y);
        Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
        room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0,180f,0);

        Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
        room.transform.position = targetDoorway.transform.position - roomPositionOffset;
    }

    bool CheckRoomOverlap(Room room)
    {
        //Checks all the bounds of the room's plane (the floor) to see if it overlaps with another plane
        Vector3 roomPos = room.gameObject.transform.position;
        Vector3 pt1 = new Vector3(room.RoomBounds.center.x - room.RoomBounds.extents.x, room.RoomBounds.center.y + room.RoomBounds.extents.y, room.RoomBounds.center.z - room.RoomBounds.extents.z);
        Vector3 pt2 = new Vector3(room.RoomBounds.center.x - room.RoomBounds.extents.x, room.RoomBounds.center.y - room.RoomBounds.extents.y, room.RoomBounds.center.z + room.RoomBounds.extents.z);
        Vector3 pt3 = new Vector3(room.RoomBounds.center.x - room.RoomBounds.extents.x, room.RoomBounds.center.y + room.RoomBounds.extents.y, room.RoomBounds.center.z + room.RoomBounds.extents.z);
        Vector3 pt4 = new Vector3(room.RoomBounds.center.x + room.RoomBounds.extents.x, room.RoomBounds.center.y - room.RoomBounds.extents.y, room.RoomBounds.center.z - room.RoomBounds.extents.z);
        Vector3 pt5 = new Vector3(room.RoomBounds.center.x + room.RoomBounds.extents.x, room.RoomBounds.center.y + room.RoomBounds.extents.y, room.RoomBounds.center.z - room.RoomBounds.extents.z);
        Vector3 pt6 = new Vector3(room.RoomBounds.center.x + room.RoomBounds.extents.x, room.RoomBounds.center.y - room.RoomBounds.extents.y, room.RoomBounds.center.z + room.RoomBounds.extents.z);

        foreach (Room r in placedRooms)
        {
            if (r.RoomBounds.Intersects(room.RoomBounds) || 
                r.RoomBounds.Contains(roomPos)|| 
                r.RoomBounds.Contains(room.RoomBounds.min) 
                || r.RoomBounds.Contains(room.RoomBounds.max)
                || r.RoomBounds.Contains(pt1)
                || r.RoomBounds.Contains(pt2)
                || r.RoomBounds.Contains(pt3)
                || r.RoomBounds.Contains(pt4)
                || r.RoomBounds.Contains(pt5)
                || r.RoomBounds.Contains(pt6))
            {
                // //Debug.Log("Overlap Detected");
                return true;
            }
        }

        return false;
    }

     //Kat's code
    public bool CheckRoomOverlapPostGeneration()
    {

        if ((endRoom.transform.position - startRoom.transform.position).magnitude <= 20)
            return true;

        foreach (Room roomi in placedRooms)
        {

            if ((roomi.transform.position - startRoom.transform.position).magnitude <= 20)
                return true;

            if ((roomi.transform.position - endRoom.transform.position).magnitude <= 20)
                return true;

            foreach (Room roomj in placedRooms)
            {
                if (roomi == roomj)
                    continue;

                if ((roomi.transform.position - roomj.transform.position).magnitude <= 20)
                    return true;

            }
        }

        return false;
    }

    //Function places the endroom (boss room) after the level has been built. Currently, it looks for just an acceptable place, so next to start room is allowed
    //Similar to the PlaceRoom function.
    void PlaceEndRoom()
    {

        endRoom = Instantiate (endRoomPrefab) as EndRoom;
        endRoom.transform.parent = this.transform;

        
        List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
        Doorway doorway = endRoom.doorways [0];


        bool roomPlaced = false;

          foreach (Doorway availableDoorway in allAvailableDoorways)
        {
            Room room = (Room)endRoom;

             PositionRoomAtDoorway(ref room, doorway, availableDoorway);
                ////Debug.Log(currentRoom);
                if (CheckRoomOverlap(endRoom))
                {
                    ////Debug.Log("HI");
                    continue;

                }

            if ((endRoom.transform.position - startRoom.transform.position).magnitude <= 5)
            {
                ////Debug.Log("HI");
                ResetLevelGenerator();
                break;
            }

            roomPlaced = true;
            doorway.gameObject.GetComponent<Doorway>().turnOnDoor();
            doorway.gameObject.SetActive(false);
            availableDoorways.Remove(doorway);
            availableDoorway.gameObject.GetComponent<Doorway>().turnOnDoor();
                availableDoorway.gameObject.SetActive(false);
                availableDoorways.Remove(availableDoorway);
            
                break;

        }
        if (roomPlaced)
        {
            roomNavMesh = endRoom.GetComponentInChildren<NavMeshSurface>();
            roomNavMesh.BuildNavMesh();
            endRoom.ShowRoom();
            //Added by Rush 7/3/19 to call my grail code
            ////Debug.Log("This is the true finish/Confirmation--should only print once");
            levelComplete.Invoke();
        }


        if (!roomPlaced) {
            ResetLevelGenerator();
        }


    }

    //Safely removes all elements of the incorrect level so the script can start a new one.
    void ResetLevelGenerator()
    {
        //Rush Change Here, 7/3/19, Clear out rooms list when we restart to avoid nulls
        rooms.Clear();

        ////Debug.Log("Reset level generator");
        StopCoroutine("GenerateLevel");

        //delete rooms
        if (startRoom)
        {
            Destroy(startRoom.gameObject);
        }

        if (endRoom)
        {
            Destroy(endRoom.gameObject);
        }
        foreach (Room room in placedRooms)
        {
            Destroy(room.gameObject);
        }

        placedRooms.Clear();
        availableDoorways.Clear();


        StartCoroutine("GenerateLevel");
        
    }


}
