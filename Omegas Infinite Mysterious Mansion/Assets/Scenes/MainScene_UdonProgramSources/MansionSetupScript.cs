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
        playerApi = Networking.LocalPlayer;
        playerApi.SetJumpImpulse(jumpImpulse);
        playerApi.SetWalkSpeed(walkSpeed);
        playerApi.SetRunSpeed(runSpeed);
        playerApi.SetGravityStrength(gravityStrengh);

        if (playerApi.isMaster)
        {
            GenerateSeed();
            RequestSerialization();
        }
    }
    private void GenerateSeed()
    {
        int seed = Random.Range(111111, 999999);
        syncedInstanceSeed = seed;
        UpdateSeed();
    }
    public void UpdateSeed()
    {
        text.text = "Seed: " + syncedInstanceSeed.ToString();
        Random.InitState(syncedInstanceSeed);
    }
    public override void OnDeserialization()
    {
        base.OnDeserialization();
        UpdateSeed();
    }
}
