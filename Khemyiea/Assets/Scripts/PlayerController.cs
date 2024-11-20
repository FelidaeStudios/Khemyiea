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
    private Animator anim;
    private float dirX = 0f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask jumpableWall;
    public float distanceToGround = 0.1f;
    public float startDashTime;
    public float dashTime;
    private bool dblJump = true;
    private bool trplJump = true;
    private bool dash = false;


    private enum MovementState { idle, running, jumping, runningR }

    [Header("Attack")]
    public int damage;
    public float attackRange;
    public float attackRate;
    private float lastAttackTime;
    public int playerCurHp;
    public int playerMaxHp;
    public bool dead;
    public int hazardDMG;
    //public GameObject rock; // Reference to the attack object (e.g., projectile)
    //public Transform attackSpawnPoint;
    //public float rockSpeed = 100f;

    public HealthBar healthBar;

    [Header("Puzzles")]
    public int proton;
    public int neutron;
    public int electron;
    public int winKey;

    [Header("UI")]
    public GameObject gameOverScreen;
    public GameObject winScreen;


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
        anim = GetComponent<Animator>();
        healthBar.SetMaxHealth(playerMaxHp);
        playerCurHp = playerMaxHp;

    }

    private void Update()
    {
        //dirX = Input.GetAxisRaw("Horizontal");
        //rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        Moving();

        /*if (Input.GetKeyDown("left shift"))
        {
            DashReset();
        }*/
        if (!dash)
        {
            PlayerDash();
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if (Input.GetButtonDown("Jump") && TouchingWall() !=0 && !IsGrounded())
        {
            Debug.Log("Wall jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);

            if (TouchingWall() == -1)
            {
                rb.AddForce(new Vector2(wallJumpForce, 0), ForceMode2D.Impulse);
            }

            else if (TouchingWall() == 1)
            {
                rb.AddForce(new Vector2(-wallJumpForce, 0), ForceMode2D.Impulse);
            }

            dblJump = true;
            trplJump = true;
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

        UpdateAnimationState();
    }

    private void Moving()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }

        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }

        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping; //make one
        }
        /*else if (rb.velocity.y < -.1)
        {
            state = MovementState.falling;  //add falling anim and state
        }*/

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private int TouchingWall()
    {
        RaycastHit2D wallHitLeft = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.left, .2f, jumpableWall);
        RaycastHit2D wallHitRight = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.right, .2f, jumpableWall);

        Debug.DrawRay(coll.bounds.center, Vector2.left * .2f, Color.red); // For left wall
        Debug.DrawRay(coll.bounds.center, Vector2.right * .2f, Color.red); // For right wall
        //bool isTouchingWall;
        if (wallHitLeft.collider != null)
        {
            //isTouchingWall = true;
            //Debug.Log("Hit left wall collider: " + wallHitLeft.collider.gameObject.name);
            //Debug.Log("Left wall");
            return -1; //Touching left wall
        }

        else if(wallHitRight.collider != null)
        {
            //isTouchingWall = true;
            //Debug.Log("Hit right wall collider: " + wallHitRight.collider.gameObject.name);
            //Debug.Log("Right wall");
            return 1; //Touching right wall
        }
        else
        {
            //isTouchingWall = false;
            //Debug.Log("Not touching a wall");
            return 0; //Not touching a wall
        }
    }

    void OnCollisionEnter2D (Collision2D hit)
    {
        if (IsGrounded())
        {
            dblJump = true;
            trplJump = true;
        }

        else if (!IsGrounded())
        {
            //Debug.Log("Collision detected" + TouchingWall());
            //Debug.Log("Touching wall: " + TouchingWall());
        }
        //Debug.Log("Collision detected");
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
        else if (hit.collider != null && hit.collider.gameObject.CompareTag("Boss"))
        {
            //Debug.Log("yo mama");
            Boss boss = hit.collider.GetComponent<Boss>();
            boss.TakeDamage(damage);
        }
        /*else if (hit.collider != null && hit.collider.gameObject.CompareTag("hsuB"))
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
        healthBar.SetHealth(playerCurHp);
    }

    public void GiveProton(int protonToGive)
    {
        proton += protonToGive;
        //damage += damageIncrease;
        // update the ui
        GameUI.instance.UpdateProtonText(proton);
        Debug.Log("Proton collected");
    }

    public void GiveNeutron(int neutronToGive)
    {
        neutron += neutronToGive;
        //damage += damageIncrease;
        // update the ui
        GameUI.instance.UpdateNeutronText(neutron);
    }

    public void GiveElectron(int electronToGive)
    {
        electron += electronToGive;
        //damage += damageIncrease;
        // update the ui
        GameUI.instance.UpdateElectronText(electron);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("HP before damage: " + playerCurHp);
        playerCurHp -= damage;
        // update the health bar
        healthBar.SetHealth(playerCurHp);
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
        gameOverScreen.SetActive(true);
        Destroy(gameObject);
        //Time.timeScale = 0f;

        //gameOver = true;
        
        //RestartLevel();
    }

    //Commented out for game systems class
    /*private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "GroundHazard")
        {
            playerCurHp -= hazardDMG;
        }
    }

    /*private void DashReset()
    {
        StartCoroutine(Dash());
        IEnumerator Dash()
        {
            if (dash)
            {
                rb.velocity = new Vector2(dirX * dashForce, rb.velocity.y);
                dash = false;
                yield return new WaitForSeconds(1.5f);
                dash = true;
            }
            else
            {
                Debug.Log("Too soon to dash");
            }
        }
    }*/

    void PlayerDash()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 direction = input.normalized;
        Vector2 velocity = direction * dashForce;
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dash)
        {
            dash = true;
            rb.velocity = velocity;
            startDashTime = Time.time;
        }
    }

    public void WinGame(int giveWinKey)
    {
        winKey += giveWinKey;
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }

    /*private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
    }*/
}