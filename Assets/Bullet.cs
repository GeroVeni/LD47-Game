using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var go = collision.gameObject;
        var flaree = go.GetComponent<Flaree>();
        Debug.Log("collide");
        if (flaree)
        {
            AudioManager.instance.Play("monsterHit");
            Debug.Log("hit flaree");
            Destroy(flaree.gameObject);
        } else
        {
            var bigFlaree = go.GetComponent<BigFlaree>();
            if (bigFlaree)
            {
                AudioManager.instance.Play("monsterHit");
                Destroy(bigFlaree.gameObject);
            }
        }
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
