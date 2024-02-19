using UdonSharp;
using UnityEngine;

public class DoorAnimation : UdonSharpBehaviour
{
    [SerializeField] private float animDuration;
    [SerializeField] private float targetRotation;
    private bool animate = false, openState = false;
    private float timeElapsed;
    public override void Interact()
    {
        if (!animate)
        {
            SwitchOpenState();
            SendCustomEventDelayedFrames("AnimateDoor", 2);
            animate = true;
        }
    }
    public void AnimateDoor()
    {
        if (timeElapsed < animDuration)
        {
            Vector3 rotation = new Vector3(0, Mathf.Lerp(transform.localRotation.eulerAngles.y, targetRotation, timeElapsed / animDuration), 0);
            transform.localRotation = Quaternion.Euler(rotation);
            timeElapsed += Time.deltaTime;
            SendCustomEventDelayedFrames("AnimateDoor", 2);
        } else {
            transform.localRotation = Quaternion.Euler(0, targetRotation, 0);
            timeElapsed = 0;
            animate = false;
        }
    }
    private void SwitchOpenState()
    {
        openState = !openState;
        if (openState) targetRotation = 135;
        else targetRotation = 0;
    }
}