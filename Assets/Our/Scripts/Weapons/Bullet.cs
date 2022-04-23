using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject projectorEnemy;
    [SerializeField] private GameObject Reflect;
    [SerializeField] private float speed;
    [SerializeField] public int healBullet;
    [SerializeField] public int force;
    [SerializeField] public GameObject enemyParticle;
    [HideInInspector] public int damage;
    [HideInInspector] public bool ForAI;

    private Ray ray;
    private List<GameObject> AI;
    private bool Used;
    private bool line;
    private bool raycast;
    private RaycastHit hit;
    private GameObject oder;
    private Rigidbody part;
    private void Awake()
    {
        line = true;
        ForAI = false;
        Used = false;
    }
    private void Start()
    {
        ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        raycast = Physics.Raycast(ray, out hit, Mathf.Infinity);
        this.hit = hit;
    }
    private void Update()
    {
        if (line)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 11 && other.gameObject.layer != 12 && other.isTrigger == false)
        {
            if (healBullet == 0)
            {
                line = false;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            if (healBullet == 0)
            {
                StartCoroutine("b_dea");
            }
            if (other.gameObject.layer == 8 && ForAI == true && Used == false && other.GetComponent<bones>() != null)
            {
                Instantiate(enemyParticle, transform.position, Quaternion.Euler(-transform.forward));
                health health = other.gameObject.GetComponentInParent<health>();
                if (health.IsDead == true)
                {
                    other.GetComponent<Rigidbody>().AddForce(transform.forward * force * other.GetComponent<Rigidbody>().mass * 3, ForceMode.Impulse);
                    StartCoroutine("b_dea");
                }
                else if (health.IsDead == false)
                {
                    oder = other.gameObject.GetComponentInParent<AI>().gameObject;
                    part = other.GetComponent<Rigidbody>();
                    gameObject.GetComponent<TrailRenderer>().emitting = false;
                    line = false;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    gameObject.GetComponent<MeshRenderer>().enabled = false;

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position - transform.forward, transform.forward, out hit))
                    {
                        Quaternion projectorRotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                        GameObject obj = Instantiate(projectorEnemy, hit.point + hit.normal * 0.25f, projectorRotation) as GameObject;
                        Quaternion randomRotZ = Quaternion.Euler(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y, Random.Range(0, 360));
                        obj.transform.rotation = randomRotZ;
                        obj.transform.parent = other.gameObject.transform;
                    }

                    AI = new List<GameObject>();
                    foreach (GameObject i in GameObject.FindGameObjectsWithTag("Debil"))
                    {
                        AI.Add(i);
                    }
                    for (int i = 0; i < AI.Count; i++)
                    {
                        AI[i].GetComponent<AI>().AiToDel.Remove(other.gameObject);
                    }
                    oder.GetComponent<AI>().heal -= damage;
                    Used = true;
                    if (oder.GetComponent<AI>().hog)
                    {
                        oder.GetComponent<AI>().bleed();
                    }


                    if (other.gameObject.GetComponentInParent<AI>().heal <= 0)
                    {
                        health.death(transform,force,other,part);
                    }
                }
                StartCoroutine(b_dea());
            }


            if (other.gameObject.layer == 9 && healBullet > 0)
            {
                healBullet -= 1;


                if (raycast)
                {
                    Ray ray1 = new Ray(transform.position, Vector3.Reflect(ray.direction, this.hit.normal));
                    RaycastHit hit1;
                    Physics.Raycast(ray1, out hit1, Mathf.Infinity);
                    transform.LookAt(hit1.point);

                    ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;
                    raycast = Physics.Raycast(ray, out hit, Mathf.Infinity);
                    this.hit = hit;

                    Instantiate(Reflect, transform.position, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                }
            }

            if (other.gameObject.layer == 16)
            {
                transform.parent = other.transform;
                gameObject.GetComponent<TrailRenderer>().emitting = false;
                line = false;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                oder.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
                StartCoroutine("b_dea");
            }

            if (other.gameObject.layer == 0 && ForAI == false)
            {
                other.GetComponent<ThrowAwayWeaponAndroid>().enabled = false;
            }


        }

        IEnumerator b_dea()
        {
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }
    }
}