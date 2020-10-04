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

    // Private fields
    float playerStamina;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerFell()
    {
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

    void RespawnPlayer()
    {
        player.transform.position = respawnPosition;
        PlayerStamina = maxPlayerStamina;
    }
}
