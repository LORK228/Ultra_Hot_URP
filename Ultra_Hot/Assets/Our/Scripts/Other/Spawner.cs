using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _MeleeTypeOfEnimes;
    [SerializeField] private GameObject[] _ShotTypeOfEnimes;

    [Header("bools")]
    [SerializeField] private bool _MeleeEnimes;
    [SerializeField] private bool _ShotEnimes;

    private List<Transform> spawners;
    private List<bool> aboba;
    private List<GameObject> _allTypeOfEnimes;
    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.1f);
    }
    private void Awake()
    {
        if (!_ShotEnimes && !_MeleeEnimes)
        {
            Destroy(gameObject);
        }

        _allTypeOfEnimes = new List<GameObject>();
        for (int i = 0; i < _MeleeTypeOfEnimes.Length; i++)
        {
            _allTypeOfEnimes.Add(_MeleeTypeOfEnimes[i]);
        }

        for (int i = 0; i < _ShotTypeOfEnimes.Length; i++)
        {
            _allTypeOfEnimes.Add(_ShotTypeOfEnimes[i]);
        }

        spawners = new List<Transform>();
        aboba = new List<bool>();
        spawners.AddRange(GetComponentsInChildren<Transform>());
        spawners.Remove(transform);

        GameObject[] b = GameObject.FindGameObjectsWithTag("Debil");
        for (int i = 0; i < b.Length; i++)
        {
            aboba.Add(b[i].GetComponent<AI>().enabled == false);
            if (b[i].GetComponent<AI>().enabled == true)
            {
                aboba.Clear();
                break;
            }
        }

        if (GameObject.FindGameObjectsWithTag("Debil").Length == 0 || aboba.Count != 0)
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                StartCoroutine(Wait());
                if (_MeleeEnimes && !_ShotEnimes)
                {
                    Instantiate(_MeleeTypeOfEnimes[Random.Range(0, _MeleeTypeOfEnimes.Length)], spawners[i].position, Quaternion.identity);
                }
                else if (_ShotEnimes && !_MeleeEnimes)
                {
                    Instantiate(_ShotTypeOfEnimes[Random.Range(0, _ShotTypeOfEnimes.Length)], spawners[i].position, Quaternion.identity);
                }
                else if (_MeleeEnimes && _ShotEnimes)
                {
                    Instantiate(_allTypeOfEnimes[Random.Range(0, _allTypeOfEnimes.Count)], spawners[i].position, Quaternion.identity);
                }
            }
            aboba.Clear();
        }
    }

    private void Update()
    {
        GameObject[] b = GameObject.FindGameObjectsWithTag("Debil");
        for (int i = 0; i < b.Length; i++)
        {
            aboba.Add(b[i].GetComponent<AI>().enabled == false);
            if(b[i].GetComponent<AI>().enabled == true)
            {
                aboba.Clear();
                break;
            }
        }

        if(GameObject.FindGameObjectsWithTag("Debil").Length == 0 || aboba.Count != 0)
        {
            for (int i = 0; i < spawners.Count; i++)
            {
                if (_MeleeEnimes && !_ShotEnimes)
                {
                    Instantiate(_MeleeTypeOfEnimes[Random.Range(0, _MeleeTypeOfEnimes.Length)], spawners[i].position, Quaternion.identity);
                }
                else if (_ShotEnimes && !_MeleeEnimes)
                {
                    Instantiate(_ShotTypeOfEnimes[Random.Range(0, _ShotTypeOfEnimes.Length)], spawners[i].position, Quaternion.identity);
                }
                else if(_MeleeEnimes && _ShotEnimes)
                {
                    Instantiate(_allTypeOfEnimes[Random.Range(0, _allTypeOfEnimes.Count)], spawners[i].position, Quaternion.identity);
                }
            }
            aboba.Clear();
        }    
    }
}
