using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DestroyIt;
using TMPro;

public class ShotOnClick : MonoBehaviour
{
    [SerializeField] private Bullet _bulletInstant;
    [SerializeField] private float _timeBefShot;
    [SerializeField] private LayerMask _animes;
    [SerializeField] private int CountOfBullet;
    [SerializeField] private int _countOfBulletWhenFired = 1;
    [SerializeField] private float _radius;
    [SerializeField] private bool _isFastGun;
    [SerializeField] private int damage;
    [SerializeField] private GameObject simpleGunParticle;
    [SerializeField] private GameObject shotGunParticle;

    //////////////////////////
    //private variables
    private Transform _spawnPoint;
    private BulletSpawner bulletSpawner;
    private UIButtonInfo ButtonInfo;
    private float _time;
    private float _timeForShooting;
    public float _timerForShooting;
    private int _countOfBullets;
    private TextMeshProUGUI text1;
    //////////////////////////
    private void Awake()
    {
        bulletSpawner = gameObject.GetComponentInChildren<BulletSpawner>();
        _spawnPoint = GameObject.Find("spawnBullets").GetComponent<Transform>();
        text1 = GameObject.Find("HowManyBullets").GetComponent<TextMeshProUGUI>();
        if(GameObject.Find("ShootButton").GetComponent<UIButtonInfo>()!= null)
        {
            ButtonInfo = GameObject.Find("ShootButton").GetComponent<UIButtonInfo>();
        }
    }
    private void Start()
    {
        text1.text = (CountOfBullet - _countOfBullets) + "/" + CountOfBullet;
        GetComponentInParent<ThrowAwayWeaponAndroid>().ShootButton.SetActive(true);
        GetComponentInParent<ThrowAwayWeaponAndroid>().Throw.SetActive(true);
        GetComponentInParent<ThrowAwayWeaponAndroid>().InteractButton.SetActive(false);
    }
    private void Update()
    {
        if (transform.parent == null) text1.text = "";

        //это стрельба узи
        if(ButtonInfo.isDown && _countOfBullets != CountOfBullet && _timeForShooting > _timerForShooting && transform.parent != null && _time < 0 && _isFastGun && _radius !=0)
        {
            GameObject particle = Instantiate(simpleGunParticle, bulletSpawner.transform.position, Quaternion.Euler(transform.forward));
            invoke();
            _countOfBullets += 1;
            _time = _timeBefShot;
            Vector3 newPoint = GetPoint();
            Bullet clonBullet = Instantiate(_bulletInstant, _spawnPoint.transform.position, Quaternion.identity);
            clonBullet.transform.LookAt(newPoint);
            particle.transform.LookAt(newPoint);
            clonBullet.gameObject.GetComponent<Bullet>().ForAI = true;
            clonBullet.gameObject.GetComponent<Bullet>().damage = damage;
            _timeForShooting = 0;
            text1.text = (CountOfBullet - _countOfBullets) + "/" + CountOfBullet;
        }
        //
        if (((ButtonInfo.isDown) && _time < 0 && _countOfBullets != CountOfBullet && transform.parent != null && _radius == 0 && !_isFastGun) || (_countOfBullets % _countOfBulletWhenFired != 0 && _timeForShooting > _timerForShooting && transform.parent != null && !_isFastGun))
        {
            GameObject particle = Instantiate(simpleGunParticle, bulletSpawner.transform.position, Quaternion.Euler(transform.forward));
            invoke();
            _countOfBullets += 1;
            _time = _timeBefShot;
            Vector3 newPoint = GetPoint();
            Bullet clonBullet = Instantiate(_bulletInstant, _spawnPoint.transform.position, Quaternion.identity);
            clonBullet.transform.LookAt(newPoint);
            particle.transform.LookAt(newPoint);
            clonBullet.gameObject.GetComponent<Bullet>().ForAI = true;
            clonBullet.gameObject.GetComponent<Bullet>().damage = damage;
            _timeForShooting = 0;
            text1.text = (CountOfBullet - _countOfBullets) + "/" + CountOfBullet;
        }

        else if (ButtonInfo.isDown && _time < 0 && _countOfBullets != CountOfBullet && transform.parent != null && !_isFastGun && _radius !=0)
        {
            Vector3 newPoint1 = GetPoint();
            GameObject particle = Instantiate(shotGunParticle, bulletSpawner.transform.position, Quaternion.Euler(transform.forward));
            particle.transform.LookAt(newPoint1);
            invoke();
            for (int i = 0; i < _countOfBulletWhenFired; i++)
            {
                _countOfBullets += 1;
                _time = _timeBefShot;
                Vector3 newPoint = GetPointForShotGun();
                Bullet clonBullet = Instantiate(_bulletInstant, _spawnPoint.position, Quaternion.identity);
                clonBullet.gameObject.GetComponent<Bullet>().ForAI = true;
                clonBullet.gameObject.GetComponent<Bullet>().damage = damage;
                clonBullet.transform.LookAt(newPoint);
                clonBullet.GetComponent<Bullet>().healBullet = 0;
            }
            text1.text = (CountOfBullet - _countOfBullets) + "/" + CountOfBullet;
        }

        _time -= Time.deltaTime;
        _timeForShooting += Time.deltaTime;
    }
    
    private Vector3 GetPoint()
    {
        Transform b = Instantiate(_spawnPoint, _spawnPoint.position, GameObject.Find("Main Camera").transform.rotation);
        Ray ray = new Ray(_spawnPoint.position, b.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, _animes);
        Destroy(b.gameObject);
        return hit.point;
    }

    private Vector3 GetPointForShotGun()
    {
        Vector3 hit1 = GetPoint();
        float distance = Vector3.Distance(GameObject.FindObjectOfType<ControllerAndroid>().transform.position, hit1);
        Vector3 a = hit1 + new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f).normalized * (_radius * distance);
        return a;
    }

    private void invoke()
    {
        foreach (GameObject j in GameObject.FindGameObjectsWithTag("Debil"))
        {
            if (j != null)
            {
                j.GetComponent<AI>().Attention();
            }
        }
    }
    
}
