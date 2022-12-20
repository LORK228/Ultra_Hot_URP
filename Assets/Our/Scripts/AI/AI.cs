using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.Events;
using System;

public class AI : MonoBehaviour
{
	public bool hog;
	public int maxheal;
	public float _time;
	public LayerMask masks;
	public bool InStun;//если в стане
	public bool Chase; // если идет за игроком (нет орудий на карте)
	public Aim aim;
	public Transform[] points;
	public GameObject arm;
	


	private int destPoint = 0;
	private float Timer;
	private Transform _player;
	private bool patrol;
	private GameObject[] grounditems;
	public Animator animation;


	[SerializeField] private GameObject item;

	[SerializeField] public float min;

	[SerializeField] private GameObject minweapon;

	[SerializeField] public int heal;

	[SerializeField] private GameObject head;



	[HideInInspector] public List<GameObject> _grounditems;

	[HideInInspector] public NavMeshAgent agent;

	[HideInInspector] public bool HaveWeapon;//имеет оружие на руках

	[HideInInspector] public bool lookforweapon; // в поисках оружия

	[HideInInspector] public bool WasInHandAI; //оружие было в руках у бота (что бы при выпадении не уничтожалось)

	[HideInInspector] public List<GameObject> AiToDel;

	public IEnumerator ai_dead() // стан с последующим обновлением списка оружий

	{
		if (heal > 0)
		{
			patrol = false;
			Chase = false;
			agent.enabled = false;
			InStun = true;
			yield return new WaitForSeconds(2);
			if (heal > 0 && gameObject != null && gameObject.GetComponent<NavMeshAgent>() != null)
			{
				InStun = false;
				refresh();
				agent.enabled = true;
			}


		}
	}

	void refresh()
	{
		_grounditems.Clear();
		if (!hog)
		{
			grounditems = GameObject.FindGameObjectsWithTag("Weapons");
			AiToDel = new List<GameObject>();
			foreach (GameObject j in GameObject.FindGameObjectsWithTag("Debil"))
			{
				if (j != gameObject)
				{
					AiToDel.Add(j);
				}

			}
			for (int i = 0; i < grounditems.Length; i++)
			{
				if (grounditems[i].GetComponent<Item>() != null)
				{
					if (grounditems[i].GetComponent<Item>().wasInHands == false && grounditems[i].GetComponent<Item>()._IsThrowable == false && grounditems[i].GetComponent<Item>()._isInHendAI == false)
					{
						_grounditems.Add(grounditems[i]);
					}
				}
			}
			if (_grounditems.Count == 0)
			{
				Chase = true;
			}
			min = Mathf.Infinity;
		}
	}
    void Awake()
	{
		animation = gameObject.GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();
		WasInHandAI = false;
		min = Mathf.Infinity;
		
		
		Timer = _time;
		_player = GameObject.Find("Player").transform;
        if (gameObject.GetComponentInChildren<ShotOnClickForAI>() != null)
        {
            Chase = false;
        }
        agent.autoBraking = false;
        if (points.Length > 0)
        {
			
			patrol = true;
			GotoNextPoint();
        }
        if (points.Length == 0)
        {
			animation.SetBool("run",true);
			patrol = false;
            agent.speed = 1.5f;
        }
        refresh();
    }
    void Update()
	{
		if (patrol)
        {
            if (Vector3.Distance(gameObject.transform.position, _player.position) < distance)
            {
                if (Vector3.Distance(gameObject.transform.position, _player.position) <= heardist)
                {
					animation.SetBool("run", true);
					refresh();
                    patrol = false;
                    agent.speed = 1.5f;
                }
                else if (RayToScan())
                {
                    refresh();
                    patrol = false;
                    agent.speed = 1.5f;
					animation.SetBool("run", true);
				}
            }

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();

        }

		if (patrol == false)
		{
			if (HaveWeapon == false && _grounditems.Count != 0)
			{

				for (int i = 0; i < _grounditems.Count; i++)
				{
					if (min > Vector3.Distance(_grounditems[i].transform.position, transform.position) && (_grounditems[i].GetComponent<MeshRenderer>().enabled == true))
					{
						min = Vector3.Distance(_grounditems[i].transform.position, transform.position);
						minweapon = _grounditems[i];
					}
				}
				if (Vector3.Distance(_player.position, gameObject.transform.position) > min)
				{
					for (int i = 0; i < AiToDel.Count; i++)
					{
						AiToDel = new List<GameObject>();
						foreach (GameObject j in GameObject.FindGameObjectsWithTag("Debil"))
						{
							if (j != gameObject)
							{
								AiToDel.Add(j);
							}

						}
						AiToDel[i].GetComponent<AI>()._grounditems.Remove(minweapon);
					}
					if (minweapon.transform != null && _grounditems.Count != 0 && HaveWeapon == false && agent.enabled == true)
                    {
                        agent.SetDestination(minweapon.transform.position);
						aim.sight = minweapon.transform;


					}
                }
				else
				{
					_grounditems.Clear();
				}
			}
			if (_grounditems.Count == 0 && HaveWeapon == true && lookforweapon == false)
			{

				RaycastHit hit;
				if (Physics.Raycast(transform.position, _player.position - transform.position, out hit, Mathf.Infinity, masks))
				{
					if (gameObject.GetComponentInChildren<ShotOnClick>() == true)
					{
						if (hit.transform != _player)
						{
							Timer = _time;
							agent.enabled = true;
							agent.SetDestination(_player.position);
							aim.sight = _player;


						}
						else
						{
							Timer -= Time.deltaTime;
							if (Timer > 0)
							{
								gameObject.GetComponentInChildren<ShotOnClickForAI>().enabled = false;
								
								agent.SetDestination(_player.position);
								aim.sight = _player;
							}

							else
							{
								
								agent.enabled = false;
								gameObject.GetComponentInChildren<ShotOnClickForAI>().enabled = true;
							}
						}
					}
				}
			}
			if (_grounditems.Count == 0 && (HaveWeapon == false || gameObject.GetComponentInChildren<Item>() == true) && InStun == false && agent.enabled == true)
			{
				Chase = true;
                agent.SetDestination(_player.position);
				aim.sight = _player;
			}
		}
	}

