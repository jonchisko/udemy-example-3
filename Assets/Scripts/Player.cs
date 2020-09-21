using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public delegate void AmmoEvent(int value);
    public AmmoEvent OnAmmoChange;
    public delegate void CoinEvent();
    public CoinEvent OnCoinPickUp;


    public ParticleSystem muzzleFlash;

    private Transform _playerCamera;
    private CharacterController _controller;

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _gravityScale = 9.8f;
    [SerializeField]
    private float _lookSensitivity = 1.0f;

    [SerializeField]
    private GameObject _hitMarker;
    [SerializeField]
    private AudioSource _aSource;
    [SerializeField]
    private AudioClip _aClip;

    private int _currentAmmo;
    private int _maxAmmo = 60;
    private bool _isReloading = false;

    private int _coin = 0;
    public int Coin
    {
        set
        {
            Debug.Log("SETTING COING");
            _coin = value;
            OnCoinPickUp?.Invoke();
        }
        get
        {
            return _coin;
        }
    }

    [SerializeField]
    private GameObject _weapon;

    // Start is called before the first frame update
    private void Awake()
    {
        _currentAmmo = _maxAmmo;
    }
    void Start()
    {
        _playerCamera = GameObject.Find("Main Camera")?.transform;
        if (_playerCamera == null)
        {
            Debug.LogError("Player::Start() -> _playerCamera is null.");
        }
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("Player::Start() -> _controller is null.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        muzzleFlash.Stop();
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        CalculateLook();
        ShowCursor();
        if(_weapon.activeSelf) WeaponFunctions();
    }

    void WeaponFunctions()
    {
        FireGun();
        ReloadGun();
    }


    void CalculateMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        Vector3 movement = direction * _speed;
        movement.y -= _gravityScale;
        movement = transform.TransformDirection(movement);
        _controller.Move(movement * Time.deltaTime);
    }

    void CalculateLook()
    {
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y += Input.GetAxis("Mouse X") * _lookSensitivity;
        transform.localEulerAngles = newRotation;

        newRotation = _playerCamera.transform.localEulerAngles;
        newRotation.x -= Input.GetAxis("Mouse Y") * _lookSensitivity;
        _playerCamera.transform.localEulerAngles = newRotation;
    }

    void ShowCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void FireGun()
    {
        _aSource.clip = _aClip;
        if (Input.GetKey(KeyCode.Mouse0) && _currentAmmo > 0)
        {
            _currentAmmo--;
            OnAmmoChange?.Invoke(_currentAmmo);
            if (!_aSource.isPlaying) _aSource.Play();
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitObject;
            if (Physics.Raycast(rayOrigin, out hitObject))
            {
                Debug.Log("We hit: " + hitObject.transform.name);
                GameObject instantiatedHitMarker = Instantiate(_hitMarker, hitObject.point, Quaternion.LookRotation(hitObject.normal));
                DestructableCreate d = hitObject.transform.GetComponent<DestructableCreate>();
                if(d != null)
                {
                    d.DestroyCrate();
                }
                Destroy(instantiatedHitMarker.gameObject, 0.3f);
            }
            if (!muzzleFlash.isPlaying) muzzleFlash.Play();
        }
        else
        {
            if(_aSource.isPlaying) _aSource.Stop();
            if(muzzleFlash.isPlaying) muzzleFlash.Stop();
        }
    }

    void ReloadGun()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading)
        {
            _isReloading = true;
            StartCoroutine("ReloadCoroutine");
        }
    }

    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        _currentAmmo = _maxAmmo;
        OnAmmoChange?.Invoke(_currentAmmo);
        _isReloading = false;
    }

    public int GetCurrentAmmo()
    {
        return _currentAmmo;
    }
    public int GetMaxAmmo()
    {
        return _maxAmmo;
    }

    public void EnableWeapon()
    {
        _weapon.SetActive(true);
    }

}
