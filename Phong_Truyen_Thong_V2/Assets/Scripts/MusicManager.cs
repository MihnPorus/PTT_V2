using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

    public AudioClip mainTheme;

    string sceneName;

    void Awake()
    {
        OnLevelWasLoaded(0);
    }


    void OnLevelWasLoaded(int sceneIndex)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

    void PlayMusic()
    {
        AudioClip clipToPlay_1 = null;

        if (sceneName == "Menu")
        {
            clipToPlay_1 = mainTheme;
            if (clipToPlay_1 != null)
            {
                AudioManager.intance.PlayMusic(clipToPlay_1);
                Invoke("PlayMusic", clipToPlay_1.length);
            }
        }
        else if (sceneName == "PhongTruyenThong")
        {
            clipToPlay_1 = mainTheme;
            if (clipToPlay_1 != null )
            {
                AudioManager.intance.PlayMusic(clipToPlay_1);
                Invoke("PlayMusic", clipToPlay_1.length);
            }                       
        }
    }

}
