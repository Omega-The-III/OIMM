using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DoorNode : UdonSharpBehaviour
{
    [SerializeField] MansionSetupScript mansionScript;

    //replace this with a script on the node slot itself that links objects together, this shit sucks
    [Tooltip("This will ALWAYS have ONLY two elements")]
    public GameObject[] roomDoorNodeSlots;

    [SerializeField] private SphereCollider trigger;
    private void Start()
    {
        mansionScript = (MansionSetupScript)gameObject.transform.parent.parent.parent.GetComponent(typeof(UdonBehaviour));
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        GenerateRoom();
        Destroy(trigger);
    }

    private void GenerateRoom()
    {
        // Make new room and grab its script as a ref
        GameObject newRoom = Instantiate(PickRandomRoom(), transform.position, Quaternion.identity, transform.parent.parent);
        RoomScript newRoomScript = (RoomScript)newRoom.GetComponent(typeof(UdonBehaviour));

        // Pick a random doornode within the newly generated room to use as an anchor point to connect to this script's doornode, what the fuck is this sentance.
        int randomOtherRoomDoor = Random.Range(0, newRoomScript.doorNodeSlots.Length);

        GameObject newRoomUsedDoorNode = newRoomScript.doorNodeSlots[randomOtherRoomDoor];
        roomDoorNodeSlots[1] = newRoomUsedDoorNode; // Complete the chain of rememberd doornode slots
        newRoomUsedDoorNode.name = "Occupied"; // Mark door as occupied to keep track of it all because i can't
        if (newRoomUsedDoorNode.transform.childCount > 0)
            newRoomUsedDoorNode.transform.GetChild(0).gameObject.SetActive(false); // Open the doorway

        //This will rotate the room so it attaches neatly to the other door before moving the room.
        Vector3 angleDiff = roomDoorNodeSlots[0].transform.eulerAngles - roomDoorNodeSlots[1].transform.eulerAngles;
        Vector3 newRot = new Vector3(angleDiff.x, 180 - angleDiff.y, angleDiff.z);
        newRoom.transform.eulerAngles -= newRot;

        //This places the new room infront of the current door based on the difference between the two connecting doors.
        Vector3 door2NewroomDist = newRoom.transform.position - roomDoorNodeSlots[1].transform.position; //the pos of the door node we wanna attach;
        newRoom.transform.position += door2NewroomDist;
    }
    private GameObject PickRandomRoom() { return mansionScript.spawnablesList[Random.Range(0, mansionScript.spawnablesList.Length)]; }
}