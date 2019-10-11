using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : LevelBuilder
{
    /// <summary>
    /// list of spots to place rooms
    /// </summary>
    public List<Vector2> coordinates = new List<Vector2>();
    /// <summary>
    /// where true values are placed rooms
    /// Arranged as such:
    /// 0 1 2 3 4 5 6 cols Y
    /// 1 | | | | | |
    /// 2 | | | | | |
    /// 3 | | | | | |
    /// 4 | | | | | |
    /// 5 | | | | | |
    /// rows X
    /// so X closer to 0 is north X closer to N is South
    /// and Y closer to 0 is West and Y closer to M is East
    /// </summary>
    public static bool[,] grid;
    public int Xleft; ///if XLeft is negative, then we are adding to X to go from start to end, (so go south)
    public int Yleft; /// if YLeft is negative, then we are adding to Y to go from start to end (so go east)
    public int[] dimensions = {0, 0}; /// 0 = rows, 1 = cols
    public int[] current;
    public int RoomsLeft;
    public List<Room> FourDoors;
    public List<Room> CorridorRooms;
    public List<Room> ElbowRooms;
    private NavMeshSurface[] NavMesh;

    /// <summary>
    /// Music to play on level load
    /// </summary>
    [SerializeField]
    protected SoundType levelMusic;
    

    /**Set up Grid:
     * Pick a number between the input min and max rooms
     * number of rows = sqrt(number)
     * number of cols = number of rows + 3
     * create grid[rows, cols]
     **/

    public void Start()
    {
        rooms = new List<Room>();
        roomLayerMask = LayerMask.GetMask("Room");
        StartCoroutine(Generate());
        //loading.enabled = true;
        //isBgOn = true;
    }
    public void makeGrid()
    {
        iterations = Random.Range((int)iterantionRange.x, (int)iterantionRange.y);
        RoomsLeft = iterations;
        if (iterations < 2)
        {
            iterations = 2;
        }
        dimensions[0] = (int)Mathf.Ceil(Mathf.Sqrt(iterations)) + 2; //set rows
        dimensions[1] = dimensions[0] + 1; //ensure grid is always greater than #rooms needed
        grid = new bool[(int)dimensions[0], (int)dimensions[1]];
        //init grid to false
        for (int r = 0; r < (int)dimensions[0]; r++)
        {
            for (int c = 0; c < (int)dimensions[1]; c++)
            {
                grid[r,c] = false;
            }
        }
    }
    
    /// <summary>
    /// Place the start and end room coordinates 
    /// *Note that this means the first and second coordinates in the coordinates list will always be
    /// start and end respectively
    /// Also, their manhattan distance will always be less than or equal to the number of rooms needed to form
    /// a path between them
    /// </summary>
    public void PlaceStartEnd()
    {
        //Start coordinates
        int x = (int)Random.Range(0, dimensions[0]);
        int y = (int)Random.Range(0, dimensions[1] );

        //End coordinates
        int endx = (int)Random.Range(0, dimensions[0]);
        int endy = (int)Random.Range(0, dimensions[1]);

        Xleft = x - endx;
        Yleft = y - endy;
        //ensure total manhattan distance is at least 1 and less than # rooms needed
        while (Xleft == 0 || Yleft == 0 || (Mathf.Abs(Xleft) + Mathf.Abs(Yleft)) > iterations)
        {
            x = (int)Random.Range(0, dimensions[0]);
            y = (int)Random.Range(0, dimensions[1]);
            endx = (int)Random.Range(0, dimensions[0]);
            endy = (int)Random.Range(0, dimensions[1]);
            Xleft = x - endx;
            Yleft = y - endy;
        }

        coordinates.Add(new Vector2(x, y));
        coordinates.Add(new Vector2(endx, endy));
        current = new int[]{x, y};
        grid[x, y] = true;
        grid[endx, endy] = true;
    }

    /// <summary>
    /// Assumes XLeft and Yleft are NOT == 0
    /// create a path in the grid abstraction from start to end rooms
    /// NOTE: path is finished when Yleft or Xleft == 0 and the other is 1 away from 0
    /// </summary>
    public void SetRoomCoordinates()
    {
        if(Xleft == 0 && Yleft == 0)
        {
            return;
        }
        int direction = (int)Random.Range(0, 1);
        int x, y;
        if ((direction == 0 && Xleft != 0) || (direction == 1 && Yleft == 0)) //move in x direction
        {
            if (Xleft < 0) //then add to x
            {
                x = current[0] + 1;
                y = current[1];
                Xleft += 1;
            }
            else
            {
                x = current[0] - 1;
                y = current[1];
                Xleft -= 1;
            }
        }
        else //move in y direction
        {
            if (Yleft < 0) //then add to y
            {
                x = current[0];
                y = current[1] + 1;
                Yleft += 1;
            }
            else
            {
                x = current[0];
                y = current[1] - 1;
                Yleft -= 1;
            }
        }
        
        if(x < dimensions[0] && y < dimensions[1] && grid[x, y] == true) //double check the coordinate is not taken
        {
            return;
        }
        coordinates.Add(new Vector2(x, y));
        current = new int[] { x, y };
        grid[x, y] = true;
        RoomsLeft -= 1;
    }

    /// <summary>
    /// Assuming RoomsLeft > 0 Add a new coordinate for a room and subtract 1 from RoomsLeft
    /// </summary>
    public void setRandomCoordinates()
    {
        int index = Random.Range(0, coordinates.Count); //check a random coordinate from the list
        while(index == 1)
        {
            index = Random.Range(0, coordinates.Count);
        }
        Vector2 picked = coordinates[index];
        List<int> directions = new List<int> { 0, 1, 2, 3 };
        int x, y;
        while (directions.Count > 0) //randomly pick directions from the list to add a coordinate until exhausted
        {
            int i = Random.Range(0, directions.Count);// pick a direction to move
            int direction = directions[i];
            directions.Remove(direction);
            if (direction == 0 && (int)picked[0] != 0) //move north if poss (X - 1)
            {
                x = ((int)picked[0] - 1);
                y = (int)picked[1];
                if (grid[x, y] != true) //double check the coordinate is not taken
                {
                    coordinates.Add(new Vector2(x, y));
                    current = new int[] { x, y };
                    grid[x, y] = true;
                    RoomsLeft -= 1;
                    return;
                }
            }
            else if (direction == 1 && (int)picked[0] != dimensions[0] - 1) //move south if poss
            {
                x = ((int)picked[0] + 1);
                y = (int)picked[1];
                if (grid[x, y] != true) //double check the coordinate is not taken
                {
                    coordinates.Add(new Vector2(x, y));
                    current = new int[] { x, y };
                    grid[x, y] = true;
                    RoomsLeft -= 1;
                    return;
                }
            }
            else if (direction == 2 && (int)picked[1] != dimensions[1] - 1) //move East if poss
            {
                x = (int)picked[0];
                y = ((int)picked[1] + 1);
                if (grid[x, y] != true) //double check the coordinate is not taken
                {
                    coordinates.Add(new Vector2(x, y));
                    current = new int[] { x, y };
                    grid[x, y] = true;
                    RoomsLeft -= 1;
                    return;
                }
            }
            else if (direction == 3 && (int)picked[1] != 0) //move West if poss (X - 1)
            {
                x = (int)picked[0];
                y = ((int)picked[1] - 1);
                if (grid[x, y] != true) //double check the coordinate is not taken
                {
                    coordinates.Add(new Vector2(x, y));
                    current = new int[] { x, y };
                    grid[x, y] = true;
                    RoomsLeft -= 1;
                    return;
                }
            }
        }
    }

    public void removeDoors(Room room, bool[] dirs)
    {
        List<(DoorLocation, Doorway)> doors = room.getDoorways();
        
        if(dirs[0] == true)//if there is a room to the north
        {
            (DoorLocation loc, Doorway door) = doors.Find(x => x.Item1 == DoorLocation.N);
            if(door != null)
            {
                door.turnOnDoor();
                door.gameObject.SetActive(false);
            }
        }
        if(dirs[1] == true) //east
        {
            (DoorLocation loc, Doorway door) = doors.Find(x => x.Item1 == DoorLocation.E);
            if (door != null)
            {
                door.turnOnDoor();
                door.gameObject.SetActive(false);
            }
        }
        if (dirs[2] == true) //east
        {
            (DoorLocation loc, Doorway door) = doors.Find(x => x.Item1 == DoorLocation.S);
            if (door != null)
            {
                door.turnOnDoor();
                door.gameObject.SetActive(false);
            }
        }
        if (dirs[3] == true) //east
        {
            (DoorLocation loc, Doorway door) = doors.Find(x => x.Item1 == DoorLocation.W);
            if (door != null)
            {
                door.turnOnDoor();
                door.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Rotate a room to fit certain...conditions (like elbow, corridor, etc)
    /// </summary>
    /// <param name="room">The rorom to rotate</param>
    /// <param name="dirs">the boolean list of where there are other rooms adjacent</param>
    public void RotateRoom(Room room, bool[] dirs) //dirs is N E S W
    {
        List<(DoorLocation, Doorway)> doors; //get doors
        if (dirs[0] == true && dirs[1] == true && dirs[2] == false && dirs[3] == false)//NorthEast Room
        {
            //NOTE: This is assuming the room is an elbow room, if not, well, we have a problem...
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.N);
                (DoorLocation loc2, Doorway door2) = doors.Find(x => x.Item1 == DoorLocation.E);
                if (door1 != null && door2 != null)
                {
                    Debug.Log(room.coordinates[0] + "," + room.coordinates[1] + ": " + loc1 + ", " + loc2);
                    break;
                }
            }
        }
        else if (dirs[0] == false && dirs[1] == true && dirs[2] == true && dirs[3] == false)//East South
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.S);
                (DoorLocation loc2, Doorway door2) = doors.Find(x => x.Item1 == DoorLocation.E);
                if (door1 != null && door2 != null)
                { 
                    break;
                }
            }
        }
        else if (dirs[0] == false && dirs[1] == false && dirs[2] == true && dirs[3] == true)//South West
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.W);
                (DoorLocation loc2, Doorway door2) = doors.Find(x => x.Item1 == DoorLocation.S);
                if (door1 != null && door2 != null)
                {
                    break;
                }
            }
        }
        else if (dirs[0] == true && dirs[1] == false && dirs[2] == false && dirs[3] == true)//North West
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.N);
                (DoorLocation loc2, Doorway door2) = doors.Find(x => x.Item1 == DoorLocation.W);
                if (door1 != null && door2 != null)
                {
                    break;
                }
            }
        }
        //Corridor
        else if (dirs[0] == true && dirs[1] == false && dirs[2] == true && dirs[3] == false)//NorthSouth
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.N);
                (DoorLocation loc2, Doorway door2) = doors.Find(x => x.Item1 == DoorLocation.S);
                if (door1 != null && door2 != null)
                {
                    break;
                }
            }
        }
        else if (dirs[0] == false && dirs[1] == true && dirs[2] == false && dirs[3] == true)
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.W);
                (DoorLocation loc2, Doorway door2) = doors.Find(x => x.Item1 == DoorLocation.E);
                if (door1 != null && door2 != null)
                {
                    break;
                }
            }
        }
        //else single room next to it...
        else if(dirs[0])
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.N);
                if (door1 != null)
                {
                    break;
                }
            }
        }
        else if (dirs[1])
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.E);
                if (door1 != null)
                {
                    break;
                }
            }
        }
        else if (dirs[2])
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.S);
                if (door1 != null)
                {
                    break;
                }
            }
        }
        else if (dirs[3])
        {
            for (int i = 1; i < 5; i++)//for each rotation
            {
                room.transform.rotation = Quaternion.Euler(0, rotationToDegree(i), 0);
                room.rotation = i;
                doors = room.getDoorways();
                (DoorLocation loc1, Doorway door1) = doors.Find(x => x.Item1 == DoorLocation.W);
                if (door1 != null)
                {
                    break;
                }
            }
        }
        else
        {
            int rotation = Random.Range(1, 5);
            room.transform.rotation = Quaternion.Euler(0, rotationToDegree(rotation), 0);
            room.rotation = rotation;
        }
    }

    public int rotationToDegree(int i)
    {
        if(i == 1)
        {
            return 0;
        }
        else if(i == 2)
        {
            return 90;
        }
        else if(i == 3)
        {
            return 180;
        }
        else
        {
            return 270;
        }
    }
    public void PlaceRooms()
    {
        int dist = 30;
        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector2 coord = coordinates[i];
            (int type, bool[] dirs) = getRoomType(coord);
            
            if (i == 0) //StartRoom
            {
                StartRoom startRoom = Instantiate(startRoomPrefab) as StartRoom;
                startRoom.transform.parent = this.transform;
                startRoom.transform.position = new Vector3(coord[0] * dist, 0, coord[1] * dist);
                //Set coordinate of startroom
                startRoom.coordinates = new int[]{ (int)coord.x, (int)coord.y};
                removeDoors(startRoom, dirs);
                rooms.Add(startRoom);
            }
            else if(i == 1) //EndRoom
            {
                EndRoom endRoom = Instantiate(endRoomPrefab) as EndRoom;
                endRoom.transform.parent = this.transform;
                endRoom.transform.position = new Vector3(coord[0] * dist, 0, coord[1] * dist);
                //set coordinated of endRoom
                endRoom.coordinates = new int[] { (int)coord.x, (int)coord.y };
                //rotate end room (It's an elbow room)...
                RotateRoom(endRoom, dirs);
                removeDoors(endRoom, dirs);
                rooms.Add(endRoom);
            }
            else
            {
                Room room;
                
                if(type == 1 && CorridorRooms.Count > 0) //corridor, and we actually HAVE corridors
                {
                    room = Instantiate(CorridorRooms[Random.Range(0, CorridorRooms.Count)]) as Room;
                    room.transform.parent = this.transform;
                    room.transform.position = new Vector3(coord[0] * dist, 0, coord[1] * dist);
                    //set coordinates
                    room.coordinates = new int[] { (int)coord.x, (int)coord.y };
                    //Rotate Room and destroy all doors
                    RotateRoom(room, dirs);
                    removeDoors(room, dirs);
                    rooms.Add(room);
                }
                else if(type == 2 && ElbowRooms.Count > 0)// Elbow
                {
                    room = Instantiate(ElbowRooms[Random.Range(0,ElbowRooms.Count)]) as Room;
                    room.transform.parent = this.transform;
                    room.transform.position = new Vector3(coord[0] * dist, 0, coord[1] * dist);
                    //set coordinates
                    room.coordinates = new int[] { (int)coord.x, (int)coord.y };
                    //rotate room and destroy all doors
                    RotateRoom(room, dirs);
                    removeDoors(room, dirs);
                    rooms.Add(room);
                }
                else //4room
                {
                    room = Instantiate(FourDoors[Random.Range(0, FourDoors.Count)]) as Room;
                    room.transform.parent = this.transform;
                    room.transform.position = new Vector3(coord[0] * dist, 0, coord[1] * dist);
                    //set coordinates
                    room.coordinates = new int[] { (int)coord.x, (int)coord.y };
                    //give the room a random rotation (1, 2, 3, or 4)
                    RotateRoom(room, dirs);
                    //remove doors.  Please note we are removing the ones we USE and keeping the others as WALLS
                    removeDoors(room, dirs);
                    rooms.Add(room);
                }
            }

        }
    }

    /// <summary>
    /// Given a coordinate, return the room type to place
    /// 0 for 4Room, 1 for Corridor, 2 for Elbow
    /// </summary>
    /// <param name="coord"></param>
    /// <returns></returns>
    public (int, bool[]) getRoomType(Vector2 coord)
    {
        int x = (int)coord[0];
        int y = (int)coord[1];
        //Check neighboring rooms
        //Combinations -> if NE, ES, SW, or WN only then Elbow
        // if NS or EW only then Corridor
        // else 4Room
        bool[] dirs = { false, false, false, false }; //N E S W
        if(x != 0) //north coord
        {
            dirs[0] = grid[x - 1, y];
        }
        if(y != dimensions[1] - 1)//east coord
        {
            dirs[1] = grid[x, y + 1];
        }
        if(x != dimensions[0] -1)//south coord
        {
            dirs[2] = grid[x + 1, y];
        }
        if(y != 0)//west coord
        {
            dirs[3] = grid[x, y - 1];
        }
        //check for adjacent trues (everything else false) those are ELBOW rooms
        if(dirs[0] == true && dirs[1] == true && dirs[2] == false && dirs[3] == false)
        {
            return (2, dirs);//elbow rooooom
        }
        else if (dirs[0] == false && dirs[1] == true && dirs[2] == true && dirs[3] == false)
        {
            return (2, dirs);
        }
        else if (dirs[0] == false && dirs[1] == false && dirs[2] == true && dirs[3] == true)
        {
            return (2, dirs);
        }
        else if (dirs[0] == true && dirs[1] == false && dirs[2] == false && dirs[3] == true)
        {
            return (2, dirs);
        }//check for alternating trues corridor type rooms
        else if (dirs[0] == true && dirs[1] == false && dirs[2] == true && dirs[3] == false)
        {
            return (1, dirs);
        }
        else if (dirs[0] == false && dirs[1] == true && dirs[2] == false && dirs[3] == true)
        {
            return (1, dirs);
        }
        else //4Room type
        {
            return (0, dirs);
        }
    }

    /**Generation
    * 1. place start room (give it coordinates)
    * 2. place end room (give it coordinates)
    *  -end room must be at least 1 x or 1 y away from start
    * 3. calculate manhattan distance from end to start (x and y distance)
    * 4. current position == start Room
    * Iterate over these:
    * 4. flip a coin for x or y if exists a doorway (else pick the doorway)
    * 5. pick a random room
    * 6. place room in direction of end Room from current position, (x or y)
    * 7. rotate room randomly until doorways match and subtract from x or y distance the scaled value 
    * 8. If the x or y less than 0 select a new room and repeat from 4
    * 9. else place room and add new coordinates to list of coordinates
    * 10.set current room to coordinate closest to end
    * 11.  repeat until room is connected to endRoom.
    * 12.  If there are left over room iterations add randomly to the adjacent grid by picking a random coordinate from
    *  list of placed room coordinates
    * Treat a multiple scale rooms a multiple rooms?
    *
   **/
    IEnumerator Generate()
    {
        //WaitForFixedUpdate startup = new WaitForFixedUpdate();
        WaitForFixedUpdate interval = new WaitForFixedUpdate();
        
        makeGrid();
        PlaceStartEnd();
        
        while (!((Xleft == 0 && (Yleft == 1 || Yleft == -1)) || (Yleft == 0 && (Xleft == 1 || Xleft == -1))))
        {
            SetRoomCoordinates();
        }
        while(RoomsLeft > 0)
        {
            setRandomCoordinates();
        }
        //RoomsLeft should be 0 now so start placing rooms
        //Note, there is 10 units of distance b/w rooms
        //printGrid();
        yield return interval;
        PlaceRooms();

        yield return interval;
        foreach (Room room in rooms)
        {
            if(room != null)
            {
                room.ShowRoom();
            }

            yield return interval;
        }
        // Once level generation has finished, populate each room with its enemies
        
        foreach (Room room in rooms)
        {
            if (room != null)
            {
                room.Populate();
                EscapePrevention escape = room.HideMe.GetComponentInChildren<EscapePrevention>();
                if(escape != null)
                {
                    escape.Load(room);
                }

            }

            yield return interval;
        }

        // Unpause music once level generation is finished
        NavMesh = this.gameObject.GetComponentsInChildren<NavMeshSurface>();
        foreach (NavMeshSurface mesh in NavMesh) {
            mesh.BuildNavMesh();
            yield return interval;
        }
        loading.enabled = false;
        isBgOn = false;
        loading.gameObject.SetActive(isBgOn);

        // Set level music if specified
        if(!levelMusic.Equals(SoundType.NONE)) {
            SoundController.Instance.SetMusic(levelMusic);
        }
        SoundController.Instance.SetMusicPlaying(true);

        levelComplete.Invoke();
    }
   
    public void printGrid()
    {
        //string total = "";
        for(int i = 0; i < (int)dimensions[0]; i++)
        {
            string row = i+ "|";
            for(int j = 0; j < (int)dimensions[1]; j++)
            {
                if(coordinates[0][0] == i && coordinates[0][1] == j)
                {
                    row += "s|";
                }
                else if(coordinates[1][0] == i && coordinates[1][1] == j)
                {
                    row += "e|";
                }
                else if(grid[i, j])
                {
                    row += "r|";
                }
                else
                {
                    row += " |";
                }
            }
            //total += row;
            //total += "\n";
            //total += "--------------\n";
            Debug.Log(row);
            Debug.Log("-----------------------");
        }
        //Debug.Log(total);
    }

    private void OnDestroy()
    {
        //rooms = new List<Room>();
        //grid = new bool[0, 0];
    }
    public static bool[,] getGrid()
    {
        return grid;
    }

    public static List<Room> getRooms()
    {
        return rooms;
    }
}
