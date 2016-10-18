using UnityEngine;
using System.Collections;

public class EquippedWeapon : MonoBehaviour {
	public Weapon current;
	// Use this for initialization
	void Start () {
	}

	public void onEquip(Weapon curWeapon)
	{
		current.addToInventory();
		current = curWeapon;
		current.transform.parent = this.transform;
		current.equip();
		
	}
	// Update is called once per frame
	void Update () {
	}
}
