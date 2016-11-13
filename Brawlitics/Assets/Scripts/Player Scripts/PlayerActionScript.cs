using UnityEngine;
using System.Collections;

public class PlayerActionScript : MonoBehaviour {

    [SerializeField]
    private bool b_weaponEquipped;
    private bool b_weaponNearby;

    private GameObject gO_weaponToEquip;
    private GameObject gO_equippedWeapon;

	// Use this for initialization
	void Start ()
    {
        b_weaponEquipped = false;
        b_weaponNearby = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(b_weaponNearby && Input.GetKeyDown(KeyCode.E))
        {
            b_weaponEquipped = true;
            gO_equippedWeapon = gO_weaponToEquip;
        }
	    if(b_weaponEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
            if(Input.GetKeyDown(KeyCode.G))
            {
                b_weaponEquipped = false;
                gO_equippedWeapon = null;
            }
        }
	}

    public bool SetWeaponNearby { set { b_weaponNearby = value; } }
    public GameObject SetWeaponToEquip { set { gO_weaponToEquip = value; } }
}