	public IEnumerator rep()
    {
		yield return new WaitForSeconds(3);
	}

	public void Attention()
    {
		if(Vector3.Distance(gameObject.transform.position, _player.gameObject.transform.position) < 40 && heal > 0)
        {
			refresh();
			patrol = false;
			agent.speed = 1.5f;
			animation.SetBool("run", true);
		}
    }
	public void Stun()
    {
		StartCoroutine(ai_dead());
    }
	public void OnVikinut()
    {
		Rigidbody b = gameObject.GetComponentInChildren<Item>().gameObject.GetComponent<Rigidbody>();
		b.transform.parent = null;
		b.GetComponent<Item>()._isInHendAI = false;
        b.GetComponent<Item>().WasInHandAI = true;
		b.GetComponent<Item>().GetComponent<MeshCollider>().enabled = true;
		b.useGravity = true;
		b.gameObject.GetComponent<Item>().wasInHands = false;
		b.isKinematic = false;
		StartCoroutine(ai_dead());
		HaveWeapon = false;

	}
	void GotoNextPoint()
	{
		
		if (points.Length == 0)
			return;
		agent.destination = points[destPoint].position;
		destPoint = (destPoint + 1) % points.Length;
	}
	[Header("Поле Зрения")]
	private int rays = 3;
	private int distance = 15;
	private int heardist = 5;
	private float angle = 20;
	public Vector3 offset;
	
	bool GetRaycast(Vector3 dir)
	{
		bool result = false;
		RaycastHit hit = new RaycastHit();
		Vector3 pos = transform.position + offset;
		if (Physics.Raycast(pos, dir, out hit, distance))
		{
			print("scan");
			if (hit.transform.position == _player.position)
			{
				print("result");
				result = true;
				Debug.DrawLine(pos, hit.point, Color.green);
			}
			else
			{
				Debug.DrawLine(pos, hit.point, Color.blue);
			}
		}
		else
		{
			Debug.DrawRay(pos, dir * distance, Color.red);
		}
		return result;
    }

    bool RayToScan()
    {
        bool result = false;
        bool a = false;
        bool b = false;
        float j = 0;
        for (int i = 0; i < rays; i++)
        {
            var x = Mathf.Sin(j);
            var y = Mathf.Cos(j);

            j += angle * Mathf.Deg2Rad / rays;

            Vector3 dir = transform.TransformDirection(new Vector3(x, 0, y));
            if (GetRaycast(dir)) a = true;

            if (x != 0)
            {
                dir = transform.TransformDirection(new Vector3(-x, 0, y));
                if (GetRaycast(dir)) b = true;
            }
        }

        if (a || b) result = true;
        return result;
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == 11)
        {
            item = other.gameObject;
            if (item.transform.parent == null && item.GetComponent<ShotOnClick>() == null && gameObject.transform.parent == null && item.GetComponent<Item>().wasInHands == false && gameObject.GetComponent<AI>().enabled == true && InStun != true && Chase == false)
            {
                Item lok = item.gameObject.GetComponent<Item>();
                lookforweapon = false;
                HaveWeapon = true;
                WasInHandAI = true;
                _grounditems.Clear();
                lok._animateForAI = true;
                lok._isInHendAI = true;
                lok.wasInHands = true;
                gameObject.transform.parent = other.gameObject.transform;
                if (!lok.isMelee)
                {
                    gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
                }
                else
                {
                    gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
		
	}
    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.layer == 17)
		{
			other.gameObject.GetComponent<BreakGlass>().BreakIt(transform.position, 2);
		}
	}
    private void OnAnimatorIK(int layerIndex)
    {
     
    }
    public void bleed()
    {
		StartCoroutine(maxhealcheck());
    }
    public IEnumerator maxhealcheck()
	{
		print("fddsf");
		yield return new WaitForSeconds(3);
		heal = 0;
	}

}