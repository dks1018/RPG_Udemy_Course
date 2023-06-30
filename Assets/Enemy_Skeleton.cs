using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Entity
{
    bool isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D isPlayerDetected;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        // Conduct a collision check every frame
        CollisionCheck();

        // Check is player is detected
        if (isPlayerDetected.collider != null)
        {
            if (isPlayerDetected.distance > 1)
            {
                Debug.Log("I see the player");
                isAttacking = false;
                Movement(moveSpeed * 2f); // double the moveSpeed when the player is detected
            }
            else
            {
                Debug.Log("Attack! " + isPlayerDetected);
                isAttacking = true;
            }
        }
        else
        {
            // If the player isn't detected, the skeleton should move at normal speed
            isAttacking = false;
            Movement(moveSpeed);
        }

        if (!isGrounded || isWallDetected)
        {
            Flip();
        }
    }

    private void Movement(float speed)
    {
        if (!isAttacking)
        {
            rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
        }

    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}
