using System;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private UI_SkillTreeSlot bounceUnclockButton;
    //public bool bounceUnclocked { get; private set; }
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnclockButton;
    //public bool pierceUnclocked { get; private set; }
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Skill Info")]
    [SerializeField] private UI_SkillTreeSlot swordUnclockButton;
    public bool swordUnclocked {  get; private set; }   
    [SerializeField] private GameObject swordPrefabs;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;
    

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnclockButton;
    //public bool spanUnclocked { get; private set; }
    [SerializeField] private float hitCooldown = .35f;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;


    [Header("Passive skills")]
    [SerializeField] private UI_SkillTreeSlot timeStopUnclockButton;
    public bool timeStopUnclocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot vulnerabilityUnclockButton;
    public bool vulnerabilityUnclocked { get; private set; }


    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;
    protected override void CheckUnlock()
    {
        UnclockSword();
        UnclockBounceSword();
        UnclockPierceSword();
        UnclockSpinSword();
        UnclockVulnerable();
        UnclockTimeStop();
    }
    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();

        swordUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockSword);
        bounceUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockBounceSword);
        pierceUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockPierceSword);
        spinUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockSpinSword);
        vulnerabilityUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockVulnerable);
        timeStopUnclockButton.GetComponent<Button>().onClick.AddListener(UnclockTimeStop);

        CheckUnlock();
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
        } else if(swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
        }
    }


    #region Unclock skill

    public void UnlockSpinSkill()
    {
        swordUnclocked = true;
        UnclockSpinSword();
    }
    public void UnlockTimeSkill()
    {
        swordUnclocked = true;
        UnclockTimeStop();
    }
    public void UnlockVulnerableSkill()
    {
        swordUnclocked = true;
        UnclockVulnerable();
    }
    public void UnlockBoundSkill()
    {
        swordUnclocked = true;
        UnclockBounceSword();
    }
    public void UnlockPierceSkill()
    {
        swordUnclocked = true;
        UnclockPierceSword();
    }
    public void UnlockSwordSkill()
    {
        swordUnclocked = true;
        UnclockSword();
    }


    private void UnclockTimeStop()
    {
        if (timeStopUnclockButton.unlocked)
        {
            timeStopUnclocked = true;
        }
    }

    private void UnclockVulnerable()
    {
        if (vulnerabilityUnclockButton.unlocked)
        {
            vulnerabilityUnclocked = true;
        }
    }

    private void UnclockSword()
    {
        if (swordUnclockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnclocked = true;
            cooldown = 7;
        }
    }

    private void UnclockBounceSword()
    {
        if (bounceUnclockButton.unlocked)
        {
           swordType =  SwordType.Bounce;
            cooldown = 9;
        }
    }

    private void UnclockPierceSword()
    {
        if (pierceUnclockButton.unlocked)
        {
            swordType = SwordType.Pierce;
            cooldown = 9;
        }
    }

    private void UnclockSpinSword()
    {
        if (spinUnclockButton.unlocked)
        {
            swordType = SwordType.Spin;
            cooldown = 9;
        }
    }
    #endregion


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {

            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefabs, player.transform.position, transform.rotation);

        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
        {
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSwordScript.SetupPierce(pierceAmount);
        } else if (swordType == SwordType.Spin)
        {
            newSwordScript.SetupSpin(true,maxTravelDistance,spinDuration,hitCooldown,returnSpeed);
        }
        newSwordScript.SetupSword(finalDir, swordGravity, player,freezeTimeDuration);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }



    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
