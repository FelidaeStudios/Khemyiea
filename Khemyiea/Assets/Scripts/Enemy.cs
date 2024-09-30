using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Info")]
    public float moveSpeed;

    public int curHp;
    public int maxHp;

    public float chaseRange;
    public float attackRange;

    public PlayerController targetPlayer;
    //public GameObject player;

    public float playerDetectRate = 0.2f;
    private float lastPlayerDetectTime;

    //public GameObject objectToSpawnOnDeath;
    //public GameObject enemy;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    //public HealthBar healthBar;
    public Rigidbody2D rig;
    public SpriteRenderer sr;
    [SerializeField] AudioClip[] clips;

    void Start()
    {
        //healthBar.SetMaxHealth(maxHp);
    }

    void Update()
    {
        //Debug.Log("meeple murple");
        if (targetPlayer != null)
        {
            //Debug.Log("hahaa");
            // calculate the distance
            float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            // if we're able to attack, do so
            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                Debug.Log("die unity");
                Attack();
            }

            // otherwise, do we move after the player?
            else if (dist > attackRange)
            {
                //Debug.Log("Hello");
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rig.velocity = dir.normalized * moveSpeed;
            }

            else
            {
                //Debug.Log("Not moving");
                rig.velocity = Vector3.zero;
            }
        }
        DetectPlayer();
    }

    // attacks the targeted player
    void Attack()
    {
        Debug.Log("Attack for " + damage + " damage");
        lastAttackTime = Time.time;
        Debug.Log("Player takes damage");
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
        /*AudioClip clip = clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);*/
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
        //if (objectToSpawnOnDeath != null)
            //Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);
        gameObject.SetActive(false);

        /*StartCoroutine(EnemyRespawn());
        IEnumerator EnemyRespawn()
        {
            yield return new WaitForSeconds(10);
            Respawn();
        }*/
        //Invoke("Respawn", 2);
    }

    /*void Respawn()
    {
        GameObject enemyClone = (GameObject)Instantiate(enemy);
        enemyClone.transform.position = transform.position;
        enemyClone.SetActive(true);

        Destroy(gameObject);
    }*/

}