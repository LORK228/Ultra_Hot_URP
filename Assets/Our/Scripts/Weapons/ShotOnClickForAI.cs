using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShotOnClickForAI : MonoBehaviour
{
	[SerializeField] private Bullet _bulletInstant;
	[SerializeField] private float _timeBefShot;
	[SerializeField] private LayerMask _animes;
	[SerializeField] private int _countOfBulletWhenFired = 1;
	[SerializeField] private float _radius;
	[SerializeField] private bool _isFastGun;
	[SerializeField] public float _timerForShooting;
	[SerializeField] public float _radiusForRandom;

	private float _time;
	private float _timeForShooting;
	private int _countOfBullets;


	// Use this for initialization
	void Start()
	{
		_time = _timeBefShot;
	}

	// Update is called once per frame

	void FixedUpdate()
	{
		gameObject.GetComponentInChildren<BulletSpawner>().transform.LookAt(GameObject.Find("Player").transform);
		if (gameObject.GetComponent<ShotOnClick>().enabled == false && (transform.parent != null && transform.parent.GetComponent<AI>() != null))
		{
			if ((_time < 0 && _radius == 0) || (_countOfBullets % _countOfBulletWhenFired != 0 && _timeForShooting > _timerForShooting && !_isFastGun))
			{
				invoke();
				_countOfBullets += 1;
				_time = _timeBefShot;
				attackPlease();
				_timeForShooting = 0;
			}
			else if ((_time < 0 && _radius == 0) || (_countOfBullets % _countOfBulletWhenFired != 0 && _timeForShooting > _timerForShooting && transform.parent != null && _time < 0 && _isFastGun))
            {
				invoke();
				_countOfBullets += 1;
				_time = _timeBefShot;
				attackPlease();
				_timeForShooting = 0;
			}
			else if (_time < 0 && _radius > 0)
			{
				invoke();
				Vector3 hit1 = GetPoint();
				for (int i = 0; i < _countOfBulletWhenFired; i++)
				{
					_countOfBullets += 1;
					_time = _timeBefShot;
					attackPleaseForShotGun(hit1);
				}
			}
		}
		else if ((transform.parent == null || transform.parent.GetComponent<AI>() == null))
		{
			gameObject.GetComponent<ShotOnClickForAI>().enabled = false;
		}

		_time -= Time.fixedDeltaTime;
		_timeForShooting += Time.fixedDeltaTime;
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
	void attackPlease()
	{ 
		Vector3 newPoint = GetPoint();
        Bullet clonBullet = Instantiate(_bulletInstant, gameObject.transform.parent.GetComponentInChildren<BulletSpawner>().transform.position, Quaternion.identity);
		clonBullet.transform.LookAt(newPoint);

	}
	void attackPleaseForShotGun(Vector3 hitForward)
	{
		Vector3 newPoint = GetPointForShotgun(hitForward);
		Bullet clonBullet = Instantiate(_bulletInstant, gameObject.transform.parent.GetComponentInChildren<BulletSpawner>().transform.position, Quaternion.identity);
		clonBullet.transform.LookAt(newPoint);
		clonBullet.GetComponent<Bullet>().healBullet = 0;
	}
	private Vector3 GetPoint()
	{
		Transform b = Instantiate(gameObject.transform.parent.GetComponentInChildren<BulletSpawner>().transform, gameObject.transform.parent.GetComponentInChildren<BulletSpawner>().transform.position, gameObject.transform.parent.GetComponentInChildren<BulletSpawner>().transform.rotation);
		Ray ray = new Ray(gameObject.transform.parent.GetComponentInChildren<BulletSpawner>().transform.position, b.forward);
		RaycastHit hit;
		Physics.Raycast(ray, out hit, Mathf.Infinity, _animes);
		float distance = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
		Vector3 point;
		if (distance > 9)
		{
			print(_radiusForRandom * distance);
			point = hit.point + new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f).normalized * (_radiusForRandom * distance);
		}
        else
        {
			point = hit.point;
        }
		Destroy(b.gameObject);
		return point;
	}

	private Vector3 GetPointForShotgun(Vector3 hit1)
	{
		float distance = Vector3.Distance(transform.position, hit1);
		Vector3 a = hit1 + new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f).normalized * (_radius * distance);
		return a;
	}
}