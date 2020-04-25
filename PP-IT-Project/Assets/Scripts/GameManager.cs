﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    
    [SerializeField] private Image fader;
    private Text ScoreTXT;

    // Start is called before the first frame update
    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ReloadScene()
    {
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 0);
        StopCoroutine("FadeReload");
        StartCoroutine("FadeReload");

    }
    IEnumerator FadeReload()
    {
        yield return new WaitForSeconds(1);

        fader.gameObject.SetActive(true);

        float startTime = Time.time;
        float duration = 0.2f;

        while(Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        startTime = Time.time;   
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 0);

    }
}
