using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider bar;
    [SerializeField] private TextMeshProUGUI loadingText;

    public void New()
    {
        PlayerPrefs.SetInt("SaveGame", 0);
    }


    public void Load(int index)
    {
        loadingScreen.SetActive(true);

        StartCoroutine(LoadAsync(index));
    }

    public void LoadNew()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsyncNew());
    }
    IEnumerator LoadAsyncNew()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("SaveGame") + 1);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            bar.value = asyncLoad.progress;
            if (asyncLoad.progress >= .9f && !asyncLoad.allowSceneActivation)
            {
                loadingText.text = "Нажмите любую кнопку";
                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    IEnumerator LoadAsync(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            bar.value = asyncLoad.progress;
            if(asyncLoad.progress >= .9f && !asyncLoad.allowSceneActivation)
            {
                loadingText.text = "Нажмите любую кнопку";
                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
