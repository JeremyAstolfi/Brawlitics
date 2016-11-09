using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    [SerializeField]
    private float f_playerSpeed;

    [SerializeField]
    private float f_jumpHeight;

    [SerializeField]
    private float f_gravity;

    [SerializeField]
    private float f_lerpTime;

    [SerializeField]
    private Vector2 v2_playerVelocity;

    private PlayerPhysicsScript script_playerPhysics;

	// Use this for initialization
	void Start ()
    {
        v2_playerVelocity = Vector2.zero;

        script_playerPhysics = GetComponent<PlayerPhysicsScript>();

        f_gravity = script_playerPhysics.GetGravity;

        InitializeNullVariables();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UserInput();

        transform.position = Vector2.Lerp(transform.position, v2_playerVelocity, f_lerpTime);
	}

    private void UserInput()
    {
        v2_playerVelocity.x += Input.GetAxis("Horizontal") * f_playerSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && !script_playerPhysics.CheckPlayerJumping())
        {
            script_playerPhysics.SetActionToJumping();
            v2_playerVelocity.y += f_jumpHeight;
        }
    }

    public void ApplyGravity()
    {
        v2_playerVelocity.y -= f_gravity * Time.deltaTime;
    }

    private void InitializeNullVariables()
    {
        if (f_playerSpeed <= 0.0f) { f_playerSpeed = 5.0f; }
        if (f_jumpHeight <= 0.0f) { f_jumpHeight = 5.0f; }
        if (f_lerpTime <= 0.0f) { f_lerpTime = 0.5f; }
    }
}
