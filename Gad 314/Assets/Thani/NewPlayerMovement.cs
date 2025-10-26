 using UnityEditor.ShaderGraph;
using UnityEngine;

public class NewPlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float walkSpeed = 3, walkBackSpeed = 2;
    public float runSpeed = 7, runBackSpeed = 5;
    public float crouchSpeed = 2, crouchBackSpeed = 1;

    public Vector3 direc;
    public float horizInput, vertiInput;
    CharacterController control;

    public float groundOffset;
    public LayerMask groundMask;
    Vector3 spherePos;

    public float gravity = -0.01f;
    Vector3 velocity;

    NewPlayerBase curState;
    public Idle Idle = new Idle();
    public Walking Walk = new Walking();
    public Running Run = new Running();
    public Crouching Crouch = new Crouching();

    void Start()
    {
        control = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void Update()
    {
        GetDirectionAndMove();
        Gravity();

        curState.UpdateState(this);
    }

    public void SwitchState(NewPlayerBase state)
    {
        curState = state;
        curState.EnterState(this);
    }

    void GetDirectionAndMove()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertiInput = Input.GetAxis("Vertical");

        direc = transform.forward * vertiInput + transform.right * horizInput;

        control.Move(direc * moveSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, control.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        control.Move(velocity * Time.deltaTime);
    }
}
