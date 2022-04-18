using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PickUpPrior : MonoBehaviour
{
    [HideInInspector] public List<GameObject> prior;
    [HideInInspector] public GameObject minwp;
    [HideInInspector] public UIButtonInfo interactButton;
    [HideInInspector] public Image _myImage;

    private Transform _player;
    private void Awake()
    {
        prior = new List<GameObject>();
        _player = gameObject.GetComponent<Transform>();
        interactButton = GameObject.Find("InteractButton").GetComponent<UIButtonInfo>();
        _myImage = GameObject.Find("InteractImage").GetComponent<Image>();
    }
    public void MinDist()
    {
        if (prior.Count > 0 && _player.gameObject.GetComponent<ControllerAndroid>().weaponized == false)
        {
            float min = Mathf.Infinity;
            foreach (GameObject wp in prior)
            {

                if (Vector3.Distance(wp.transform.position, _player.position) < min)
                {
                    minwp = wp;
                    min = Vector3.Distance(wp.transform.position, _player.position);
                }


            }

            foreach (GameObject wp in prior)
            {
                if (wp != minwp)
                {
                    wp.GetComponent<Item>()._meshRenderer.material = wp.GetComponent<Item>()._materialStandart;
                }



                minwp.GetComponent<Item>()._meshRenderer.material = minwp.GetComponent<Item>()._materialInteract;
            }
        }
    }


    public void Removed1(GameObject lm)
    {
        prior.Remove(lm);
    }
}

