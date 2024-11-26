using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public SpriteRenderer sr;
    public int curHp;
    public int maxHp;
    public GameObject objectToSpawnOnDeath;
    //public HealthBar healthBar;

    void Start()
    {
        //healthBar.SetMaxHealth(maxHp);
    }

    public void TakeDamage(int damage)
    {
        /*
        int index = UnityEngine.Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
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
        if (objectToSpawnOnDeath != null)
            Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
