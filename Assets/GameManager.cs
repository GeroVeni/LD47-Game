using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // UI
    public HealthBar staminaView;

    // Public fields
    public float maxPlayerStamina;
    public Vector3 respawnPosition;
    public GameObject player;

    public GameObject displayApple;
    public GameObject displayWater;
    public GameObject displayHoney;
    public GameObject displayThunder;

    // Private fields
    float playerStamina;
    Dictionary<Pickup.PickupType, bool> pickups = new Dictionary<Pickup.PickupType, bool>();

    // Properties
    public float PlayerStamina
    {
        get { return playerStamina; }

        set
        {
            playerStamina = value;
            staminaView.SetValue(playerStamina, maxPlayerStamina);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerStamina = maxPlayerStamina;
        player.GetComponent<PlayerMovement>().SetOnPlayerMovementListener(OnPlayerMovement);

        pickups[Pickup.PickupType.APPLE] = false;
        pickups[Pickup.PickupType.WATER] = false;
        pickups[Pickup.PickupType.HONEY] = false;
        pickups[Pickup.PickupType.THUNDER] = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerFell()
    {
        RespawnPlayer();
    }

    public void PickupItem(Pickup.PickupType pickup)
    {
        pickups[pickup] = true;
        RespawnPlayer();
    }

    void OnPlayerMovement(float dist)
    {
        PlayerStamina -= dist;
        if (PlayerStamina <= 0)
        {
            RespawnPlayer();
        }
    }

    GameObject GetPickupDisplay(Pickup.PickupType pickup)
    {
        if (pickup == Pickup.PickupType.APPLE) return displayApple;
        if (pickup == Pickup.PickupType.WATER) return displayWater;
        if (pickup == Pickup.PickupType.HONEY) return displayHoney;
        if (pickup == Pickup.PickupType.THUNDER) return displayThunder;
        return new GameObject();
    }

    void RespawnPlayer()
    {
        // Reset player position and stamina
        player.transform.position = respawnPosition;
        PlayerStamina = maxPlayerStamina;

        // Display collected items
        foreach (var item in pickups)
        {
            GetPickupDisplay(item.Key).SetActive(item.Value);
        }
        
        // TODO: Clear existing enemies
        // TODO: Respawn enemies
    }
}
