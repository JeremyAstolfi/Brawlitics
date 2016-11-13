using UnityEngine;
using System.Collections;

public abstract class WeaponScript : MonoBehaviour {

    public Vector2 SpawnPoint { get; set; }

    public bool IsFacingRight { get; set; }

    public abstract int Ammo { get; set; }

    public abstract void Attack();
}
