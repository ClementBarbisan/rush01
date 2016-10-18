using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
	[HideInInspector]public List<GameObject> slots = new List<GameObject>();
	private Weapon[] slotsFree = new Weapon[12];
	[HideInInspector]
	public bool show = false;
	[HideInInspector]
	public CanvasGroup canvas;
	public GameObject slotEquip;
	public Weapon currentEquip;
	private AudioSource source;
	// Use this for initialization
	void Start () {
		int i = 0;
		source = this.GetComponent<AudioSource> ();
		slotEquip = GameObject.FindGameObjectWithTag ("equipWeapon");
		canvas = this.GetComponentInParent<CanvasGroup> ();
		canvas.alpha = 0;
		canvas.interactable = false;
		canvas.blocksRaycasts = false;
		foreach (GameObject slot in GameObject.FindGameObjectsWithTag("slot")) {
			slotsFree[i] = null;
			slots.Add (slot);
			slot.GetComponent<Collider>().enabled = false;
			i++;
		}
	}

	public void setFull(GameObject current, Weapon curWeapon)
	{
		int index = slots.IndexOf (current);
		slotsFree [index] = curWeapon;
	}

	public void setEmpty(GameObject current)
	{
		int index = slots.IndexOf (current);
		slotsFree [index] = null;
	}

	public Weapon checkFree(GameObject current)
	{
		int index = slots.IndexOf (current);
		return (slotsFree[index]);
	}

	public void setEquipWeapon(Weapon curWeapon)
	{
		if (currentEquip != null)
			Destroy (currentEquip.gameObject);
		curWeapon.state = Weapon.w_state.EQUIPINVENTORY;
		currentEquip = (Weapon)Instantiate (curWeapon, slotEquip.transform.position - slotEquip.transform.right / 2.0f, slotEquip.transform.rotation);
		currentEquip.transform.Rotate (new Vector3(0.0f, 0.0f, -90.0f), Space.Self);
		curWeapon.state = Weapon.w_state.EQUIPPED;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("i")) {
			if (show)
			{
				canvas.alpha = 0;
				canvas.interactable = false;
				canvas.blocksRaycasts = false;
				foreach(GameObject slot in slots)
				{
					slot.GetComponent<Collider>().enabled = false;
				}
				source.Play();
				show = false;
			}
			else
			{
				canvas.alpha = 1;
				canvas.interactable = true;
				canvas.blocksRaycasts = true;
				foreach(GameObject slot in slots)
				{
					slot.GetComponent<Collider>().enabled = true;
				}
				source.Play();
				show = true;
			}
		}
	}
}
