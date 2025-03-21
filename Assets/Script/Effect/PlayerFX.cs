using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{

    [Header("Screen shake Fx")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;


    [Header("After image fx")]
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    private float afterImageCooldownTimer;


    [Space]
    [SerializeField] private ParticleSystem dustFx;
    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();

    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;

    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }


    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        AudioManager.instance.PlaySFX(32, player.transform);
        screenShake.GenerateImpulse();
    }

    public void PlayDustFx()
    {
        if (dustFx != null)
        {
            dustFx.Play();
        }
    }
}
