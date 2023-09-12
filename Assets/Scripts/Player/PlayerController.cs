using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public float MoveForce { get; private set; } = 250f;
    [field: SerializeField] public float RunForce { get; private set; } = 280f;
    [field: SerializeField] public CharacterState Idle { get; private set; }
    [field: SerializeField] public CharacterState Walk { get; private set; }
    [field: SerializeField] public CharacterState Use { get; private set; }
    [field: SerializeField] public CharacterState Run { get; private set; }
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations { get; private set; }
    [field: SerializeField] public float WalkVelocityThreshold { get; private set; } = 0.05f;
    [field: SerializeField] public Animator Animator { get; private set; }
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
    private CharacterState currentState;
    private AnimationClip currentClip;
    private Vector2 facingDirection;
    private float timeToEndAnimation = 0f;
    private ActionBar actionBar;
    private bool isRunning = false;

    private bool isTargetBelow = false;
    private bool isTargetAbove = false;
    private bool isTargetLeft = false;
    private bool isTargetRight = false;

    private Vector2 DIRECTION_DOWN = new Vector2(0, -1);
    private Vector2 DIRECTION_LEFT = new Vector2(-1, 0);
    private Vector2 DIRECTION_RIGHT = new Vector2(1, 0);
    private Vector2 DIRECTION_UP = new Vector2(0, 1);

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        targetLock = GetComponentInChildren<TargetLock>();
        actionBar = GetComponentInChildren<ActionBar>();
        CurrentState = Idle;
    }

    void Awake() {
        InputSystem.Update();
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
                if (isRunning && !isMovingBackwards()) {
                    CurrentState = Run;
                } else {
                    CurrentState = Walk;
                }
            } else {
                CurrentState = Idle;
            }

            setFacingDirection(axisInput);
            ChangeClip();
        }
    }

    void ChangeClip() {
        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(CurrentState, facingDirection);

        if (currentClip == null || currentClip != expectedClip) {
            Animator.Play(expectedClip.name);
            currentClip = expectedClip;
        }
    }

    void FixedUpdate() {
        if (CurrentState.CanMove) {
            Vector2 moveForce;

            if (isRunning && !isMovingBackwards()) {
                moveForce = axisInput * RunForce * Time.fixedDeltaTime;
            } else {
                moveForce = axisInput * MoveForce * Time.fixedDeltaTime;
            }

            rb.AddForce(moveForce);
        }
    }

    private bool isMovingBackwards() {
        return
            (Target != null && axisInput.x == 1 && facingDirection.x == -1) ||
            (Target != null && axisInput.x == -1 && facingDirection.x == 1) ||
            (Target != null && axisInput.y == 1 && facingDirection.y == -1) ||
            (Target != null && axisInput.y == -1 && facingDirection.y == 1);
    }

    void OnRun(InputValue value) {
        isRunning = value.isPressed;
    }

    void setFacingDirection(Vector2 axisInput) {
        if (axisInput != Vector2.zero && Target != null) {
            Vector2 playerRelativeToTarget = transform.position - Target.transform.position;
            float x = playerRelativeToTarget.x;
            float y = playerRelativeToTarget.y;
 
            isTargetBelow = y > 0 && (x > -y && x < y);
            if (isTargetBelow) {
                facingDirection = DIRECTION_DOWN;
            }
    
            isTargetLeft = x > 0 && (y > -x && y < x);
            if(isTargetLeft) {
                facingDirection = DIRECTION_LEFT;
            }
    
            isTargetRight = x < 0 && (y > x && y < -x);
            if(isTargetRight) {
                facingDirection = DIRECTION_RIGHT;
            }
    
            isTargetAbove = y < 0 && (x > y && x < -y);
            if(isTargetAbove) {
                facingDirection = DIRECTION_UP;
            }

            return;
        }

        if (axisInput != Vector2.zero) {
            facingDirection = axisInput;
        }
    }

    void OnMove(InputValue value) {
        axisInput = value.Get<Vector2>();

        setFacingDirection(axisInput);
    }

    // Todo reimplement after creating action bar logic
    void OnUseTool() {
        if (CurrentState == Use || GameManager.IsPaused) {
            return;
        }
        
        // CurrentState = Use;

        // ActionBarAction action = actionBar.ActionSet.ElementAt(0).Value;
        
        // if (action != null) {
        //     ActionBarAction actionInstance = Instantiate(action, transform.position, Quaternion.identity);

        //     actionInstance.Execute();
        // }
    }

    void OnTargetSwitch() {
        if (GameManager.AvailableTargets.Count < 1) {
            Target = null;
        } else if (!target) {
            Target = GameManager.AvailableTargets[0];
        } else {
            Target = GameManager.AvailableTargets.Next();
        }

        targetLock.Target = Target;
    }

    void OnDodge() {
        if (CurrentState == Walk) {
            Vector2 moveForce = axisInput * (MoveForce * 16) * Time.fixedDeltaTime;

            rb.AddForce(moveForce);
        }
    }

    void OnAction1() {
        executeAction(0);
    }

    void OnAction2() {
        executeAction(1);
    }

    void OnAction3() {
        executeAction(2);
    }

    void OnAction4() {
        executeAction(3);
    }

    void OnAction5() {
        executeAction(4);
    }

    void OnAction6() {
        executeAction(5);
    }

    void OnAction7() {
        executeAction(6);
    }

    void OnAction8() {
        executeAction(7);
    }

    private void executeAction(int index) {
        Action action = actionBar.ActionSet.ElementAt(index).Value;

        if (action != null) {
            action.Execute();
        }
    }

    // Cast spell
    // void OnAction2() {
    //     if (Target != null) {
    //         Instantiate(SpellPrefab, transform);
    //     }
    // }

    void OnExit() {
        if (target) {
            target.GetComponentInChildren<TargetIndicator>().GetComponent<SpriteRenderer>().enabled = false;
            target = null;
            targetLock.Target = null;
        }
    }
}
