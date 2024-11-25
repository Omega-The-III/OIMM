using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RoomScript : UdonSharpBehaviour
{
    private MansionSetupScript mansionScript;

    public GameObject[] doorNodes;

    [SerializeField] private Vector2[] myCoordinates;
    [SerializeField] private GameObject doorPrefab;
    public GameObject roomModel;

    //Metadata for this room
    [SerializeField] private string roomType;

    GameObject doorIWannaSee;

    private void Start()
    {
        mansionScript = transform.parent.parent.GetComponent<MansionSetupScript>();
    }
    public void GenerateRoom(GameObject usedDoorNode)
    {
        // Make new room and grab its script as a ref
        GameObject newRoom = Instantiate(mansionScript.PickRandomRoom(), transform.position, Quaternion.identity, transform.parent);
        RoomScript newRoomScript = (RoomScript)newRoom.GetComponent(typeof(UdonBehaviour));

        // Pick a random doornode within the newly generated room to use as an anchor point to connect to this script's doornode, what the fuck is this sentance.
        int randomOtherRoomDoor = Random.Range(0, newRoomScript.doorNodes.Length);

        //This will rotate the room so it attaches neatly to the other door before moving the room.
        GameObject newRoomUsedDoorNode = newRoomScript.doorNodes[randomOtherRoomDoor];
        Vector3 angleDiff = usedDoorNode.transform.eulerAngles - newRoomScript.doorNodes[randomOtherRoomDoor].transform.eulerAngles;
        Vector3 newRot = new Vector3(angleDiff.x, 180 - angleDiff.y, angleDiff.z);
        newRoom.transform.eulerAngles -= newRot;

        //This places the new room infront of the current door based on the difference between the two connecting doors.
        Vector3 door2NewroomDist = (usedDoorNode.transform.position - newRoomUsedDoorNode.transform.position); //the pos of the door node we wanna attach;
        newRoom.transform.position += door2NewroomDist;

        CheckDoorCollisions(newRoomScript);

        newRoomScript.SendCustomEventDelayedSeconds("EnableAllDoors", 0.5f);

        //usedDoorNode.GetComponent<DoorNode>().RemoveTrigger();
    }
    
    private void CheckDoorCollisions(RoomScript newRoomScript)
    {
        RaycastHit hit;
        foreach (GameObject doornode in newRoomScript.doorNodes)
        {
            doorIWannaSee = doornode;
            if (Physics.BoxCast(doornode.transform.position + doornode.transform.up, new Vector3(1, 2, 1), doornode.transform.forward, out hit, doornode.transform.rotation, 200, 1 << 23))
            {
                hit.collider.gameObject.GetComponent<DoorNode>().RemoveTrigger();
                doornode.SetActive(false);
                doornode.name = doornode.name + " SHOULD BE CLOSED";
            }
        }
    }
    public void EnableAllDoors()
    {
        Debug.Log("enabling doors");
        foreach (GameObject door in doorNodes)
            if (door != null)
                door.SetActive(true);
    }
    private void OnDrawGizmos()
    {
        if(doorIWannaSee != null)
            Gizmos.DrawWireCube(doorIWannaSee.transform.position + doorIWannaSee.transform.up, new Vector3(1, 2, 1));
    }
}
