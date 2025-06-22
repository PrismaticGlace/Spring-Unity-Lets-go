using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Utilities;

public class PlayerMovement : MonoBehaviour {

    //Refs
    public CharacterController cc;
    public Transform playMesh;
    public PlayerInput pli;
    private PlayerInputActions plina;
    public GameManager gameMana;

    //Controls
#pragma warning disable IDE0052 // Remove unread private members
    private ControlType currContType;
#pragma warning restore IDE0052 // Remove unread private members
    private enum ControlType {
        KeyboardAndMouse,
        Keyboard,
    }

    [Space]
    //Speed Variables
    [SerializeField, Range(0.0f, 15.0f)] private float speed;
    [SerializeField, Range(0.0f, 20.0f)] private float turnSpeed;
    [SerializeField, Range(0.0f, 750.0f)] private float turnRotateSpeed;

    [Space]
    //Jump Variables
    [SerializeField] private float gravity = -9.81f;
    [SerializeField, Range(0.0f, 30.0f)] private float jumpheight;
    [SerializeField, Range(0.0f, 10.0f)] private float jumpVeloFallOff;
    [SerializeField, Range(0.0f, 10.0f)] private float fallMulti;
    [SerializeField, Range(0.0f, 10.0f)] private float lowJumpMulti;
    [SerializeField, Range(0.0f, 30.0f)] private float termVelo;
    //Smaller Jumps
    [SerializeField, Range(0.0f, 1.0f)] private float jumpBufferTime;
    private float jumpBufferCounter;
    //Health and Such
    [Space]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float lastTakeDamage;
    [SerializeField] private float invFrames;
    [Space]
    //Input Dampening
    [SerializeField, Range(0.0f, 2.0f)] private float inDampRotate;
    [SerializeField, Range(0.0f, 1.0f)] private float inDampMoveBasic;
    [SerializeField, Range(0.0f, 1.0f)] private float inDampAccel;
    [Range(0.0f, 1.0f)] public float inDampDeccel;
    [SerializeField, Range(0.0f, 1.0f)] private float inDampTurn;
    [SerializeField, Range(0.0f, 1.0f)] private float airDampMove;
    [SerializeField, Range(0.0f, 1.0f)] private float airDampRotate;
    private float currDampMove;
    private float currDampRotate;

    //Vectors
    private Vector3 gravityVect;
    public Vector3 velocity;
    private Vector3 inVectorRotate;
    private Vector3 inVelocityRotate;
    private Vector3 inVectorMove;
    private Vector3 inVelocityMove;

    [Space]
    //Booleans 
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isAttacking;
    public bool currWeapon;
    public GameObject swordObj;
    public GameObject spearObj;

    //Getters and Setters for Health and Lives
    public float getHeath() {
        return health;
    }

    public void addHealth() {
        health += 1;
    }

    private void Awake() {
        plina = new PlayerInputActions();
        getCurrentControls(pli);
        gameMana = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
    }
    private void OnEnable() {
        plina.Player.Enable();
        plina.Player.Move.started += MoveStarted;
    }

    private void OnDisable() {
        plina.Player.Move.started -= MoveStarted;
        plina.Player.Disable();
    }

