using UnityEngine;
using System.Collections;

public class PlayerPhysicsScript : MonoBehaviour {

    private PlayerActionScript script_playerAction;

    [SerializeField]
    private Vector2 v2_playerVelocity;
    [SerializeField]
    private float f_playerMaxVelocityX;
    [SerializeField]
    private float f_playerMaxVelocityY;

    [SerializeField]
    protected float f_gravity;

    private bool b_isFacingRight; 

    public LayerMask lM_mask;
    public LayerMask lM_mask2;

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

    [SerializeField]
    protected e_PlayerAction pA_playerAction;

    // Use this for initialization
    void Awake()
    {
        script_playerAction = GetComponent<PlayerActionScript>();
        bC_collider = this.GetComponent<BoxCollider>();
        pA_playerAction = e_PlayerAction.Falling;
        b_isFacingRight = true;
        InitializeNullVariables();
	}

    void Start()
    {
        v2_playerVelocity = Vector2.zero;
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
        if(v2_playerVelocity.x > 0.0f) { b_isFacingRight = true; }
        else if(v2_playerVelocity.x < 0.0f) { b_isFacingRight = false; }
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

        bool verticalConnected = CheckCollision(i_verticalRays, startPoint, endPoint, verticalDirection, distance, false, lM_mask);

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

        bool weaponConnected = CheckCollision(i_verticalRays, startPoint, endPoint, verticalDirection, distance, false, lM_mask2);

        if (weaponConnected)
        {
            script_playerAction.SetWeaponNearby = true;
            script_playerAction.SetWeaponToEquip = rH_hitInfo.collider.gameObject;
        }
    }

    private void CheckHorizontalCollision()
    {
        Vector2 horizontalDirection = Vector2.right * Mathf.Sign(v2_playerVelocity.x);
        
        Vector2 startPoint = new Vector2(transform.position.x, transform.position.y - r_box.height / 2);
        Vector2 endPoint = new Vector2(transform.position.x, transform.position.y + r_box.height / 2);

        float distance = r_box.width / 2 + Mathf.Abs(v2_playerVelocity.x * Time.deltaTime);

        bool horizontalConnected = CheckCollision(i_horizontalRays, startPoint, endPoint, horizontalDirection, distance, false, lM_mask);

        if (horizontalConnected)
        {
            transform.Translate(Mathf.Sign(v2_playerVelocity.x) * Vector2.right * (rH_hitInfo.distance - r_box.width / 2.0f + 0.001f));
            v2_playerVelocity.x = 0;
        }
    }

    private bool CheckCollision(float rays, Vector2 startPoint, Vector2 endPoint, Vector2 direction, float distance, bool connected, LayerMask mask)
    {
        RaycastHit hitInfo;

        for (int i = 0; i < rays; i++)
        {
            float lerpAmount = (float)i / (float)(rays - 1);
            Vector2 origin = Vector2.Lerp(startPoint, endPoint, lerpAmount);
            Ray ray = new Ray(origin, direction);
            Debug.DrawRay(origin, direction);
            connected = Physics.Raycast(ray, out hitInfo, distance, mask);

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
