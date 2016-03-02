﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject PlayMenuHolder;
    public GameObject introduceViewHolder;

    public Slider volumeSliders;
    public Toggle[] resolutionToggles;
    public int[] screenWidths;
    int activeScreenResIndex;

    public AudioClip SoundPlay;

    void Start()
    {

        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        volumeSliders.value = AudioManager.intance.masterVolumePercent;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].isOn = i == activeScreenResIndex;

        }
        SetFullscreen(isFullscreen);
    }

    public void Play()
    {
        //PlayMenuHolder.SetActive(true);
        //mainMenuHolder.SetActive(false);

        //AudioManager.intance.playSoundInTime(SoundPlay, 2);
        Move();

    }

    public void Quit()
    {
        Application.Quit();

    }

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    public void AutoMoving()
    {
        mainMenuHolder.SetActive(false);
        introduceViewHolder.SetActive(true);
        StartCoroutine(NextSecence(true));
    }

    public void SetMaterVolume(float value)
    {
        AudioManager.intance.SetVolum(value);
    }

    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(screenWidths[i], (int)(screenWidths[i] / aspectRatio), false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles[i].interactable = !isFullscreen;

        }

        if (isFullscreen)
        {
            Resolution[] allResolution = Screen.resolutions;
            Resolution maxResolution = allResolution[allResolution.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            SetScreenResolution(activeScreenResIndex);
        }

        PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void Move()
    {
        mainMenuHolder.SetActive(false);
        introduceViewHolder.SetActive(true);
        StartCoroutine(NextSecence(false));
    }

    IEnumerator NextSecence(bool ismove)
    {
        while (AudioManager.intance.IsPlayNow())
        {
            yield return null;
        }
        SceneManager.LoadScene("PhongTruyenThong");
        AudioManager.intance.isMove = ismove;
    }
}
