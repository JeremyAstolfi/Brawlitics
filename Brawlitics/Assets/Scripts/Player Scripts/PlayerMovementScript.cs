using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {

    [SerializeField]
    private float f_playerSpeed;

    [SerializeField]
    private float f_jumpHeight;

    [SerializeField]
    private float f_lerpTime;


    private PlayerPhysicsScript script_playerPhysics;

	// Use this for initialization
	void Start ()
    {

        script_playerPhysics = GetComponent<PlayerPhysicsScript>();

        InitializeNullVariables();
	}
	
	// Update is called once per frame
	void Update ()
    {
        UserInput();

        transform.position = 
            Vector2.Lerp(transform.position, 
                new Vector2(transform.position.x + script_playerPhysics.GetPlayerVelocity.x,
                transform.position.y + script_playerPhysics.GetPlayerVelocity.y),
            f_lerpTime);
	}

    private void UserInput()
    {
        script_playerPhysics.SetPlayerVelocityX = Input.GetAxis("Horizontal") * f_playerSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && !script_playerPhysics.CheckPlayerInAir())
        {
            script_playerPhysics.SetActionToJumping();
            script_playerPhysics.AddPlayerVelocityY = f_jumpHeight;
        }
    }
    

    private void InitializeNullVariables()
    {
        if (f_playerSpeed <= 0.0f) { f_playerSpeed = 5.0f; }
        if (f_jumpHeight <= 0.0f) { f_jumpHeight = 5.0f; }
        if (f_lerpTime <= 0.0f) { f_lerpTime = 0.5f; }
    }

}
