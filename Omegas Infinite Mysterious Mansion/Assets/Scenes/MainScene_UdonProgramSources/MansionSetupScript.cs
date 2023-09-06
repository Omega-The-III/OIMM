using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MansionSetupScript : UdonSharpBehaviour
{
    public GameObject[] spawnablesList;

    [SerializeField] private TextMeshProUGUI text;
    [UdonSynced] public int syncedInstanceSeed;
    VRCPlayerApi playerApi;

    [Header("Player Settings")]
    [SerializeField] float jumpImpulse = 3;
    [SerializeField] float walkSpeed = 2;
    [SerializeField] float runSpeed = 4;
    [SerializeField] float gravityStrengh = 1;

    void Start()
    {
        if (Networking.LocalPlayer.isMaster)
        {
            GenerateSeed();
        }

        playerApi = Networking.LocalPlayer;
        playerApi.SetJumpImpulse(jumpImpulse);
        playerApi.SetWalkSpeed(walkSpeed);
        playerApi.SetRunSpeed(runSpeed);
        playerApi.SetGravityStrength(gravityStrengh);
    }
    //This should generate and update a new seed if one is requested.
    private void GenerateSeed()
    {
        int seed = Random.Range(111111, 999999);
        syncedInstanceSeed = seed;
        //SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "UpdateSeed");
        UpdateSeed();
    }
    public void UpdateSeed()
    {
        text.text = "Seed: " + syncedInstanceSeed.ToString();
        Random.InitState(syncedInstanceSeed);
    }
    //this should be for late joiners
    public override void OnDeserialization()
    {
        base.OnDeserialization();
        UpdateSeed();
    }
}
