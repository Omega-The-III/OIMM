using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RoomScript : UdonSharpBehaviour
{
    public GameObject[] doorNodeSlots;

    [SerializeField] private GameObject doorPrefab;

    //Metadata for this room
    [SerializeField] private string roomType, roomSize;
    [SerializeField] private Vector3 _size;
    [SerializeField] private Vector3 center;

    public Vector3 Size { get { return _size; } }

    private void Start()
    {
        SendCustomEventDelayedSeconds("PopulateDoors", 0.1f);
    }
    public void PopulateDoors()
    {
        for(int i = 0; i < doorNodeSlots.Length; i++)
        {
            //If name is 0 then door is unoccupied
            if (doorNodeSlots[i].name == "UnOccupied")
            {
                // check if want to spawn hallway or room
                // make spawn room function to decide what room and give chance of not spawning door
                // make generic spawn/finish up function that instaniates given objece and place and rotation and links rooms
                // make spawn hallways function that NEVER doesnt spawn doors.
                // make spawn door function that changes the type of doors that spawn.
                //new idea, spawn 1 of several doors. normal door, only door frame.
                //Very MAYBE a wall?
                if (doorNodeSlots.Length < 3) //if we only have 2 or 1 door always spawn doors
                    SpawnDoor(i);
                else if (Random.value > 0.1f) //10% chance to not spawn a door, this needs to be changed with other door types
                    SpawnDoor(i); //Spawn a door prefab and set it up
            }
        }
        //Upon activation it will check if doorslots are populated or not or already occupied and populate those that arn't with a chance not to.
        //if it will populate then place a random room behind there and check if it can be placed place it or otherwise remove the room and disable the doornode.
    }
    private void SpawnDoor(int doorSlotIndex)
    {
        GameObject newDoor = Instantiate(doorPrefab, doorNodeSlots[doorSlotIndex].gameObject.transform.position, doorNodeSlots[doorSlotIndex].gameObject.transform.rotation, transform.parent);
        DoorNode doorNodeScript = (DoorNode)newDoor.transform.GetChild(0).GetComponent(typeof(UdonBehaviour));
        doorNodeScript.linkedRooms[0] = this;
        doorNodeSlots[doorSlotIndex].gameObject.name = doorSlotIndex.ToString(); //Mark doorslot as populated
        doorNodeScript.roomDoorNodeSlots[0] = doorNodeSlots[doorSlotIndex];
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + center, _size);
    }
}
