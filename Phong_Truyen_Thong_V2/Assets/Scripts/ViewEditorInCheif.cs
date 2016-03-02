using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ViewEditorInCheif : MonoBehaviour {

    public Image[] imageHold;
    
    int activeImangeHold;
    int current;

    AudioClip[] audioclips;
    Sprite[] sprites;

    public static ViewEditorInCheif intance;

    void Awake()
    {
        intance = this;
    }

    void Start () {
        current = 0;
        audioclips = AudioManager.intance.GetClipsFormName("Editor In Cheif");
        sprites = ImageManager.intance.GetSpritesFormName("Editor In Cheif");
        onNewImage();
    }

    void onNewImage()
    {
        activeImangeHold = 1 - activeImangeHold;
        imageHold[activeImangeHold].sprite = sprites[current];
        StartCoroutine(AnimateNewImage(2));
        
    }

    IEnumerator AnimateNewImage(float time)
    {
        yield return StartCoroutine(Fade(Color.clear, Color.white, time));

        AudioClip clip = audioclips[current];

        AudioManager.intance.playSoundInTime(clip, 1);

        while (AudioManager.intance.IsPlayNow())
        {
            yield return null;
        }
        current++;
        if (current < sprites.Length)
        {
            onNewImage();
        }
        
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            imageHold[activeImangeHold].color = Color.Lerp(from, to, percent);
            imageHold[1 - activeImangeHold].color = Color.Lerp(to, from, percent);

            yield return null;
        }
    }

    public IEnumerator endView()
    {
        yield return new WaitForSeconds(2);
        while(current < sprites.Length)
        {
            yield return null;
        }
            
    }
}
