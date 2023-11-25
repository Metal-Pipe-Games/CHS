using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public PlayerEnemyAlerter alerter;
    public CameraShake cameraShake;
    PlayableDirector director;

    public float playerSpeed = 2.0f;
    public float sprintSpeed = 2.0f;
    public float stamina = 100;
    public float maxStamina = 100;
    public float staminaUse = 0.8f;
    public float staminaRecharge = 1.1f;
    public bool tired = false;
    private bool sprinting = false;
    public int jumpStaminaLoss = 50;

    public float bonusSpeed = 1;
    public float minimumSpeed = 0.5f;

    public float jumpHeight = 1.0f;
    public float gravityValue = -3f;
    public bool canJump = true;
    private float jumpCD = 2;
    private float yVelocity = 0;

    public GameObject head;
    public float sensitivityX = 1.0f;
    public float sensitivityY = 1.0f;

    public float tiltAngle = 10;

    public bool isLocked = true;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        director = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (!isLocked)
        {
            float sprint = 1;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            bool aired = !controller.isGrounded;

            Vector3 rotate = gameObject.transform.eulerAngles + new Vector3(-mouseY * sensitivityY, mouseX * sensitivityX, 0);
            if (rotate.x > 90 && rotate.x <= 180)
            {
                rotate.x = 90;
            }
            else if (rotate.x < 270 && rotate.x > 180)
            {
                rotate.x = -90;
            }

            //Debug.Log(rotate);
            gameObject.transform.eulerAngles = rotate;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            if (controller.isGrounded)
            {
                yVelocity = -0.001f;
                if (Input.GetButtonDown("Jump") && canJump && jumpCD <= 0)
                {
                    yVelocity = jumpHeight * (stamina < jumpStaminaLoss ? 0.5f : 1);
                    stamina -= (stamina < jumpStaminaLoss ? jumpStaminaLoss / 10 : jumpStaminaLoss);
                    jumpCD = 1;
                }

                if (!sprinting && stamina < maxStamina)
                {
                    stamina += staminaRecharge * Time.deltaTime;
                    if (stamina > 50) { tired = false; canJump = true; }
                }

                if (Input.GetButton("Sprint") && stamina > 0 && !tired)
                {
                    sprinting = true;
                    sprint = sprintSpeed;
                    stamina -= staminaUse * Time.deltaTime;
                }
                else
                {
                    sprinting = false;
                }
                if (stamina <= 0) { tired = true; canJump = false; }
            }
            else
            {
                yVelocity += gravityValue * Time.deltaTime;
            }

            if (jumpCD > 0) jumpCD -= Time.deltaTime;

            move.y = yVelocity;
            Vector3 old = transform.position;
            Vector3 final = playerSpeed * bonusSpeed * Time.deltaTime * new Vector3(move.x * sprint, move.y, move.z * sprint);
            controller.Move(final);
            
            cameraShake.Shake(Vector3.Distance(transform.position,old), aired, controller.isGrounded);

            if (move.x != 0 && move.z != 0 && !aired)
            {
                if (sprinting)
                {
                    alerter.Alert(2);
                }
                else
                {
                    alerter.Alert(0);
                }
            }

            if (aired && controller.isGrounded)
            {
                alerter.Alert(1);
            }
        }
    }

    public void SetBonusSpeed(float speed)
    {
        bonusSpeed = Mathf.Max(speed, minimumSpeed);
    }

    public void SetLocked(int v)
    {
        isLocked = v == 0;
    }

    public void Enable()
    {
        isLocked = false;
        GetComponent<ToolHandler>().active = true;
        if(director != null)
        {
            director.Stop();
            director.time = 0;
            director.enabled = false;
        }

        //Debug.Log("Enabled");
    }

    public void Disable()
    {
        isLocked = true;
        GetComponent<ToolHandler>().enabled = false;
    }
}