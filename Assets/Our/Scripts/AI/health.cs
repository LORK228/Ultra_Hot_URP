using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class health : MonoBehaviour
{
    private GameObject _player;
    public bool IsDead;
    public List<Rigidbody> allChasti;
    private List<CapsuleCollider> allChasticol;
    private AI ai;
    public List<Rigidbody> allChasti2;
    private int force;
    private Collider other;
    private GameObject chastvr;
    private Rigidbody part;
    private Transform impulse;
    private SkinnedMeshRenderer skiMesh;
    private GameObject chast;
    private void Start()
    {

        IsDead = false;
        _player = GameObject.Find("Player");
        ai = gameObject.GetComponent<AI>();
        foreach (Rigidbody wp in allChasti)
        {
            wp.isKinematic = enabled;
        }
    }
    
    public IEnumerator dead()
    {
        gameObject.GetComponent<AI>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        _player.GetComponent<NextLevelSet>().enimesCount -= 1;
        Destroy(gameObject.GetComponent<NavMeshAgent>());
        ai.enabled = false;
        chast = other.GetComponent<bones>().Chast;
        
        gameObject.GetComponent<NavMeshObstacle>().enabled = false;
        Destroy(gameObject.GetComponentInChildren<MeleForAI>());
        gameObject.GetComponent<Animator>().enabled = false;
        if (gameObject.GetComponentInChildren<Item>() != null)
        {
            ai.OnVikinut();
        }
        for (int i = 0; i < allChasti.Count; i++)
        {
            if (allChasti[i] != null && allChasti[i] != other.GetComponent<Rigidbody>())
            {
                allChasti[i].isKinematic = false;
            }
        }
        
        for (int i = 0; i < allChasti2.Count; i++)
        {
            if(allChasti2[i] != null)
            {
                allChasti2[i].GetComponent<BodyPart>().go = true;
            }
        }
        yield return new WaitForSeconds(500);
        Destroy(gameObject);
    }

    public void death(Transform tr, int fr, Collider othr,Rigidbody pt)
    {
        part = pt;
        other = othr;
        force = fr;
        impulse = tr;
        
        if (IsDead == false)
        {
            StartCoroutine(dead());
        }
        IsDead = true;
        if (other.name != "mixamorig:Spine" && other.name != "mixamorig:Hips")
        {
            chast = other.GetComponent<bones>().Chast;
            skiMesh = chast.GetComponent<SkinnedMeshRenderer>();
            bones[] chasti;
            chasti = other.GetComponentsInChildren<bones>();

            if (other.GetComponent<CharacterJoint>() != null)
            {
                other.GetComponent<CharacterJoint>().GetComponent<Rigidbody>().isKinematic = false;

            }
            for (int i = 1; i < chasti.Length; i++)
            {
                if (i == 1)
                {
                    if (force > 0)
                    {
                        chasti[i].GetComponent<Rigidbody>().AddForce(impulse.transform.forward * force * chasti[i].GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                        other.GetComponent<CharacterJoint>().connectedBody.AddForce(impulse.transform.forward * force * other.GetComponent<CharacterJoint>().connectedBody.mass, ForceMode.Impulse);
                    }
                    chastvr = chasti[i].Chast;
                    Destroy(chasti[i].GetComponent<CharacterJoint>());
                    chastvr.GetComponent<SkinnedMeshRenderer>().BakeMesh(chastvr.GetComponent<MeshFilter>().mesh);
                    Destroy(chastvr.GetComponent<SkinnedMeshRenderer>());
                    chastvr.GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    chastvr = chasti[i].Chast;
                    if (chasti[i].GetComponent<CharacterJoint>() != null)
                    {
                        chasti[i].GetComponent<CharacterJoint>().connectedBody = chasti[i - 1].GetComponent<Rigidbody>();
                    }
                    chastvr.GetComponent<MeshRenderer>().enabled = true;
                    chasti[2].Chast.GetComponent<Rigidbody>().isKinematic = true;


                }
            }
            if (part.GetComponent<CharacterJoint>() != null)
            {
                Destroy(other.GetComponent<CharacterJoint>());
            }

            other.GetComponent<Rigidbody>().isKinematic = true;
        skiMesh.gameObject.GetComponent<Destroyer>().DestroyMesh();
        Destroy(skiMesh);
        }
        else
        {
            other.GetComponent<Rigidbody>().isKinematic = false;
            other.GetComponent<Rigidbody>().AddForce(transform.forward * force * other.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        }


        

    }

}