    // Update is called once per frame
    void Update() {
        //Checking if it is Grounded
        isGrounded = cc.isGrounded;
        currDampRotate = inDampRotate;

        Vector3 input = new Vector3(plina.Player.Move.ReadValue<Vector2>().x, 0.0f, plina.Player.Move.ReadValue<Vector2>().y);

        if (!plina.Player.Move.IsInProgress() && isGrounded) {
            currDampMove = inDampDeccel;
        }

        if (plina.Player.Move.IsInProgress() && inVectorMove.toIso().normalized != Vector3.zero) {
            float dot = Vector3.Dot(input.toIso().normalized, inVectorMove.toIso().normalized);
            if (dot > 0.0f) {
                currDampMove = isGrounded ? inDampMoveBasic : inDampMoveBasic + airDampMove;
                currDampRotate = isGrounded ? inDampRotate : inDampRotate + airDampRotate;
            }
            else if (dot <= 0.0f) {
                currDampMove = isGrounded ? inDampTurn : inDampTurn + airDampMove;
                currDampRotate = isGrounded ? currDampRotate : inDampRotate;
            }
        }

        if (!plina.Player.Move.IsInProgress() && inVectorMove.magnitude < 0.01f) {
            currDampMove = 0;
        }
        else {
            currDampMove = inDampDeccel;

        }

        //Smooth Interpolate Rotation
        inVectorRotate = Vector3.SmoothDamp(inVectorRotate, input, ref inVectorRotate, currDampRotate, isGrounded ? Mathf.Infinity : 6);

        //Smooth Interpolate Movement
        inVectorMove = Vector3.SmoothDamp(inVectorMove, input, ref inVectorMove, currDampMove, 10f);

        //Debug Rays
        Debug.DrawRay(transform.position, input.toIso() * 3, Color.green);
        Debug.DrawRay(transform.position, inVectorMove.toIso() * 3, Color.red);
        Debug.DrawRay(transform.position, inVelocityMove.toIso() * 3, Color.blue);
        Debug.DrawRay(transform.position, velocity, Color.green);

        //Don't move the player if the input is not pressed and movement isnt being smoothed
        if (inVectorRotate != Vector3.zero) {
            Quaternion targetRot = Quaternion.LookRotation(inVectorRotate.toIso(), Vector3.up); // Returns target rotation.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnRotateSpeed * Time.deltaTime);

            playMesh.rotation = Quaternion.RotateTowards(playMesh.rotation, targetRot, 100.0f * Time.deltaTime);
        }
        velocity.x = inVectorMove.toIso().x * speed;
        if (velocity.x > 12) {
            velocity.x = 12;
        }
        else if (velocity.x < -12) {
            velocity.x = -12;
        }
        velocity.z = inVectorMove.toIso().z * speed;
        if (velocity.z > 12) {
            velocity.z = 12;
        }
        else if (velocity.z < -12) {
            velocity.z = -12;
        }

        if (isGrounded) {
            if (isJumping) {
                isJumping = false;
                if (velocity.y < 0.0f) {
                    velocity.y = 0.0f;
                }
            }
        }

        //Jump Calcs
        jumpBufferCounter -= Time.deltaTime;
        if (plina.Player.Jump.WasPressedThisFrame() && isGrounded) {
            isJumping = true;
            jumpBufferCounter = jumpBufferTime;
        }

        if (jumpBufferCounter > 0.0f) {
            isJumping = true;
            isGrounded = false;
            jumpBufferCounter = 0.0f;
            velocity.y = jumpheight;
        }
        else {
            isGrounded = true;
        }

        //Gravity
        if (velocity.y > 0.0f && !plina.Player.Jump.IsPressed()) {
            gravityVect.y = (gravity * lowJumpMulti);
        }
        else if (velocity.y < jumpVeloFallOff) {
            gravityVect.y = (gravity * fallMulti);
        }
        else {
            gravityVect.y = gravity;
        }

        //Attacking
        if (plina.Player.Attack.WasPressedThisFrame() && !isAttacking) {
            if (currWeapon) {
                swordObj.SetActive(true);
            }
            else {
                spearObj.SetActive(true);
            }
        }
        lastTakeDamage -= Time.deltaTime;

        velocity.y += (gravityVect.y) * Time.deltaTime;

        velocity.y = Mathf.Clamp(velocity.y, -termVelo, termVelo);

        cc.Move(velocity * Time.deltaTime);

    }

    private void getCurrentControls(PlayerInput input) {
        if (input.currentControlScheme == "Keyboard") {
            currContType = ControlType.Keyboard;
        }
        else if (input.currentControlScheme == "KeyboardAndMouse") {
            currContType = ControlType.KeyboardAndMouse;
        }
    }

    private void MoveStarted(InputAction.CallbackContext ctx) {
        currDampMove = inDampAccel;
    }

    public virtual void TakePlayerDamage(int damageValue) {
        if (lastTakeDamage < 0) {
            health -= damageValue;
            lastTakeDamage = invFrames;
        }
        if (health <= 0) {
            gameMana.TakeLife();
            gameObject.SetActive(false);
        }
    }

    public void PlayerSaveData() {
        LoadnSave.GameStateData.PlayerData data = GameManager.StateManager.gameState.playerData;

        data.collectWeapon = currWeapon;
        data.playerHealth = health;

        data.posRotSca.posX = transform.position.x;
        data.posRotSca.posY = transform.position.y;
        data.posRotSca.posZ = transform.position.z;

        data.posRotSca.rotX = transform.rotation.x;
        data.posRotSca.rotY = transform.rotation.y;
        data.posRotSca.rotZ = transform.rotation.z;

        data.posRotSca.scaleX = transform.localScale.x;
        data.posRotSca.scaleY = transform.localScale.y;
        data.posRotSca.scaleZ = transform.localScale.z;
    }

    public void PlayerLoadData() {
        LoadnSave.GameStateData.PlayerData data = GameManager.StateManager.gameState.playerData;

        currWeapon = data.collectWeapon;
        health = data.playerHealth;

        transform.position = new Vector3(data.posRotSca.posX, data.posRotSca.posY, data.posRotSca.posZ);
        transform.localRotation = Quaternion.Euler(data.posRotSca.rotX, data.posRotSca.rotY, data.posRotSca.rotZ);
        transform.localScale = new Vector3(data.posRotSca.scaleX, data.posRotSca.scaleY, data.posRotSca.scaleZ);
    }
}