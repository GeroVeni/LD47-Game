using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flaree : MonoBehaviour
{
    public FlareeState state;

    public float roamTurnDelayMin;
    public float roamTurnDelayMax;
    public float roamSpeed;

    public float huntSpeed;
    public float attackDelay;
    public float attackReadyTime;
    public float attackRange;
    public Vector2 launchForce;
    public LayerMask whatIsPlayer;
    public Transform eyes;

    Vector2 roamDirection;
    float roamTurnDelay;
    float roamTurnDelayTimer;
    float attackDelayTimer;

    private bool m_Grounded;
    private bool m_Launched;
    const float k_GroundedRadius = .2f;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;

    public UnityEvent OnLandEvent;

    GameObject player;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        roamDirection = transform.right;
        roamTurnDelay = Random.Range(roamTurnDelayMin, roamTurnDelayMax);
        roamTurnDelayTimer = 0;
        attackDelayTimer = 0;

        m_Launched = false;

        if (OnLandEvent == null) { OnLandEvent = new UnityEvent(); }
        OnLandEvent.AddListener(OnLand);
    }

    bool CanSeePlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyes.position, roamDirection, Mathf.Infinity, whatIsPlayer);
        return hit.collider != null;
    }

    void OnLand()
    {
        if (state == FlareeState.LAUNCH)
        {
            state = FlareeState.HUNT;
            Flip();
            attackDelayTimer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        switch (state)
        {
            case FlareeState.PEACEFULL:
                Roam(false);
                break;
            case FlareeState.HUNT:
                Hunt();
                break;
            case FlareeState.LAUNCH:
                Launch();
                break;
        }
    }

    void Launch()
    {
        if (!m_Launched)
        {
            rb.AddForce(new Vector2(launchForce.x * roamDirection.x, launchForce.y), ForceMode2D.Impulse);
            m_Launched = true;
        }
    }

    void Hunt()
    {
        if (attackDelayTimer < attackDelay) { attackDelayTimer += Time.deltaTime; }
        bool canSeePlayer = CanSeePlayer();
        if (attackDelayTimer >= attackDelay && canSeePlayer && GetPlayerDist() > attackRange)
        {
            state = FlareeState.LAUNCH;
            m_Launched = false;
        }
        else { Roam(canSeePlayer); }
    }

    float GetPlayerDist()
    {
        return Mathf.Abs(player.transform.position.x - transform.position.x);
    }

    void Roam(bool canSeePlayer)
    {
        roamTurnDelayTimer += Time.deltaTime;
        if (roamTurnDelayTimer > roamTurnDelay && !canSeePlayer)
        {
            Flip();
        }

        float velocityX = roamDirection.x * roamSpeed;
        if (GetPlayerDist() < 0.2f) { velocityX = 0; }
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    void Flip()
    {
        roamDirection = -roamDirection;

        // Multiply the entity's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // Reset timer
        roamTurnDelay = Random.Range(roamTurnDelayMin, roamTurnDelayMax);
        roamTurnDelayTimer = 0;
    }

    public enum FlareeState
    {
        PEACEFULL,
        LAUNCH,
        HUNT
    }
}
