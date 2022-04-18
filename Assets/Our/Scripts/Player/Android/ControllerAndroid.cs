using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ControllerAndroid : MonoBehaviour
{
    [Header("PlayerMover")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _gravity = -22f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _timeWhenStand;
    [SerializeField] private FloatingJoystick JoystickWalk;
    [SerializeField] public GameObject menu;
    [SerializeField] public GameObject Mouse;

    [Header("CameraMover")]
    [SerializeField] private float mouseSensitiviti;
    [SerializeField] private Camera _camera;

    //////////////////////////////////////////////////////////////////////////////////////////////
    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;
    [HideInInspector] public bool weaponized;
    // private variables
    private Item item;
    private PickUpPrior prior;
    private float sens;
    private GameObject other;
    private float Horizontal;
    private float Vertical; 
    private float _xRotation = 0f;
    private float pushPower = 2.0f;
    private bool _isGrounded;
    private Vector3 _move;
    private Vector3 _velocity;
    private ThrowAwayWeaponAndroid Throw;
    private float _timeWhenStandAndMouseMove => _timeWhenStand * 2f;
    private float lastTime;

    //////////////////////////////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        sens = mouseSensitiviti;
        Time.fixedDeltaTime = 0.02f;
        prior = gameObject.GetComponent<PickUpPrior>();
    }
    private void Start()
    {
        Throw = gameObject.GetComponent<ThrowAwayWeaponAndroid>();
        weaponized = false;
        GameObject.Find("InteractButton").SetActive(false);
        Time.timeScale = _timeWhenStand;
        Time.fixedDeltaTime *= Time.timeScale;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionalMenu();
        }

        if (Throw.enabled == false)
        {
            GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>().enabled = true;
            GameObject.Find("GameOverText").GetComponent<TextMeshProUGUI>().text = "гаме овер";
            _speed = 0;
            mouseSensitiviti = 0;
            Time.timeScale = 0;
            if (Input.anyKeyDown)
            {
                Time.timeScale = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }
        }
    }
    private void OnTriggerEnter(Collider othir)
    {
        other = othir.gameObject;
        item = other.GetComponent<Item>();
        if (other.tag == "Weapons")
        {
            if (weaponized == false)
            {
                if (item.wasInHands == false && item._isInHend == false && item._isInHendAI == false)
                {
                    if (prior.prior.Contains(other) == false)
                    {
                        prior.prior.Add(other);
                    }
                }
            }
        }
        if (prior.prior.Count == 1)
        {
            prior.interactButton.gameObject.SetActive(true);
            prior._myImage.enabled = true;
        }
    }
    private void OnTriggerExit(Collider othir)
    {
        other = othir.gameObject;
        item = other.GetComponent<Item>();
        if (other.tag == "Weapons")
        {
            if (prior.prior.Contains(other) == true)
            {
                prior.prior.Remove(other);
                item._meshRenderer.material = item._materialStandart;
            }

        }
        if (prior.prior.Count == 0)
        {

            prior.interactButton.gameObject.SetActive(false);
            prior._myImage.enabled = false;
            
        }
    }



    private void OnTriggerStay(Collider othir)
    {
        if (othir.gameObject.tag == "Weapons" && weaponized==false)
        {
            prior.MinDist();
        }
    }
    




    private void FixedUpdate()
    {

        //////////////////////////////////////////////////////////////////////////////////////////////
        // Mouse Mover logic
        mouseX = mouseX * Time.fixedDeltaTime * mouseSensitiviti;
        mouseY = mouseY * Time.fixedDeltaTime * mouseSensitiviti;


        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        //////////////////////////////////////////////////////////////////////////////////////////////

        // Player Mover and timeScale logic
        Horizontal = JoystickWalk.Horizontal;
        Vertical = JoystickWalk.Vertical;


        if (_isGrounded == true && Horizontal == 0 && Vertical == 0 && (mouseX != 0 || mouseY != 0))
        {
            Time.timeScale = _timeWhenStandAndMouseMove;
            if (lastTime != Time.timeScale)
            {
                mouseSensitiviti = sens;
                mouseSensitiviti /= Time.timeScale;
                Time.fixedDeltaTime = 0.02f;
                Time.fixedDeltaTime *= Time.timeScale;
                lastTime = Time.timeScale;
            }



        }

        if (_isGrounded == false || Horizontal != 0 || Vertical != 0)
        {
            Time.timeScale = 1;
            if (lastTime != Time.timeScale)
            {
                mouseSensitiviti = sens;
                mouseSensitiviti /= Time.timeScale;
                Time.fixedDeltaTime = 0.02f;
                Time.fixedDeltaTime *= Time.timeScale;
                lastTime = Time.timeScale;
            }
        }

        if (_isGrounded == true && Horizontal == 0 && Vertical == 0 && mouseX == 0 && mouseY == 0)
        {
            Time.timeScale = _timeWhenStand;
            if (lastTime != Time.timeScale)
            {
                mouseSensitiviti = sens;
                mouseSensitiviti /= Time.timeScale;
                Time.fixedDeltaTime = 0.02f;
                Time.fixedDeltaTime *= Time.timeScale;
                lastTime = Time.timeScale;
            }
        }

        _move = transform.right * Horizontal + transform.forward * Vertical;
        _characterController.Move(_move * _speed * Time.fixedDeltaTime);

        _velocity.y += _gravity * Time.fixedDeltaTime;
        _characterController.Move(_velocity * Time.fixedDeltaTime);


        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
    }
    public void NormalTime()
    {
        JoystickWalk.gameObject.SetActive(true);
        Time.timeScale = 1;
    }
    public void ToMain()
    {
        SceneManager.LoadScene(0);
    }
    public void OptionalMenu()
    {
        JoystickWalk.gameObject.SetActive(false);
        menu.SetActive(true);
        Mouse.SetActive(false);
        Time.timeScale = 0;
    }

    // чтобы толкать дверь
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}