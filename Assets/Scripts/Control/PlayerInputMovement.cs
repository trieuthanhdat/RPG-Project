using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInputMovement : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    [SerializeField]
    private InputActionAsset InputActions;
    private InputActionMap PlayerActionMap;
    private InputAction Movement;
#endif
    [SerializeField] private Camera Camera;
    [SerializeField] private float TargetLerpSpeed = 1;
    [SerializeField]
    [Range(0, 0.99f)]
    private float Smoothing = 0.25f;

    private NavMeshAgent Agent;
    private Vector3 TargetDirection;
    private float LerpTime = 0;
    private Vector3 LastDirection;
    private Vector3 MovementVector;
    private Mover mover;

    private void Awake()
    {
        mover = GetComponent<Mover>();
        Agent = GetComponent<NavMeshAgent>();

        PlayerActionMap = InputActions.FindActionMap("Player");
        Movement = PlayerActionMap.FindAction("Move");
        Movement.started += HandleMovementAction;
        Movement.canceled += HandleMovementAction;
        Movement.performed += HandleMovementAction;
        Movement.Enable();
        PlayerActionMap.Enable();
        InputActions.Enable();
    }

    private void HandleMovementAction(InputAction.CallbackContext Context)
    {
        Vector2 input = Context.ReadValue<Vector2>();
        MovementVector = new Vector3(input.x, 0, input.y);
    }

    private void Update()
    {
        mover.UpdateAnimator();
        // DoNewInputSystemMovement();
        DoOldInputSystemMovement();
    }

    private void DoNewInputSystemMovement()
    {
        MovementVector.Normalize();
        if (MovementVector != LastDirection)
        {
            LerpTime = 0;
        }
        LastDirection = MovementVector;
        TargetDirection = Vector3.Lerp(
            TargetDirection, 
            MovementVector, 
            Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
        );

        Agent.Move(TargetDirection * Agent.speed * Time.deltaTime);

        Vector3 lookDirection = MovementVector;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.LookRotation(lookDirection), 
                Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
            );
        }

        LerpTime += Time.deltaTime;
    }

    private void DoOldInputSystemMovement()
    {
        MovementVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            MovementVector += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MovementVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MovementVector += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            MovementVector += Vector3.back;
        }

        MovementVector.Normalize();
        if (MovementVector != LastDirection)
        {
            LerpTime = 0;
        }
        LastDirection = MovementVector;

        TargetDirection = Vector3.Lerp(
            TargetDirection, 
            MovementVector, 
            Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
        );

        Agent.Move(TargetDirection * Agent.speed * Time.deltaTime);
        Vector3 lookDirection = MovementVector.normalized;
        if (lookDirection != Vector3.zero)
        {
            Agent.transform.rotation = Quaternion.Lerp(
                transform.rotation, 
                Quaternion.LookRotation(lookDirection), 
                Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
            );
        }

        LerpTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        Camera.transform.position = transform.position + Vector3.up * 10;
    }
}
