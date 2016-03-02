using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    AudioSource[] musicSource;
   
    public float masterVolumePercent { get; private set; }

    public static AudioManager intance;

    SoundLibrary libirary;

    [HideInInspector]
    public bool isMove = true;

    Transform audioListerner;
    Transform playerT;

    void Awake()
    {
        if (intance != null)
        {
            DestroyObject(gameObject);
        }
        else
        {
            intance = this;

            DontDestroyOnLoad(gameObject);

            musicSource = new AudioSource[2];

            for (int i = 0; i < musicSource.Length; i++)
            {
                GameObject newMusicSource = new GameObject(" Music Source " + (i + 1));
                musicSource[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }
            libirary = GetComponent<SoundLibrary>();

            audioListerner = FindObjectOfType<AudioListener>().transform;

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1);
        }

        
    }

    void Update()
    {
        if (FindObjectOfType<Player>() != null &&playerT==null)
        {
            playerT = FindObjectOfType<Player>().transform;
        }
        if (playerT != null)
        {
            audioListerner.position = playerT.position;
        }
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            musicSource[1].clip = clip;
           AudioSource.PlayClipAtPoint(musicSource[1].clip, pos, masterVolumePercent);
           
           StartCoroutine(AnimateMusicCrossfade(musicSource[1], 2));

        }
    }

    public void PlayMusic(AudioClip clip)
    {
        
        musicSource[0].clip = clip;
        musicSource[0].Play();
    }

     public void playSoundInTime(AudioClip clip, float fadeDuration = 1)
    {   
        musicSource[1].clip = clip;
        musicSource[1].Play();
        StartCoroutine(AnimateMusicCrossfade(musicSource[1],fadeDuration));
    }

    public AudioClip[] GetClipsFormName(string name)
    {
       return libirary.GetClipsFormName(name);
    }

    IEnumerator AnimateMusicCrossfade(AudioSource sound, float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSource[0].volume = Mathf.Lerp(musicSource[0].volume , .2f, percent);
            yield return null;
        }

        while (sound.isPlaying)
        {
            yield return null;
        }

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSource[0].volume = Mathf.Lerp(musicSource[0].volume, masterVolumePercent, percent);
    
            yield return null;
        }
    }

    public bool IsPlayNow()
    {
        return musicSource[1].isPlaying;
    }

    public float AudioTime()
    {
        return musicSource[1].time;

        
    }

    public float LenghtSound()
    {
        return musicSource[1].clip.length;
    }

    public void SetVolum(float volumPercent)
    {

        masterVolumePercent = volumPercent;


        musicSource[0].volume = masterVolumePercent;
        musicSource[1].volume = masterVolumePercent;

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.Save();
    }

    public void StopSound()
    {
        musicSource[1].Stop();
    }
}
