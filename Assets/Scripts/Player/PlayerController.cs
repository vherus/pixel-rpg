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
    [field: SerializeField] public Spell SpellPrefab;

    public Enemy Target {
        get { return target; }

        private set {
            if (target != null) {
                TargetIndicator tar = target.GetComponentInChildren<TargetIndicator>();
                tar.GetComponent<SpriteRenderer>().enabled = false;
            }

            if (value != null) {
                TargetIndicator ti = value.GetComponentInChildren<TargetIndicator>();
                ti.GetComponent<SpriteRenderer>().enabled = true;
            }

            target = value;
        }
    }

    private Enemy target;
    private TargetLock targetLock;

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
        targetLock = GetComponentInChildren<TargetLock>();
        CurrentState = Idle;
    }

    void Update()
    {
        if (GameManager.IsPaused) {
            return;
        }

        if (GameManager.AvailableTargets.Count < 1 || (Target && !GameManager.AvailableTargets.Contains(Target))) {
            Target = null;
            targetLock.Target = null;
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

    // Todo, change this signature and key bind
    // This function cycles through available targets in the order they were added to the available target list
    void OnAction2() {
        if (GameManager.AvailableTargets.Count < 1) {
            Target = null;
        } else if (!target) {
            Target = GameManager.AvailableTargets[0];
        } else {
            Target = GameManager.AvailableTargets.Next();
        }

        targetLock.Target = Target;
    }

    // Dodge
    void OnAction3() {
        if (CurrentState == Walk) {
            Vector2 moveForce = axisInput * (MoveForce * 16) * Time.fixedDeltaTime;
            rb.AddForce(moveForce);
        }
    }

    // Cast spell
    void OnAction4() {
        if (Target != null) {
            Instantiate(SpellPrefab, transform);
        }
    }

    // Clear target
    void OnAction8() {
        if (target) {
            target.GetComponentInChildren<TargetIndicator>().GetComponent<SpriteRenderer>().enabled = false;
            target = null;
            targetLock.Target = null;
        }
    }

    void OnActionPress() {
        OnUse();
    }
}
