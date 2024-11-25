
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RaycastTest : UdonSharpBehaviour
{
    void Start()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 200, Color.red, 999);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, 1 << 23))
        {
            Debug.Log("we hit " + hit.collider.gameObject.name + " with layermask 23/doors");
        }
        else
        {
            Debug.Log("we hit NOTHING with layermask 23/doors");
        }
    }
}
