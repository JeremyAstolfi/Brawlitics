using UnityEngine;
using System.Collections;

public class PlayerPhysicsScript : MonoBehaviour {

    [SerializeField]
    private Vector2 v2_playerVelocity;
    [SerializeField]
    private float f_playerMaxVelocityX;
    [SerializeField]
    private float f_playerMaxVelocityY;

    [SerializeField]
    protected float f_gravity;

    private int i_layerMask;

    private Rect r_box;

    private int i_horizontalRays = 6;
    private int i_verticalRays = 4;
    private int i_margin = 2;

    private BoxCollider bC_collider;

    private RaycastHit rH_hitInfo;

    public enum e_PlayerAction
    {
        Standing,
        Jumping,
        Falling,
        Ducking
    }
    

    public float direction; // 0.0f = left, 1.0f = right

    [SerializeField]
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
        //if (CheckPlayerInAir())
        //{
            v2_playerVelocity.y -= f_gravity * Time.deltaTime;
        //}

        if (v2_playerVelocity.y < 0) { pA_playerAction = e_PlayerAction.Falling; }

        CheckVerticalCollision();
        CheckHorizontalCollision();
    }

    private void CheckVerticalCollision()
    {
        Vector2 verticalDirection = Vector2.up;
        if (pA_playerAction != e_PlayerAction.Jumping) { verticalDirection = Vector2.down; }

        Vector2 startPoint = new Vector2(transform.position.x - r_box.width / 2, transform.position.y);// + r_box.center.y);
        Vector2 endPoint = new Vector2(transform.position.x + r_box.width / 2, transform.position.y);// + r_box.center.y);

        float distance = r_box.height / 2 + (pA_playerAction == e_PlayerAction.Standing ? i_margin : Mathf.Abs(v2_playerVelocity.y * Time.deltaTime));

        bool verticalConnected = CheckCollision(i_verticalRays, startPoint, endPoint, verticalDirection, distance, false);

        if (verticalConnected)
        {
            pA_playerAction = e_PlayerAction.Standing;
            transform.Translate(Vector2.down * (rH_hitInfo.distance - r_box.height / 2.0f + 0.001f));
            v2_playerVelocity.y = 0;
        }

        if (!verticalConnected && pA_playerAction == e_PlayerAction.Standing)
        {
            pA_playerAction = e_PlayerAction.Falling;
        }
    }

    private void CheckHorizontalCollision()
    {
        Vector2 horizontalDirection = Vector2.right * Mathf.Sign(v2_playerVelocity.x);
        
        Vector2 startPoint = new Vector2(transform.position.x, transform.position.y - r_box.height / 2);
        Vector2 endPoint = new Vector2(transform.position.x, transform.position.y + r_box.height / 2);

        float distance = r_box.width / 2 + Mathf.Abs(v2_playerVelocity.x * Time.deltaTime);

        bool horizontalConnected = CheckCollision(i_horizontalRays, startPoint, endPoint, horizontalDirection, distance, false);

        if (horizontalConnected)
        {
            transform.Translate(Mathf.Sign(v2_playerVelocity.x) * Vector2.right * (rH_hitInfo.distance - r_box.width / 2.0f + 0.001f));
            v2_playerVelocity.x = 0;
        }
    }

    private bool CheckCollision(float rays, Vector2 startPoint, Vector2 endPoint, Vector2 direction, float distance, bool connected)
    {
        RaycastHit hitInfo;

        for (int i = 0; i < rays; i++)
        {
            float lerpAmount = (float)i / (float)(rays - 1);
            Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);
            Ray ray = new Ray(origin, direction);
            Debug.DrawRay(origin, direction);
            connected = Physics.Raycast(ray, out hitInfo, distance, i_layerMask);

            if (connected)
            {
                rH_hitInfo = hitInfo;
                return connected;
            }
        }
        return connected;
    }

    //Accessors
    public float SetPlayerVelocityX { set { v2_playerVelocity.x = value; } }
    public float SetPlayerVelocityY { set { v2_playerVelocity.y = value; } }
    public float AddPlayerVelocityY { set { v2_playerVelocity.y += value; } }
    public Vector2 GetPlayerVelocity { get { return v2_playerVelocity; } }

    public float GetGravity { get { return f_gravity; } }

    public float SetGravity { set { f_gravity = value; } }

    public e_PlayerAction GetPlayerAction { get { return pA_playerAction; } }

    public e_PlayerAction SetPlayerAction { set { pA_playerAction = value; } }
    
    //Null Variable Initializer
    private void InitializeNullVariables()
    {
        if (f_gravity <= 0.0f) { f_gravity = 10.0f; }
        if (f_playerMaxVelocityX <= 0.0f) { f_playerMaxVelocityX = 1.5f; }
        if (f_playerMaxVelocityY <= 0.0f) { f_playerMaxVelocityY = 5.0f; }
    }
}
