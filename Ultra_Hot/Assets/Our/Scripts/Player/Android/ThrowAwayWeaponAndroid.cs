using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAwayWeaponAndroid : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _force;

    [SerializeField] public GameObject Throw;
    [SerializeField] public GameObject ShootButton;
    [SerializeField] public GameObject InteractButton;

    private UIButtonInfo ButtonInfo;
    private void Start()
    {
        ButtonInfo = Throw.GetComponent<UIButtonInfo>();
        Throw.SetActive(false);
    }
    private void Update()
    {

        if (ButtonInfo.isDown)
        {

            if (_camera.GetComponentInChildren<ShotOnClick>() != null)
            {
                
                Throw.SetActive(false);
                InteractButton.SetActive(true);
                ButtonInfo.isDown = false;
                Rigidbody b = _camera.GetComponentInChildren<ShotOnClick>().gameObject.GetComponent<Rigidbody>();
                b.mass = 0.3f;
                b.gameObject.layer = 11;
                b.transform.parent = null;
                b.useGravity = true;
                b.gameObject.GetComponent<Item>().wasInHands = true;
                gameObject.GetComponent<ControllerAndroid>().weaponized = false;
                b.isKinematic = false;
                
                b.AddForce(_camera.transform.forward * _force, ForceMode.Impulse);
                b.gameObject.GetComponent<Item>()._meshRenderer.material = b.gameObject.GetComponent<Item>()._materialStandart;
            }
            else if (_camera.GetComponentInChildren<Item>() != null && _camera.GetComponentInChildren<Item>().isMelee)
            {
                Throw.SetActive(false);
                Rigidbody b = _camera.GetComponentInChildren<Item>().gameObject.GetComponent<Rigidbody>();
                b.mass = 0.3f;
                GameObject.Find("Player").GetComponentInChildren<MeleAndroid>().gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 0.72f, 0.96f);
                GameObject.Find("Player").GetComponentInChildren<MeleAndroid>().gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0, 1);
                GameObject.Find("Mele").GetComponent<MeleAndroid>().damage = 1;
                b.gameObject.layer = 11;
                b.gameObject.GetComponent<Item>().wasInHands = true;
                b.transform.parent = null;
                b.useGravity = true;
                b.isKinematic = false;
                b.gameObject.GetComponent<Item>()._meshRenderer.material = b.gameObject.GetComponent<Item>()._materialStandart;
                gameObject.GetComponentInChildren<MeleAndroid>().buttonInfo.gameObject.SetActive(true);
                gameObject.GetComponent<ControllerAndroid>().weaponized = false;
                b.AddForce(_camera.transform.forward * _force, ForceMode.Impulse);

            }
        }
    }
}
