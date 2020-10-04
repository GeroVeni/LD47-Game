using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        APPLE,
        WATER,
        HONEY,
        THUNDER
    }

    public PickupType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Pick up item
        GameObject go = collision.gameObject;
        if (go.CompareTag("Player"))
        {
            go.GetComponent<PickupManager>().NewItem(type);
            Destroy(gameObject);
        }
    }
}
