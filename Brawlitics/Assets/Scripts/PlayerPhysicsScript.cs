using UnityEngine;
using System.Collections;

public class PlayerPhysicsScript : MonoBehaviour {

    [SerializeField]
    protected float f_gravity;

    public enum e_PlayerAction
    {
        Standing,
        Jumping,
        Ducking
    }

    public float direction; // 0.0f = left, 1.0f = right

    protected e_PlayerAction pA_playerAction;

    // Use this for initialization
    void Awake ()
    {
        pA_playerAction = e_PlayerAction.Standing;
        direction = 0.0f;
        InitializeNullVariables();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (pA_playerAction == e_PlayerAction.Jumping)
        {
            this.GetComponent<PlayerMovementScript>().ApplyGravity();
        }
    }

    private void InitializeNullVariables()
    {
        if (f_gravity <= 0.0f) { f_gravity = 10.0f; }
    }

    public bool CheckPlayerJumping()
    {
        return pA_playerAction == e_PlayerAction.Jumping;
    }

    public void SetActionToJumping()
    {
        pA_playerAction = e_PlayerAction.Jumping;
    }

    //Accessors
    public float GetGravity
    {
        get
        {
            return f_gravity;
        }
    }

    public float SetGravity
    {
        set
        {
            f_gravity = value;
        }
    }

    public e_PlayerAction GetPlayerAction
    {
        get
        {
            return pA_playerAction;
        }
    }

    public e_PlayerAction SetPlayerAction
    {
        set
        {
            pA_playerAction = value;
        }
    }
   
}
