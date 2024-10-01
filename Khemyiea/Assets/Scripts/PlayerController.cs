using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    //private Animator anim;
    private float dirX = 0f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;
    public float distanceToGround = 0.1f;
    private bool dblJump = true;
    private bool trplJump = true;
    private bool isTouchingWall;

    private enum MovementState { idle, running, jumping, runningR }

    [Header("Attack")]
    public int damage;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    public int playerCurHp;
    public int playerMaxHp;
    public bool dead;
    //public GameObject rock; // Reference to the attack object (e.g., projectile)
    //public Transform attackSpawnPoint;
    //public float rockSpeed = 100f;

    //public HealthBar healthBar;

    [Header("Puzzles")]
    public int key;


    [SerializeField] AudioClip[] clips;

    private void OnMouseDown()
    {
        int index = UnityEngine.Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
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

        if (Input.GetButtonDown("Jump") && isTouchingWall && !IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            int wallSide = TouchingWall();

            if(wallSide == -1)
            {
                rb.AddForce(new Vector2(wallJumpForce, 0), ForceMode2D.Impulse);
            }

            else if(wallSide == 1)
            {
                rb.AddForce(new Vector2(-wallJumpForce, 0), ForceMode2D.Impulse);
            }
        }

        /*if (transform.localScale.x > 0)
        {
            facingDir = 1;  // Facing right
        }
        else if (transform.localScale.x < 0)
        {
            facingDir = -1; // Facing left
        }*/


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
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private int TouchingWall()
    {
        RaycastHit2D wallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, 1f, jumpableWall);
        RaycastHit2D wallHitRight = Physics2D.Raycast(transform.position, Vector2.right, 1f, jumpableWall);

        if (wallHitLeft.collider != null)
        {
            isTouchingWall = true;
            return -1; //Touching left wall
        }

        else if(wallHitRight.collider != null)
        {
            isTouchingWall = true;
            return 1; //Touching right wall
        }
        else
        {
            isTouchingWall = false;
            return 0; //Not touching a wall
        }
    }

    void OnCollisionEnter2D (Collision2D hit)
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

    public void Heal(int amountToHeal)
    {
        playerCurHp = Mathf.Clamp(playerCurHp + amountToHeal, 0, playerMaxHp);
        // update the health bar
        //healthBar.SetHealth(playerCurHp);
    }

    public void GiveKey(int keyToGive)
    {
        key += keyToGive;
        //damage += damageIncrease;
        // update the ui
        //GameUI.instance.UpdateKeyText(key);
    }

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
        else
        {
            FlashDamage();
        }
        playerCurHp = Mathf.Clamp(playerCurHp, 0, playerMaxHp);
    }

    void FlashDamage()
    {
        StartCoroutine(DamageFlash());
        IEnumerator DamageFlash()
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sprite.color = Color.white;
        }
    }

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