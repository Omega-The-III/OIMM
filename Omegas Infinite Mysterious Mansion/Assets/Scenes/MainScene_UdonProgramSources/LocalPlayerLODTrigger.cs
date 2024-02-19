using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class LocalPlayerLODTrigger : UdonSharpBehaviour
{
    [SerializeField] private SphereCollider triggerCollider;
    private void Start()
    {
        triggerCollider.enabled = true;
    }
    private void FixedUpdate()
    {
        if(Networking.LocalPlayer != null)
            transform.position = Networking.LocalPlayer.GetPosition();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Room") || other.name.Contains("Door"))
        {
            for (int i = 0; i < other.gameObject.transform.childCount; i++)
                other.gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("_Room") || other.name.Contains("DoorNode"))
        {
            for(int i = 0; i < other.gameObject.transform.childCount; i++)
                other.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}