using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewEditorInCheif : MonoBehaviour {

    public Image[] imageGroup;
    public Image[] banerGroup;
    public RectTransform[] rectTransformGroup;

    Queue<int> One = new Queue<int>();
    Queue<int> Two = new Queue<int>();
    Queue<int> Three = new Queue<int>();
    Queue<int> Four = new Queue<int>();

    int activeImangeHold;
    int current, one, two, three, four;

    AudioClip[] audioclips;
    Sprite[] sprites;   

    public static ViewEditorInCheif intance;

    void Awake()
    {
        intance = this;
    }

    void Start () {
        Init();
        current = 0 ;
        audioclips = AudioManager.intance.GetClipsFormName("Editor In Cheif");
        sprites = ImageManager.intance.GetSpritesFormName("Editor In Cheif");
        onNewImage();
    }

    void Init()
    {
        for(int i=0; i < imageGroup.Length; i++)
        {
            One.Enqueue(i);
        }

        Two.Enqueue(1);
        Two.Enqueue(2);
        Two.Enqueue(3);
        Two.Enqueue(0);

        Three.Enqueue(2);
        Three.Enqueue(3);
        Three.Enqueue(0);
        Three.Enqueue(1);

        Four.Enqueue(3);
        Four.Enqueue(0);
        Four.Enqueue(1);
        Four.Enqueue(2);
    }

    void onNewImage()
    {
        one = (int)One.Dequeue();
        two = (int)Two.Dequeue();
        three = (int)Three.Dequeue();
        four = (int)Four.Dequeue();

        imageGroup[one].sprite = sprites[current];
        StartCoroutine(AnimateNewImage(2));

        
    }

    IEnumerator AnimateNewImage(float time)
    {
        

        
        if (current == 0)
        {
            AudioClip clip = audioclips[current];
            yield return StartCoroutine(FadeFirst(one, two, new Color(1, 1, 1, .5f), Color.white, time));
            AudioManager.intance.playSoundInTime(clip, time);
            while (AudioManager.intance.IsPlayNow())
            {
                yield return null;
            }

            yield return StartCoroutine(FadeSecond(new Color(1, 1, 1, .5f), Color.white, time));
        }
        else
        {
            yield return StartCoroutine(FadeSecond(new Color(1, 1, 1, .5f), Color.white, time));
            AudioClip clip = audioclips[current];
            AudioManager.intance.playSoundInTime(clip, time);
            while (AudioManager.intance.IsPlayNow())
            {
                yield return null;
            }

        }      
        
    }

    

    IEnumerator FadeFirst(int _one, int _two, Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        
        current++;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            imageGroup[_one].color = Color.Lerp(from, to, percent);
            banerGroup[_one].color = Color.Lerp(new Color(0.424f,.275f,.086f,.5f) , new Color(0.424f, .275f, .086f, 1), percent);
            rectTransformGroup[_one].anchoredPosition = Vector3.right * Mathf.Lerp(500, 0, percent);
            rectTransformGroup[_one].localScale = Vector3.one * Mathf.Lerp(.25f, 1, percent);

            if (current < sprites.Length)
            {
                imageGroup[_two].sprite = sprites[current];
                imageGroup[_two].color = Color.Lerp(Color.clear, from, percent);
                banerGroup[_two].color = Color.Lerp(Color.clear, new Color(0.424f, .275f, .086f, .5f), percent);
            }

            yield return null;
        }
        
    }

    IEnumerator FadeSecond(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        StartCoroutine(FadeFirst(two, three, from, to, time));
        
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;

            imageGroup[one].color = Color.Lerp(to, from, percent);
            banerGroup[one].color = Color.Lerp(new Color(0.424f, .275f, .086f, 1), new Color(0.424f, .275f, .086f, .5f), percent);
            rectTransformGroup[one].anchoredPosition = Vector3.right * Mathf.Lerp(0, -500, percent);
            rectTransformGroup[one].localScale = Vector3.one * Mathf.Lerp(1, .25f, percent);

            imageGroup[four].color = Color.Lerp(imageGroup[four].color, Color.clear, percent);
            banerGroup[four].color = Color.Lerp(banerGroup[four].color, Color.clear, percent);
            

            yield return null;
        }

        AudioClip clip = audioclips[current - 1];
        AudioManager.intance.playSoundInTime(clip, time);

        percent = 0;
        while (AudioManager.intance.IsPlayNow() || percent < 1)
        {
            percent += Time.deltaTime * speed;
            rectTransformGroup[four].anchoredPosition = Vector3.right * Mathf.Lerp(-500, 500, percent);
            yield return null;
        }

        One.Enqueue(one);
        Two.Enqueue(two);
        Three.Enqueue(three);
        Four.Enqueue(four);

        one = (int)One.Dequeue();
        two = (int)Two.Dequeue();
        three = (int)Three.Dequeue();
        four = (int)Four.Dequeue();
        if (current < sprites.Length)
        {
            StartCoroutine(FadeSecond(new Color(1, 1, 1, .5f), Color.white, time));
        }
        else
        {
            percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime * speed;

                imageGroup[one].color = Color.Lerp(to, from, percent);
                banerGroup[one].color = Color.Lerp(new Color(0.424f, .275f, .086f, 1), new Color(0.424f, .275f, .086f, .5f), percent);
                rectTransformGroup[one].anchoredPosition = Vector3.right * Mathf.Lerp(0, -500, percent);
                rectTransformGroup[one].localScale = Vector3.one * Mathf.Lerp(1, .25f, percent);

                imageGroup[four].color = Color.Lerp(imageGroup[four].color, Color.clear, percent);
                banerGroup[four].color = Color.Lerp(banerGroup[four].color, Color.clear, percent);
                yield return null;
            }
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
