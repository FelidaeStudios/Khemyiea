using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Info")]
    //public float moveSpeed;

    public int curHp;
    public int maxHp;

    //public float chaseRange;
    public float attackRange;

    public PlayerController targetPlayer;

    public float playerDetectRate = 0.2f;
    private float lastPlayerDetectTime;

    public GameObject objectToSpawnOnDeath;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    //public HealthBar healthBar;
    public SpriteRenderer sr;
    public Rigidbody2D rig;
    [SerializeField] AudioClip[] clips;

    void Start()
    {
        //healthBar.SetMaxHealth(maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hahaa");
        if (targetPlayer != null)
        {
            //Debug.Log("hahaa");
            // calculate the distance
            float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            // if we're able to attack, do so
            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                //Debug.Log("die unity");
                Attack();
            }

            // otherwise, do we move after the player?
            /*else if (dist > attackRange)
            {
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rig.velocity = dir.normalized * moveSpeed;
            }

            else
            {
                rig.velocity = Vector2.zero;
            }*/
        }
        DetectPlayer();
    }

    // attacks the targeted player
    void Attack()
    {
        //Debug.Log("boop");
        lastAttackTime = Time.time;
        targetPlayer.TakeDamage(damage);
    }

    void DetectPlayer()
    {
        if (Time.time - lastPlayerDetectTime > playerDetectRate)
        {
            lastPlayerDetectTime = Time.time;
        }
    }

    public void TakeDamage(int damage)
    {
        int index = UnityEngine.Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);
        curHp -= damage;
        // update the health bar
        //healthBar.SetHealth(curHp);
        if (curHp <= 0)
            Die();
        else
        {
            FlashDamage();
        }
    }

    void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
    }

    void Die()
    {
        if (objectToSpawnOnDeath != null)
            Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}