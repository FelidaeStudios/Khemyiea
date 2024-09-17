using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;
    private CapsuleCollider coll;
    //private SpriteRenderer sprite;
    //private Animator anim;
    private float dirX = 0f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private LayerMask Ground;
    public float distanceToGround = 0.1f;
    private bool dblJump = true;
    private bool trplJump = true;

    private enum MovementState { idle, running, jumping, runningR }

    [Header("Attack")]
    public int damage;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    public int playerCurHp;
    public int playerMaxHp;
    public bool dead;
    //public int item;
    //public HealthBar healthBar;


    [SerializeField] AudioClip[] clips;

    private void OnMouseDown()
    {
        int index = UnityEngine.Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        //mesh = GetComponent<MeshRenderer>();
        //anim = GetComponent<Animator>();
        //healthBar.SetMaxHealth(playerMaxHp);
        playerCurHp = playerMaxHp;

    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
        else if (Input.GetButtonDown("Jump") && !IsGrounded() && dblJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            dblJump = false;
        }
        else if (Input.GetButtonDown("Jump") && !IsGrounded() && !dblJump && trplJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            trplJump = false;
        }

        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime > attackRate)
            Attack();

        //UpdateAnimationState();
    }

    /*private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX < 0f)
        {
            state = MovementState.running;
            //sprite.flipX = false;
        }

        else if (dirX > 0f)
        {
            state = MovementState.runningR;
            //sprite.flipX = true;
        }

        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }*/
        /*else if (rb.velocity.y < -.1)
        {
            state = MovementState.falling;  //add falling anim and state
        }*/

        //anim.SetInteger("state", (int)state);
    //}

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(coll.bounds.center.x, coll.bounds.min.y, coll.bounds.center.z);
        bool grounded = Physics.CheckCapsule(coll.bounds.center, capsuleBottom, distanceToGround, Ground, QueryTriggerInteraction.Ignore);
        return grounded;
    }

    void OnCollisionEnter (Collision hit)
    {
        IsGrounded();
        dblJump = true;
        trplJump = true;

        Debug.Log("Collision detected");
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        // calculate the direction
        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
        // shoot a raycast in the direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position + dir, dir, attackRange);
        // did we hit an enemy?
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy"))
        {
            // get the enemy and damage them
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            //enemy.photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
            enemy.TakeDamage(damage);
        }
        /*else if (hit.collider != null && hit.collider.gameObject.CompareTag("Boss"))
        {
            //Debug.Log("yo mama");
            Boss boss = hit.collider.GetComponent<Boss>();
            boss.TakeDamage(damage);
        }
        else if (hit.collider != null && hit.collider.gameObject.CompareTag("hsuB"))
        {
            //Debug.Log("hsuB");
            Barrier hsub = hit.collider.GetComponent<Barrier>();
            hsub.TakeDamage(damage);
        }*/
        // play attack animation
        //anim.SetTrigger("Attack");
    }

    /*public void Heal(int amountToHeal)
    {
        playerCurHp = Mathf.Clamp(playerCurHp + amountToHeal, 0, playerMaxHp);
        // update the health bar
        healthBar.SetHealth(playerCurHp);
    }

    public void GiveItem(int itemToGive, int damageIncrease)
    {
        item += itemToGive;
        damage += damageIncrease;
        // update the ui
        GameUI.instance.UpdateItemText(item);
    }*/

    public void TakeDamage(int damage)
    {
        Debug.Log("HP before damage: " + playerCurHp);
        playerCurHp -= damage;
        // update the health bar
        //healthBar.SetHealth(playerCurHp);
        Debug.Log(damage + " damage taken");
        Debug.Log(playerCurHp + " hp remaining");
        if (playerCurHp <= 0)
        {
            Debug.Log("Death");
            Die();
        }
        /*else
        {
            FlashDamage();
        }*/
        playerCurHp = Mathf.Clamp(playerCurHp, 0, playerMaxHp);
    }

    /*void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            mesh.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            mesh.color = Color.white;
        }
    }*/

    private void Die()
    {
        //Debug.Log("OwO");
        //rb.bodyType = RigidbodyType.Static;
        Destroy(gameObject);
        RestartLevel();
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
    }*/
}