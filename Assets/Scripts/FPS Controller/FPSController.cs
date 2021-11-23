using System.Collections;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Range(0f, 20f)]
    public float WalkSpeed = 6.5f, RunSpeed = 10f, CrouchingWalkSpeed = 2f, JumpForce = 3f, CrouchingSpeed = 3f;

    private Rigidbody _playerRigidbody;
    private BoxCollider _boxColliderForCanJumping;
    private CapsuleCollider _playerCapsuleCollider;
    private Transform _cameraTransform;
    private bool _canJumping = false, _isCrouching;
    private float _inputX = 0, _inputZ = 0, _inputModifyFactor, _speed, _playerCapsuleColliderDefaultHeight
    , _playerCapsuleColliderSetHeight, _cameraDefaultHeight, _cameraSetHeight;




    void Start()
    {
        _speed = WalkSpeed;
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _cameraDefaultHeight = _cameraTransform.localPosition.y;
        _playerRigidbody = GetComponent<Rigidbody>();
        _boxColliderForCanJumping = GetComponent<BoxCollider>();
        _playerCapsuleCollider = GetComponent<CapsuleCollider>();
        _playerCapsuleColliderDefaultHeight = _playerCapsuleCollider.height;
    }

    void Update()
    {
        PlayerMovement();
    }

    void OnTriggerEnter(Collider Ground)
    {
        if (Ground.CompareTag("Ground"))
        {
            _canJumping = true;
        }
    }

    void PlayerMovement()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouching = !_isCrouching;
            StopCoroutine(SetCrouching());
            StartCoroutine(SetCrouching());

            if (_isCrouching)
            {
                _speed = CrouchingWalkSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!_isCrouching)
            {
                _speed = RunSpeed;
            }
        }
        else
        {
            if (!_isCrouching)
            {
                _speed = WalkSpeed;
            }
        }



        _inputX = Input.GetAxis("Horizontal") * _speed;
        _inputZ = Input.GetAxis("Vertical") * _speed;

        _inputModifyFactor = (_inputX != 0 && _inputZ != 0) ? .5f : 1;

        transform.Translate(new Vector3((_inputX * _inputModifyFactor) * Time.deltaTime, 0, (_inputZ * _inputModifyFactor) * Time.deltaTime));



        if (Input.GetKeyDown(KeyCode.Space) && _canJumping && !_isCrouching)
        {
            JumpingPlayer();
        }
    }

    void JumpingPlayer()
    {
        _playerRigidbody.velocity = new Vector3(0, JumpForce * Time.deltaTime, 0);
    }

    IEnumerator SetCrouching()
    {
        _playerCapsuleColliderSetHeight = _isCrouching ? _playerCapsuleColliderDefaultHeight / 2 : _playerCapsuleColliderDefaultHeight;
        _cameraSetHeight = _isCrouching ? _cameraDefaultHeight / 2 : _cameraDefaultHeight;

        while (Mathf.Abs(_playerCapsuleCollider.height - _playerCapsuleColliderSetHeight) > 0.0001f)
        {
            _playerCapsuleCollider.height = Mathf.Lerp(_playerCapsuleCollider.height, _playerCapsuleColliderSetHeight, CrouchingSpeed * Time.deltaTime);
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, new Vector3(_cameraTransform.localPosition.x, _cameraSetHeight, _cameraTransform.localPosition.z), CrouchingSpeed * Time.deltaTime);
        }

        yield return null;
    }
}
