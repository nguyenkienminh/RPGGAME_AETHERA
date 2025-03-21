using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimunDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bmg;

    public bool playerBmg;
    private int bmgIndex;

    private bool canPlaySFX;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 

        Invoke("AllowSFX", 1f);
    }


    private void Update()
    {
        if (!playerBmg)
        {
            StopAllBGM();
        }else
        {
            if (!bmg[bmgIndex].isPlaying)
            {
                PlayBGM(bmgIndex);
            }
        }
    }
    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //{
        //    return;
        //}
        //Debug.Log(_sfxIndex);
        if (canPlaySFX == false)
        {
            return;
        }

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimunDistance)
        {
            return;
        }

        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();  
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void StopSFXWithTime(int _index)
    {
        if (this == null || sfx[_index] == null) return; 

        StartCoroutine(DescreaseVolume(sfx[_index]));
    }

    private IEnumerator DescreaseVolume(AudioSource _audio)
    {
        if (_audio == null) yield break;

        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            if (_audio == null) yield break;

            _audio.volume -= _audio.volume * .2f;

            yield return new WaitForSeconds(.6f);

            if(_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    } 

    public void PlayRandomBMGP()
    {
        bmgIndex = Random.Range(0, bmg.Length);
        PlayBGM(bmgIndex);
    }
    public void PlayBGM(int _bgmIndex)
    {
        bmgIndex = _bgmIndex;

        StopAllBGM();
        bmg[bmgIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bmg.Length; i++)
        {
            bmg[i].Stop();
        }
    }

    private void AllowSFX() => canPlaySFX = true;
}
