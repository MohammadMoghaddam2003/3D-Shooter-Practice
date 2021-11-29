using System.Collections;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Range(0f, 20f)]
    public float WalkSpeed = 6.5f, RunSpeed = 10f, CrouchingWalkSpeed = 2f, JumpForce = 3f, CrouchingSpeed = 3f
    , GravityForce = 10f;

    private CharacterController _playerCharacterController;
    private Transform _cameraTransform;
    private Animator _playerAnimator;
    private Vector3 _moveDirection;
    private bool _isGrounded = false, _isCrouching, _isWalking;
    private float _inputX = 0, _inputZ = 0, _inputModifyFactor, _speed, _playerCapsuleColliderDefaultHeight
    , _playerCapsuleColliderSetHeight, _cameraDefaultHeight, _cameraSetHeight;




    void Start()
    {
        _speed = WalkSpeed;
        _cameraTransform = GameObject.Find("Main Camera").transform;
        _cameraDefaultHeight = _cameraTransform.localPosition.y;
        _playerAnimator = GetComponent<Animator>();
        _playerCharacterController = GetComponent<CharacterController>();
        _playerCapsuleColliderDefaultHeight = _playerCharacterController.height;
    }

    void Update()
    {
        PlayerMovement();
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



        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");



        _inputModifyFactor = (_inputX != 0 && _inputZ != 0) ? .5f : 1;
        _moveDirection = new Vector3((_inputX * _inputModifyFactor) * Time.deltaTime, 0, (_inputZ * _inputModifyFactor) * Time.deltaTime);
        _moveDirection -= new Vector3(0f, GravityForce * Time.deltaTime, 0f);
        _moveDirection = transform.TransformDirection(_moveDirection * _speed);
        _isGrounded = (_playerCharacterController.Move(_moveDirection) & CollisionFlags.Below) != 0f;



        if (_inputX != 0f || _inputZ != 0f)
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }

        _playerAnimator.SetFloat("Walking", _playerCharacterController.velocity.magnitude);

        _playerAnimator.SetBool("IsWalking", _isWalking);


        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_isCrouching)
        {
            JumpingPlayer();
        }
    }

    void JumpingPlayer()
    {
        _moveDirection += new Vector3(0f, JumpForce * Time.deltaTime, 0f);
    }

    IEnumerator SetCrouching()
    {
        _playerCapsuleColliderSetHeight = _isCrouching ? _playerCapsuleColliderDefaultHeight / 2 : _playerCapsuleColliderDefaultHeight;
        _cameraSetHeight = _isCrouching ? _cameraDefaultHeight / 2 : _cameraDefaultHeight;

        while (Mathf.Abs(_playerCharacterController.height - _playerCapsuleColliderSetHeight) > 0.0001f)
        {
            _playerCharacterController.height = Mathf.Lerp(_playerCharacterController.height, _playerCapsuleColliderSetHeight, CrouchingSpeed * Time.deltaTime);
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, new Vector3(_cameraTransform.localPosition.x, _cameraSetHeight, _cameraTransform.localPosition.z), CrouchingSpeed * Time.deltaTime);
        }

        yield return null;
    }
}

