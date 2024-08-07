﻿using System.Collections;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RoomScript : UdonSharpBehaviour
{
    public GameObject[] doorNodeSlots;

    [SerializeField] private GameObject doorPrefab;
    public GameObject roomModel;

    //Metadata for this room
    [SerializeField] private string roomType, roomSize;

    private float maxRaycastDist = 3f;

    private void Start()
    {
        SendCustomEventDelayedSeconds("PopulateDoors", 0.1f);
    }

    public void PopulateDoors()
    {
        for(int i = 0; i < doorNodeSlots.Length; i++)
        {
            //Check if the door is unoccupied
            if (doorNodeSlots[i].name == "UnOccupied")
            {
                Transform doorTransform = doorNodeSlots[i].gameObject.transform;

                RaycastHit hit;
                Ray RayForward = new Ray(doorTransform.position, doorTransform.forward * 2);

                if (!Physics.Raycast(RayForward, out hit, maxRaycastDist))
                {
                    if (doorNodeSlots.Length < 3) //if we only have less then 3 doors always spawn doors
                        PlaceDoor(i, doorTransform);
                    else if (Random.value > 0.1f) //10% chance to not spawn a door, this needs to be changed with other door types
                        PlaceDoor(i, doorTransform); //Spawn a door prefab and set it up
                }
                else  // If there is something behind this door then block the entryway
                {
                    if (doorTransform.childCount > 0)
                        doorTransform.GetChild(0).gameObject.SetActive(true);
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        }
        //Upon activation it will check if doorslots are populated or not or already occupied and populate those that arn't with a chance not to.
        //if it will populate then place a random room behind there and check if it can be placed place it or otherwise remove the room and disable the doornode.
    }
    private void PlaceDoor(int doorSlotIndex, Transform doorTransf) // Note make this get a random door if i want different kinds of doors later on
    {
        GameObject newDoor = Instantiate(doorPrefab, doorTransf.position, doorTransf.rotation, transform.parent);

        DoorNode doorNodeScript = (DoorNode)newDoor.transform.GetChild(0).GetComponent(typeof(UdonBehaviour));

        doorTransf.gameObject.name = doorSlotIndex.ToString(); //Mark doorslot as populated

        doorNodeScript.roomDoorNodeSlots[0] = doorNodeSlots[doorSlotIndex];
    }
}
