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
        transform.position = Networking.LocalPlayer.GetPosition();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Room") || other.name.Contains("Door"))
        {
            if (!other.gameObject.transform.GetChild(0).gameObject.activeSelf)
                other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Room") || other.name.Contains("Door"))
        {
            if (other.gameObject.transform.GetChild(0).gameObject.activeSelf)
                other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}