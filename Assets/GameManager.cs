using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // UI
    public TMPro.TextMeshProUGUI staminaView;

    // Public fields
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
            staminaView.text = playerStamina.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerStamina = 0;
        player.GetComponent<PlayerMovement>().SetOnPlayerMovementListener(OnPlayerMovement);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerMovement(float dist)
    {
        PlayerStamina += dist;
        if (PlayerStamina >= 100)
        {
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        player.transform.position = respawnPosition;
        PlayerStamina = 0;
    }
}
