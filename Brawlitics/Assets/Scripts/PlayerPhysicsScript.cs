using UnityEngine;
using System.Collections;

public class PlayerPhysicsScript : MonoBehaviour {

    [SerializeField]
    private Vector2 v2_playerVelocity;

    [SerializeField]
    protected float f_gravity;

    private int i_layerMask;

    private Rect r_box;

    private int i_horizontalRays = 6;
    private int i_verticalRays = 4;
    private int i_margin = 2;

    private BoxCollider bC_collider;

    public enum e_PlayerAction
    {
        Standing,
        Jumping,
        Falling,
        Ducking
    }
    

    public float direction; // 0.0f = left, 1.0f = right

    protected e_PlayerAction pA_playerAction;

    // Use this for initialization
    void Awake()
    {
        bC_collider = this.GetComponent<BoxCollider>();
        pA_playerAction = e_PlayerAction.Falling;
        direction = 0.0f;
        InitializeNullVariables();
	}

    void Start()
    {
        v2_playerVelocity = Vector2.zero;
        i_layerMask = LayerMask.NameToLayer("normalCollision");
    }

    void FixedUpdate()
    {
        r_box = new Rect(
            bC_collider.bounds.min.x,
            bC_collider.bounds.min.y,
            bC_collider.bounds.size.x,
            bC_collider.bounds.size.y);
    }
	
	// Update is called once per frame
	void Update()
    {
        ApplyGravity();
    }

    private void InitializeNullVariables()
    {
        if (f_gravity <= 0.0f) { f_gravity = 10.0f; }
    }

    public bool CheckPlayerInAir()
    {
        return (pA_playerAction == e_PlayerAction.Jumping || pA_playerAction == e_PlayerAction.Falling);
    }

    public void SetActionToJumping()
    {
        pA_playerAction = e_PlayerAction.Jumping;
    }

    private void ApplyGravity()
    {

        if (CheckPlayerInAir())
        {
            v2_playerVelocity.y -= f_gravity * Time.deltaTime;
        }

        if (v2_playerVelocity.y < 0) { pA_playerAction = e_PlayerAction.Falling; }

        if (pA_playerAction == e_PlayerAction.Falling || pA_playerAction == e_PlayerAction.Standing)
        {
            //Vector2 startPoint = new Vector2(r_box.xMin, r_box.center.y);
            //Vector2 endPoint = new Vector2(r_box.xMax, r_box.center.y);
            Vector2 startPoint = new Vector2((transform.position.x - r_box.width / 2), r_box.center.y);
            Vector2 endPoint = new Vector2((transform.position.x + r_box.width / 2), r_box.center.y);

            RaycastHit hitInfo;

            float distance = r_box.height / 2 + (pA_playerAction == e_PlayerAction.Standing ? i_margin : Mathf.Abs(v2_playerVelocity.y * Time.deltaTime));

            bool connected = false;

            for(int i = 0; i < i_verticalRays; i++)
            {
                float lerpAmount = (float)i / (float)(i_verticalRays - 1);
                Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);
                Ray ray = new Ray(origin, Vector2.down);
                Debug.DrawRay(origin, Vector2.down);
                connected = Physics.Raycast(ray, out hitInfo, distance, i_layerMask);

                if(connected)
                {
                    pA_playerAction = e_PlayerAction.Standing;
                    transform.Translate(Vector2.down * (hitInfo.distance - r_box.height/2.0f + 0.001f));
                    v2_playerVelocity.y = 0;
                    break;
                }
            }

            if(!connected)
            {
                pA_playerAction = e_PlayerAction.Falling;
            }
        }
    }

    //Accessors
    public float AddPlayerVelocityX { set { v2_playerVelocity.x += value; } }
    public float AddPlayerVelocityY { set { v2_playerVelocity.y += value; } }
    public Vector2 GetPlayerVelocity { get { return v2_playerVelocity; } }

    public float GetGravity { get { return f_gravity; } }

    public float SetGravity { set { f_gravity = value; } }

    public e_PlayerAction GetPlayerAction { get { return pA_playerAction; } }

    public e_PlayerAction SetPlayerAction { set { pA_playerAction = value; } }
   
}
