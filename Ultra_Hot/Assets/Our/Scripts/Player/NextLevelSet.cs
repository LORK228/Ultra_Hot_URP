using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextLevelSet : MonoBehaviour
{
    [SerializeField] public int enimesCount;

    [HideInInspector]public int levelIndex;

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>();
        levelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (enimesCount == 0)
        {
            PlayerPrefs.SetInt("SaveGame", levelIndex);
            Time.timeScale = 0;
            text.text = "Уровень закончен, продолжить?";
            text.enabled = true;
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("SaveGame") +1);
            }
        }
    }
}
