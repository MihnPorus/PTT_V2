using UnityEngine;
using System.Collections;

public class MainPlayer : MonoBehaviour {

    public GameObject PlayerHold1;
    public GameObject PlayerHold2;

    public AudioClip startVoice;


    void Start()
    {
        if (!AudioManager.intance.isMove)
        {
            PlayerHold2.SetActive(true);
            PlayerHold1.SetActive(false);
        }
        else
        {
            PlayerHold2.SetActive(false);
            PlayerHold1.SetActive(true);

        }
        Invoke("PlaySound", 0.5f);
    }

    void PlaySound()
    {
        AudioManager.intance.playSoundInTime(startVoice, 2);
    }


}
