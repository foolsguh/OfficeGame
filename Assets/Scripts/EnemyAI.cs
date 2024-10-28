using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    bool moving;
    public float speed;

    Rigidbody2D rb;
    float tDist;

    bool attacking;
    public float attackDist;
    public float lastCooldownTime;
    public float lastAttackTime;

    private void Start()
    {
        target = FindObjectOfType<PlayerManager>().transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        tDist = Vector2.Distance(transform.position, target.position);

        if (tDist <= attackDist)
        {
            Attack();
            attacking = true;
            moving = false;
        }
        else
        {
            attacking = false;
            moving = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!moving) { rb.linearVelocity = Vector2.zero; return; }

        float xDist = (target.position.x - transform.position.x) / tDist;
        float yDist = (target.position.y - transform.position.y) / tDist;

        rb.linearVelocity = new Vector2(xDist * speed, yDist * speed);
    }

    void Attack()
    {
        if(lastAttackTime + lastCooldownTime < Time.time && attacking)
        {
            Debug.Log("kapow");
            target.GetComponent<Health>().TakeDamage(5);
            lastAttackTime = Time.time;
        }
    }
}
