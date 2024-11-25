using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DoorNode : UdonSharpBehaviour
{
    private RoomScript myRoom;

    private void Start()
    { 
        myRoom = (RoomScript)transform.parent.parent.parent.GetComponent(typeof(UdonBehaviour));
    }
    public override void OnPlayerTriggerEnter(VRCPlayerApi player) { myRoom.GenerateRoom(gameObject); RemoveTrigger(); }
    public void RemoveTrigger() { Destroy(GetComponent<Collider>()); }
}