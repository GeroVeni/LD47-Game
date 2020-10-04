using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
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
        GameObject go = collision.gameObject;
        if (go.CompareTag("Player"))
        {
            // Player fell
            GameManager.Instance.PlayerFell();
        } else
        {
            // Animal fell
            Destroy(go);
        }
    }
}
