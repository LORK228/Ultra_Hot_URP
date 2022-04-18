using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLockedLevel : MonoBehaviour
{
    [SerializeField] private int indexOflevel;
    private void OnEnable()
    {
        if (indexOflevel <= PlayerPrefs.GetInt("SaveGame") + 1)
        {
            gameObject.SetActive(false);
        }
    }   
}
