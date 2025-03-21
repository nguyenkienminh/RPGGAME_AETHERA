using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator animator => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.BlackholeSkill.GetBlackholeRadius();
        Collider2D[] physic = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (physic.Length > 0)
        {
            closestTarget = physic[Random.Range(0, physic.Length)].transform;

        }

    }

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget,Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void Update()
    {
        if (crystalExistTimer > 0)
        {
            crystalExistTimer -= Time.deltaTime;
            if (crystalExistTimer < 0)
            {
                FinishCrystal();
            }
        }

        if (canMove && closestTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
            else if (canMove && closestTarget == null)
            {
                FinishCrystal();
                canMove = false;
            }
        }


        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);


                player.chacracterStats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                AudioManager.instance.PlaySFX(49, transform);
                ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if(equipedAmulet != null)
                {
                    equipedAmulet.Effect(hit.transform);
                }
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}