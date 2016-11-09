using UnityEngine;
using System.Collections;

public class PlayerPhysicsScript : MonoBehaviour {

    [SerializeField]
    protected float f_gravity;

    // Use this for initialization
    void Start ()
    {
        InitializeNullVariables();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void InitializeNullVariables()
    {
        if (f_gravity <= 0.0f) { f_gravity = 10.0f; }
    }

    public float GetGravity
    {
        get
        {
            return f_gravity;
        }
        set
        {
            f_gravity = value;
        }
    }
}
