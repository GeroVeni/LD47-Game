using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    // Public fields
    public float fireDelay;
    public float bulletSpeed;
    public Rigidbody2D bulletPrefab;
    public Transform gunPoint;

    // Private fields
    bool canFire;
    float fireDelayTimer;
    int bltN = 0;

    // Start is called before the first frame update
    void Start()
    {
        canFire = true;
        fireDelayTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Update fire delay timer
        if (!canFire) { fireDelayTimer += Time.deltaTime; }

        // Check if player can shoot again
        if (fireDelayTimer >= fireDelay)
        {
            fireDelayTimer = 0;
            canFire = true;
        }

        // Fire projectiles
        if (Input.GetMouseButtonDown(0))
        {
            if (canFire)
            {
                canFire = false;
                Rigidbody2D bullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
                CharacterController2D cc2d = GetComponent<CharacterController2D>();
                Vector2 shootingDirection = -transform.right;
                if (cc2d.FacingRight) shootingDirection = transform.right;
                bullet.velocity = shootingDirection * bulletSpeed;
                Debug.Log("Fire " + bltN++.ToString());
            }
        }
    }
}
