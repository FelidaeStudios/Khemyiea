using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundHazard : MonoBehaviour
{
    [Header("Info")]
    public int curHp;
    public int maxHp;
    public PlayerController targetPlayer;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    public SpriteRenderer sr;
    //[SerializeField] AudioClip[] clips;

    void Start()
    {
        
    }

    void Update()
    {
        //OnCollisionEnter2D();
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        if (targetPlayer != null)
        {
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                if(Time.time - lastAttackTime >= attackRate)
                {
                    Attack();
                }
            }
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        targetPlayer.TakeDamage(damage);
    }
}
