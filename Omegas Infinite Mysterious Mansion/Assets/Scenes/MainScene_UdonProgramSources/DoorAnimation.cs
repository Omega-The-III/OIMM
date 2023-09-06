    using UdonSharp;
using UnityEngine;

public class DoorAnimation : UdonSharpBehaviour
{
    //this will need a local door moderator to disable doors fully when out of range of a player to minimize fixed updates
    //Animation stuff
    [SerializeField] private float animDuration;
    [SerializeField] private float targetRotation;
    private bool animate, openState;
    private float timeElapsed;
    public override void Interact()
    {
        animate = true;
    }
    private void FixedUpdate()
    {
        if (animate)
        {
            if (openState) targetRotation = 0; else targetRotation = 135;

            if (timeElapsed <= animDuration)
            {
                Vector3 rotation = new Vector3(0, Mathf.Lerp(transform.localRotation.eulerAngles.y, targetRotation, timeElapsed / animDuration), 0);
                transform.localRotation = Quaternion.Euler(rotation);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                timeElapsed = 0;
                animate = false;
                openState = !openState;
            }
        }
    }
}