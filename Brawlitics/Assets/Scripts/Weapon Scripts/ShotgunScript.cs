using UnityEngine;
using System.Collections;

public class ShotgunScript : WeaponScript
{
    [SerializeField]
    private Rigidbody gO_shell;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
	}

    public override int Ammo { get; set; }

    public override void Attack()
    {
        Vector3 direction = (IsFacingRight ? transform.right : -transform.right);


        for (int i = 0; i < 15; i++)
        {
            Vector3 angle = new Vector3(0, 0, Random.Range(-15, 15));
            Rigidbody shellPellet = Instantiate(gO_shell, transform.position + direction/1.5f, transform.rotation) as Rigidbody;

            shellPellet.transform.Rotate(angle);
            direction = (IsFacingRight ? shellPellet.transform.right : -shellPellet.transform.right);
            shellPellet.velocity = direction * 25.0f;

            shellPellet.transform.localScale = Vector3.Lerp(shellPellet.transform.localScale, Vector3.zero, 0.5f);

            Destroy(shellPellet.gameObject, 0.3f);
        }
    }


}
