using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    PlayerManager player;
    CircleCollider2D circleCollider;
    public Collider2D[] xpInRadius;
    public float xpSpeed;

    private void Start()
    {
        player = GetComponentInParent<PlayerManager>();
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void Update()
    {
        xpInRadius = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);

        foreach (var xp in xpInRadius)
        {
            if(xp.GetComponent<XP_Script>() || xp.GetComponent<HealItem>())
            {
                float distToXP = Vector2.Distance(xp.transform.position, transform.position);

                if (distToXP < .1f)
                {
                    CollectXP(xp.gameObject);
                }

                float speed = xpSpeed * (2 * circleCollider.radius - distToXP) * Time.deltaTime;

                xp.transform.position = Vector2.MoveTowards(xp.transform.position, transform.position, speed);
            }
        }
    }

    void CollectXP(GameObject xp)
    {
        if (xp.GetComponent<XP_Script>())
        {
            player.xpIncrease(xp.GetComponent<XP_Script>().xpValue);
            Destroy(xp.gameObject);
            FindFirstObjectByType<SoundManager>().PlaySound("pickup");
        }
        else if (xp.GetComponent<HealItem>())
        {
            Health health = player.GetComponent<Health>();

            health.currentHealth += xp.GetComponent<HealItem>().healthToHeal;
            health.currentHealth = Mathf.Clamp(health.currentHealth, 0, health.maxHealth);

            health.UpdateHealthBar();

            Destroy(xp.gameObject);
            FindFirstObjectByType<SoundManager>().PlaySound("pickup");
        }
    }

    // when we add an ability that uses this
    public float sizeChange(float valueChange) {
        GetComponent<CircleCollider2D>().radius += valueChange;
        return GetComponent<CircleCollider2D>().radius;
    }
}
