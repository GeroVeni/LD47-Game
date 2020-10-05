using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject flareePrefab;
    public GameObject bigFlareePrefab;
    public Transform flareeParent;

    // Private fields
    float playerStamina;
    Dictionary<Pickup.PickupType, bool> pickups = new Dictionary<Pickup.PickupType, bool>();
    List<Vector3> flareePositions;
    Vector3 bigFlareePosition;

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

        AudioManager.instance.Play("theme");

        flareePositions = new List<Vector3>();
        foreach (var flaree in FindObjectsOfType<Flaree>())
        {
            flareePositions.Add(flaree.transform.position);
        }
        bigFlareePosition = FindObjectOfType<BigFlaree>().transform.position;

        RespawnPlayer(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
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

    public bool CanShoot()
    {
        return pickups[Pickup.PickupType.APPLE];
    }

    bool IsWater()
    {
        return !pickups[Pickup.PickupType.WATER];
    }

    void RespawnPlayer(bool playSound = true)
    {
        if (playSound) AudioManager.instance.Play("death");
        // Reset player position and stamina
        player.transform.position = respawnPosition;
        PlayerStamina = maxPlayerStamina;

        // Display collected items
        bool collectedAll = true;
        foreach (var item in pickups)
        {
            if (!item.Value) collectedAll = false;
            GetPickupDisplay(item.Key).SetActive(item.Value);
        }
        if (collectedAll)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        Array.ForEach(FindObjectsOfType<Tree>(), tree => tree.IsSick = !IsWater());

        // TODO: Clear existing enemies
        Array.ForEach(FindObjectsOfType<Flaree>(), flaree => Destroy(flaree.gameObject));
        Destroy(FindObjectOfType<BigFlaree>()?.gameObject);
        // TODO: Respawn enemies
        flareePositions.ForEach(flareePos =>
        {
            var fl = Instantiate(flareePrefab, flareePos, Quaternion.identity, flareeParent);
            fl.GetComponent<Flaree>().state = CanShoot() ? Flaree.FlareeState.HUNT : Flaree.FlareeState.PEACEFULL;
        });
        Instantiate(bigFlareePrefab, bigFlareePosition, Quaternion.identity, flareeParent);
    }
}
