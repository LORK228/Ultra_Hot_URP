using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleAndroid : MonoBehaviour
{
    [SerializeField] public UIButtonInfo buttonInfo;
    [SerializeField] private int force;
    [HideInInspector] public int damage;

    private float _timer;
    private bool _BoxColliderForMeleeCombat;
    private List<GameObject> udarniki;
    private GameObject _part;
    public float _timeBetweenBeats;
    private bool HaveGun;
    
    private bool ThrowableObj;
    private UIButtonInfo sButton;
    private GameObject _enemy;
    private List<GameObject> _parts;
    private float min;
    private Collider othr;

    private void Start()
    {
        _parts = new List<GameObject>();
        udarniki = new List<GameObject>();
        HaveGun = gameObject.GetComponentInParent<ControllerAndroid>().gameObject.GetComponentInChildren<ShotOnClick>();
        sButton = GameObject.Find("ShootButton").GetComponent<UIButtonInfo>();
        damage = 1;
    }
    private void Update()
    {
        if (gameObject.GetComponentInParent<ControllerAndroid>().gameObject.GetComponentInChildren<Item>() != null)
        {
            if (gameObject.GetComponentInParent<ControllerAndroid>().gameObject.GetComponentInChildren<Item>()._IsThrowable)
            {
                ThrowableObj = true;
            }
            else
            {
                ThrowableObj = false;
            }
        }
        else
        {
            ThrowableObj = false;
        }
        if (udarniki.Count > 0)
        {
            _enemy = udarniki[0];
            foreach (GameObject wp in udarniki)
            {
                if (min > Vector3.Distance(wp.transform.position, transform.position))
                {
                    _enemy = wp;
                    min = Vector3.Distance(wp.transform.position, transform.position);
                }
            }
            foreach (GameObject wp in _parts)
            {
                if (min > Vector3.Distance(wp.transform.position, transform.position))
                {
                    _part = wp;
                    min = Vector3.Distance(wp.transform.position, transform.position);
                }
            }

            if (sButton.isDown && gameObject.GetComponentInParent<ControllerAndroid>().gameObject.GetComponent<ShotOnClick>() == null) {
                if (_timer < 0 && _enemy.GetComponent<AI>().heal > 0)

                {
                    if (ThrowableObj == false && _timer < 0)
                    {
                        if (_enemy.GetComponent<AI>().hog == false)
                        {
                            {

                                if (_enemy.GetComponentInChildren<ShotOnClick>() == null && _enemy.GetComponentInChildren<Item>() == null)
                                {
                                    _enemy.GetComponent<AI>().heal -= damage;

                                    if (_enemy.GetComponent<AI>().heal > 0)
                                    {
                                        _enemy.GetComponent<AI>().Stun();
                                    }
                                    _timer = _timeBetweenBeats;
                                }

                                else if (_enemy.GetComponentInChildren<Item>() != null)
                                {
                                    _enemy.GetComponent<AI>().heal -= damage;
                                    _enemy.GetComponent<AI>().OnVikinut();
                                    if (_enemy.GetComponent<AI>().heal <= 0)
                                    {
                                        _enemy.GetComponent<health>().death(transform, force, _part.GetComponent<Collider>(), _part.GetComponent<Rigidbody>());
                                        print(_part.name);
                                        _part.GetComponent<Rigidbody>().AddForce(_part.transform.forward * force, ForceMode.Impulse);

                                    }
                                    else { _enemy.GetComponent<AI>().Stun(); }
                                    _timer = _timeBetweenBeats;

                                }
                            }
                        }
                    }
                }
                    }
        }
        _timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.GetComponent<bones>()!=null)
        {
            _parts.Add(other.gameObject);
            udarniki.Add(other.gameObject.GetComponentInParent<AI>().gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 && other.GetComponent<bones>() != null)
        {
            udarniki.Remove(other.gameObject.GetComponentInParent<AI>().gameObject);
            _parts.Remove(other.gameObject);
        }
    }
}
