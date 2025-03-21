using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private float returnSpeed = 35;
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;


    private bool canRotate = true;
    private bool isReturning;

    private float freezeeTimeDuration;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;


    [Header("Bouncing info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;


    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();


    }
    private void DestroyMe()
    {
        Destroy(gameObject);
    }
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration)
    {
        player = _player;
        freezeeTimeDuration = _freezeTimeDuration;
        if (rb != null)
        {
            rb.linearVelocity = _dir;
            rb.gravityScale = _gravityScale;
        }
        else
        {
            Debug.LogError("Rigidbody2D is not assigned in  word_Skill_Controller.");
        }

        if (pierceAmount <= 0)
        {
            animator.SetBool("Rotation", true);
        }

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
        enemyTarget = new List<Transform>();
    }
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown, float _returnSpeed)
    {
        isSpinning = _isSpinning;
        returnSpeed = _returnSpeed;
        spinDuration = _spinDuration;
        maxTravelDistance = _maxTravelDistance;
        hitCooldown = _hitCooldown;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.linearVelocity;
        }

        if (isReturning)
        {
            animator.SetBool("Rotation", true);

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < .5f)
            {
                player.CatchTheSword();
            }
        }

        BounceLogic();

        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;

                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            //hit.GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", freezeeTimeDuration);
                            //hit.GetComponent<Enemy>().Damage();
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }

            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            if (enemyTarget[targetIndex] == null)
            {
                enemyTarget.RemoveAt(targetIndex);

                if (enemyTarget.Count == 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    return;
                }

                targetIndex = targetIndex % enemyTarget.Count;
            }

            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                //enemyTarget[targetIndex].GetComponent<Enemy>().Damage();
                //enemyTarget[targetIndex].GetComponent<Enemy>().StartCoroutine("FreezeTimerFor", freezeeTimeDuration);

                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }

        if (collision.GetComponent<Enemy>() != null && !isSpinning && !isBouncing)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(collision);

        StuckInto(collision);


    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();

        player.chacracterStats.DealTotalDamage(enemyStats, false);
        AudioManager.instance.PlaySFX(28, null);

        if (player.skill.swordSkill.timeStopUnclocked)
        {
            enemy.FreezeTimeFor(freezeeTimeDuration);
        }


        if (player.skill.swordSkill.vulnerabilityUnclocked)
        {
            enemyStats.MakeVulnerableFor(freezeeTimeDuration);
        }


        ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipedAmulet != null)
        {
            equipedAmulet.Effect(enemy.transform);
        }
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {

                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }


        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();

        if (isBouncing && enemyTarget.Count > 0)
            return;
        AudioManager.instance.PlaySFX(28, null);

        animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
