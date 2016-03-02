using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SWS;

public class GameUI : MonoBehaviour {

    public Image fadePlane;
    public GameObject gameOverUI;

    public static GameUI intance;
    void Start()
    {
        intance = this;
    }

    public void disGameover()
    {
        Cursor.visible = false;
        StartCoroutine(Fade(Color.white, Color.clear, 1));
        gameOverUI.SetActive(false);
    }
    public void onGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, Color.white, 1));
        gameOverUI.SetActive(true);
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    public void onAgain()
    {
        FindObjectOfType<bezierMove>().onAgain();
    }

    public void onMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
