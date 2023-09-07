using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public float MoveForce { get; private set; } = 250f;
    [field: SerializeField] public CharacterState Idle { get; private set; }
    [field: SerializeField] public CharacterState Walk { get; private set; }
    [field: SerializeField] public CharacterState Use { get; private set; }
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations { get; private set; }
    [field: SerializeField] public float WalkVelocityThreshold { get; private set; } = 0.05f;

    public CharacterState CurrentState {
        get {
            return currentState;
        }

        private set {
            if (currentState != value) {
                currentState = value;
                ChangeClip();
                timeToEndAnimation = currentClip.length;
            }
        }
    }

    private Vector2 axisInput = Vector2.zero;
    private Rigidbody2D rb;
    private Animator animator;
    private CharacterState currentState;
    private AnimationClip currentClip;
    private Vector2 facingDirection;
    private ActionBar actionBar;
    private float timeToEndAnimation = 0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        actionBar = GetComponent<ActionBar>();
        CurrentState = Idle;
    }

    void Update()
    {
        if (GameManager.IsPaused) {
            return;
        }

        timeToEndAnimation = Mathf.Max(timeToEndAnimation - Time.deltaTime, 0);

        if (CurrentState.CanExitWhilePlaying || timeToEndAnimation <= 0) {
            if (axisInput != Vector2.zero && rb.velocity.magnitude > WalkVelocityThreshold) {
                CurrentState = Walk;
            } else {
                CurrentState = Idle;
            }

            ChangeClip();
        }
    }

    void ChangeClip() {
        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(CurrentState, facingDirection);

        if (currentClip == null || currentClip != expectedClip) {
            animator.Play(expectedClip.name);
            currentClip = expectedClip;
        }
    }

    void FixedUpdate() {
        if (CurrentState.CanMove) {
            Vector2 moveForce = axisInput * MoveForce * Time.fixedDeltaTime;
            rb.AddForce(moveForce);
        }
    }

    void OnMove(InputValue value) {
        axisInput = value.Get<Vector2>();

        if (axisInput != Vector2.zero) {
            facingDirection = axisInput;
        }
    }

    void OnUse() {
        if (GameManager.IsPaused) {
            return;
        }
        
        CurrentState = Use;
    }

    void OnAction1() {
        if (CurrentState == Use || GameManager.IsPaused) {
            return;
        }

        ActionBarAction action = actionBar.ActionSet.ElementAt(0).Value;
        
        if (action != null) {
            ActionBarAction actionInstance = Instantiate(action, transform.position, Quaternion.identity);

            actionInstance.Execute();
        }

        OnActionPress();
    }

    void OnActionPress() {
        OnUse();
    }
}
