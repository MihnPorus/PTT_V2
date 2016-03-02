using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroduceView : MonoBehaviour {

    public Image[] imageHolder;

    int activeImangeHold;

    AudioClip audioclip;
    Sprite[] sprites;

    

    void Awake()
    {
        audioclip = AudioManager.intance.GetClipsFormName("Introduce")[0];
        sprites=ImageManager.intance.GetSpritesFormName("Introduce");
        

        AudioManager.intance.playSoundInTime(audioclip, 1);
        StartCoroutine(IntroducePlay());

    }

    IEnumerator IntroducePlay()
    {
        float percent = 0;
        int currentTime = 0;
        int spriteindex = 0;
        while (percent <= audioclip.length)
        {

            percent = AudioManager.intance.AudioTime();
            if (percent >= currentTime && spriteindex < sprites.Length)
            {
                activeImangeHold = 1 - activeImangeHold;
                imageHolder[activeImangeHold].sprite= sprites[spriteindex++];
                StartCoroutine(Fade(Color.clear, Color.white, 2));
                currentTime += Mathf.RoundToInt(audioclip.length / sprites.Length);
            }
            yield return null;
        }
        
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            imageHolder[activeImangeHold].color = Color.Lerp(from, to, percent);
            imageHolder[1- activeImangeHold].color = Color.Lerp(to, from, percent);
            
            yield return null;
        }
    }
}
