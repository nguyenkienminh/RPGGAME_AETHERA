using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;

    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotkey = true;
    private bool cloneAttackReleased;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotKey = new List<GameObject>();


    public bool playerCanExitState { get; private set; }

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;

      

    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
        {
            PlayerManager.instance.player.fx.MakeTransparent(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackHoleAbility();
            }
        }

        CloneAttackLogic();

        if (canGrow & !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotkey = false;

        PlayerManager.instance.player.fx.MakeTransparent(true);
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int random = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 1;
            }
            else
            {
                xOffset = -1;
            }

            if (SkillManager.instance.cloneSkill.crystalInsteadOfClone)
            {
                SkillManager.instance.crystalSkill.CreateCrystal();
                SkillManager.instance.crystalSkill.CurrentCrystalChooseRandomTarget();

            }
            else
            {
                SkillManager.instance.cloneSkill.CreateClone(targets[random], new Vector3(xOffset, 0));

            }

            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility", 1f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;



    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(true);

            CreateHotkey(collision);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTimer(false);
        }
    }
    private void DestroyHotKeys()
    {
        if (createHotKey.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < createHotKey.Count; i++)
        {
            Destroy(createHotKey[i]);
        }
    }
    private void CreateHotkey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.Log("Not enough hot key in the list");
            return;
        }

        if (!canCreateHotkey)
        {
            return;
        }

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createHotKey.Add(newHotkey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_Hotkey_Controller newHotKeyScript = newHotkey.GetComponent<Blackhole_Hotkey_Controller>();
        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void addEnemyToList(Transform _EnemyTransform) => targets.Add(_EnemyTransform);
}
