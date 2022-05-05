using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DestroyIt;
public class Item : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _interactDistance;
    [SerializeField] public bool _isInHend;
    [SerializeField] public bool _isInHendAI; 
    [SerializeField] public Material _materialInteract;
    [SerializeField] public bool IsLethal;
    [SerializeField] public bool _IsThrowable;
    [SerializeField] private int rotationy;
    [SerializeField] private int rotationz;
    [SerializeField] public bool isMelee;
    [SerializeField] private int force;
    [HideInInspector] public UIButtonInfo interactButton;
    [HideInInspector] public MeshRenderer _meshRenderer;
    [HideInInspector] public Material _materialStandart;
    [HideInInspector] public bool _animateForAI;
    [HideInInspector] public bool WasInHandAI;
    [HideInInspector] public bool wasInHands;

    private bool WasAI;
    private Destructible des;
    private ControllerAndroid control;
    private Transform _target;
    private Transform _player;
    private AI _AI;
    private float _distance;
    private Image _myImage;
    private Item _playerItem;
    private List<GameObject> AI;
    private bool _animate;
    private GameObject player;
    private AI part;
    private PickUpPrior priora;
    private void Awake()
    {
        interactButton = GameObject.Find("InteractButton").GetComponent<UIButtonInfo>();
        WasInHandAI = false;
        WasAI = false;
        // записываются в переменные компонент Image (который высвечивается когда смотрим на оружие)
        _myImage = GameObject.Find("InteractImage").GetComponent<Image>();
        // записываются в переменные компонент Transform из нашего Игорька 
        player = GameObject.Find("Player");
        _player = player.GetComponent<Transform>();
        control = player.GetComponent<ControllerAndroid>();

        _playerItem = player.GetComponentInChildren<Item>();

        _target = GameObject.Find("Player").GetComponentInChildren<Target>().transform;
        priora = _player.gameObject.GetComponent<PickUpPrior>();
    }
    private void Start()
    {
        _materialStandart = gameObject.GetComponent<MeshRenderer>().material;
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        // тут мы отключаем ShotOnclick чтобы он не стрелял дистанционно
        if (!isMelee)
        {
            gameObject.GetComponent<ShotOnClickForAI>().enabled = _isInHendAI;
        }
        
        des = gameObject.GetComponentInChildren<Destructible>();
        if (_isInHendAI && !isMelee)
        {
            transform.parent.gameObject.GetComponent<AI>().WasInHandAI = true;
            for (int i = 0; i < transform.GetComponentInParent<AI>().AiToDel.Count; i++)
            {
                transform.GetComponentInParent<AI>().AiToDel[i].GetComponent<AI>()._grounditems.Remove(gameObject);
                transform.GetComponentInParent<AI>().AiToDel[i].GetComponent<AI>().min = Mathf.Infinity;
            }
            wasInHands = true;
            gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.parent.gameObject.GetComponent<AI>().HaveWeapon = true;
            transform.parent.gameObject.GetComponent<AI>().lookforweapon = false;
            transform.parent.gameObject.GetComponent<AI>()._grounditems.Clear();
            
        }
        else if(_isInHendAI && isMelee)
        {
            transform.parent.gameObject.GetComponent<AI>().WasInHandAI = true;
            for (int i = 0; i < transform.GetComponentInParent<AI>().AiToDel.Count; i++)
            {
                transform.GetComponentInParent<AI>().AiToDel[i].GetComponent<AI>()._grounditems.Remove(gameObject);
                transform.GetComponentInParent<AI>().AiToDel[i].GetComponent<AI>().min = Mathf.Infinity;
            }
            _isInHendAI = true;
            gameObject.transform.localRotation = Quaternion.Euler(0, -180, 0);
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.parent.gameObject.GetComponent<AI>().HaveWeapon = true;
            transform.parent.gameObject.GetComponent<AI>().lookforweapon = false;
            transform.parent.gameObject.GetComponent<AI>()._grounditems.Clear();
        }
        if (!isMelee)
        {
            gameObject.GetComponent<ShotOnClick>().enabled = _isInHend;
        }

        
        
        

    }

        private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 0 && transform.parent == null && wasInHands == false)
        {
            if (control.weaponized == false)
            {
                if (other.gameObject.GetComponentInChildren<Item>() == null)
                {
                    
                    if (interactButton.isDown)
                    {
                        if (priora.minwp == gameObject && wasInHands == false && other.gameObject.GetComponentInChildren<Item>() == null && _isInHendAI == false)
                                {
                                    priora.prior.Clear();
                                    control.weaponized = true;
                                    
                                    AI = new List<GameObject>();
                                    foreach (GameObject i in GameObject.FindGameObjectsWithTag("Debil"))
                                    {
                                        AI.Add(i);
                                    }

                                    if (AI.Count != 0)
                                    {
                                        for (int i = 0; i < AI.Count; i++)
                                        {

                                             
                                            AI[i].GetComponent<AI>()._grounditems.Remove(gameObject);
                                            AI[i].GetComponent<AI>().min = Mathf.Infinity;
                                        }
                                    }


                                    gameObject.layer = 12;
                                    // если мы смотрим на оружие и нажали на левую кнопку мыши то на картинка отключается,
                                    _myImage.enabled = false;
                                    // оружие начинает лететь к нам,
                                    _animate = true;
                                    // включается переменная что мы держали оружие,
                                    wasInHands = true;
                                    WasInHandAI = false;
                                    _isInHend = true;
                                    // делаем родителем для оружия камеру, чтобы пистолет следовал за камерой(логично),
                                    gameObject.transform.parent = _player.GetComponentInChildren<Camera>().gameObject.transform;
                                    GetComponentInParent<ThrowAwayWeaponAndroid>().Throw.SetActive(true);
                                    GetComponentInParent<ThrowAwayWeaponAndroid>().Throw.GetComponent<UIButtonInfo>().isDown = false;
                                    GetComponentInParent<ThrowAwayWeaponAndroid>().InteractButton.SetActive(false);
                                    GetComponentInParent<ThrowAwayWeaponAndroid>().InteractButton.GetComponent<UIButtonInfo>().isDown = false;
                                    GetComponentInParent<ThrowAwayWeaponAndroid>().ShootButton.SetActive(true);
                                    GetComponentInParent<ThrowAwayWeaponAndroid>().ShootButton.GetComponent<UIButtonInfo>().isDown = false;
                                    // делаем поворот для оружия,
                                    if (!isMelee)
                                    {
                                        gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
                                    }
                                    else
                                    {
                                        gameObject.transform.localRotation = Quaternion.Euler(0, rotationy, rotationz);
                                        GameObject.Find("Player").GetComponentInChildren<MeleAndroid>().gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 0.72f, 1.62f);
                                        GameObject.Find("Player").GetComponentInChildren<MeleAndroid>().gameObject.GetComponent<BoxCollider>().center = new Vector3(0, 0, 1.51f);
                                    }
                                    // отключаем физику для оружия чтобы оно не падало
                                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                                    if (IsLethal == true && _IsThrowable == false)
                                    {
                                        GameObject.Find("Player").GetComponentInChildren<MeleAndroid>().damage = 3;
                            }
                                    _meshRenderer.material = _materialStandart;
                                    priora.Removed1(gameObject);
                        }
                    }
                }
            }
        }

        if (gameObject.transform.parent == null)
        {
            if (other.gameObject.layer == 8 && other.GetComponentInChildren<ShotOnClick>() == null  && gameObject.transform.parent == null && wasInHands == false && other.gameObject.GetComponent<AI>().enabled == true && other.GetComponent<AI>().InStun != true && other.GetComponent<AI>().Chase == false && other.GetComponent<AI>().hog == false)
            {
                _AI = other.gameObject.GetComponent<AI>();
                _AI.lookforweapon = false;
                _AI.HaveWeapon = true;
                _AI.WasInHandAI = true;
                _AI._grounditems.Clear();
                _AI.Chase = false;
               _animateForAI = true;
                _isInHendAI = true;
                wasInHands = true;
                GetComponent<MeshCollider>().enabled = false;
                gameObject.transform.parent = other.gameObject.transform;
                if (!isMelee)
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
    private void Update()
    {
        // собственно это "анимация" движения нашего предмета
        _distance = Vector3.Distance(_player.position, transform.position);
    
        if (_AI != null && _animateForAI == true && _AI.GetComponentInChildren<Target>() != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _AI.GetComponentInChildren<Target>().transform.position, _speed * Time.deltaTime);
            if (transform.position == _AI.GetComponentInChildren<Target>().transform.position)
            {
                if (!isMelee)
                {
                    gameObject.GetComponent<ShotOnClickForAI>().enabled = true;
                }
                _animateForAI = false;
            }
        }

        if (_animate == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
            if (transform.position == _target.position)
            {
                if (!isMelee)
                {
                    gameObject.GetComponent<ShotOnClick>().enabled = true;
                }
                
                _animate = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.transform.parent == null)
        {
            if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 8) && wasInHands == true && gameObject.transform.parent == null)
            {
                if (collision.gameObject.layer == 8 && WasInHandAI == true)
                {
                    WasAI = true;
                }
                if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 8) && WasAI == false)
                {
                    
                    if (collision.gameObject.layer == 8 && collision.gameObject.GetComponentInParent<health>().IsDead == false && collision.gameObject.GetComponent<bones>()!=null)
                    {
                        part = collision.gameObject.GetComponent<bones>().GetComponentInParent<AI>();
                        if (part.gameObject.GetComponentInChildren<Item>() != null)
                        {
                            part.OnVikinut();
                        }
                        else if (part.gameObject.GetComponentInChildren<Item>() == null )
                        {
                            part.Stun();
                        }

                        if (IsLethal)
                        {
                            part.heal -= 3;
                        }

                        Destroyweapon();
                        if (part.heal <= 0)
                        {
                            part.GetComponent<health>().death(transform, force, collision.gameObject.GetComponent<Rigidbody>());
                            collision.gameObject.GetComponent<Rigidbody>().AddForce(part.transform.forward * force*collision.gameObject.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                        }

                    }
                    else if (collision.gameObject.layer == 9)
                    {
                        print(collision.gameObject.layer);
                        Destroyweapon();
                    }
                }
            }
        }
    }
    public void Destroyweapon()
    {
        des.ApplyDamage(50);
        if (isMelee)
        {
            GetComponentInChildren<DestoyWeaponMelee>().transform.rotation = transform.rotation;
            GetComponentInChildren<DestoyWeaponMelee>().transform.parent = null;
        }
        else
        {
            GetComponentInChildren<DestoyWeapon>().transform.rotation = transform.rotation;
            GetComponentInChildren<DestoyWeapon>().transform.parent = null;
        }
        Destroy(gameObject);
    }
}