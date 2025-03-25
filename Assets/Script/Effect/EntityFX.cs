using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer sr;

    [Header("Pop up text")]
    [SerializeField] private GameObject popUpTextPrefabs;





    [Header("Flash FX")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float FlashDuration;
    private Material originalMaterial;

    [Header("Ailment colors")]
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    [Header("Parliment particles")]
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit Fx")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject hitCriticalFx;
    //private GameObject myHealthBar;
    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMaterial = sr.material;
        player = PlayerManager.instance.player;

        //myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
    }

    public void CreatePopupText(string _text)
    {
        Vector3 positionOffset = new Vector3(0, 3, 0);

        GameObject newText = Instantiate(popUpTextPrefabs, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }




    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            //myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            //myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }

    //private IEnumerator FlashFX()
    //{
    //    sr.material = hitMaterial;

    //    yield return new WaitForSeconds(FlashDuration);

    //    sr.material = originalMaterial;
    //}


    private IEnumerator FlashFX()
    {
        sr.color = Color.red; // Xanh nhạt

        yield return new WaitForSeconds(FlashDuration);
        sr.color = Color.white;

    }


    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    public void IgniteFxFor(float _second)
    {
        igniteFx.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    public void ShockFxFor(float _second)
    {
        shockFx.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    public void ChillFxFor(float _second)
    {
        chillFx.Play();

        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }
    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
        {
            sr.color = chillColor[0];
        }
        else
        {
            sr.color = chillColor[1];
        }
    }

    public void CreateHitFx(Transform _target, bool _critical)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if (_critical)
        {
            hitPrefab = hitCriticalFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
            {
                yRotation = 180;
            }

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);


        Destroy(newHitFx, .5f);
    }



}
