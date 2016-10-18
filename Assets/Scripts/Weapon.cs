using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Weapon : MonoBehaviour {
	public enum w_state
	{
		EQUIPPED,
		INVENTORY,
		GROUNDED,
		EQUIPINVENTORY
	};
	public enum w_rate
	{
		COMMON,
		NON_COMMON,
		MAGIC,
		LEGENDARY
	};
	[HideInInspector]
	public Inventory inventory;
	public w_rate rareness = w_rate.COMMON;
	public w_state state = w_state.GROUNDED;
	public float attackSpeed = 1f;
	public int damages = 1;
	public float weight = 1f;
	private int level;
	private Vector3 initialPosition;
	private Vector3 initialRotation;
	private Vector3 equippedScale;
	private Rigidbody rb;
	private MeshCollider coll;
	private Vector3 screenPoint;
	private Vector3 offset;
	private Player player;
	private EquippedWeapon equippedWeapon;
	private GameObject currentSlot = null;

	// Use this for initialization
	void Start () {
		foreach (GameObject current in GameObject.FindGameObjectsWithTag("Player")) {
			if (current.GetComponent<Player>())
			{
				player = current.GetComponent<Player>();
				break;
			}
		}
		inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<Inventory>();
		coll = GetComponent<MeshCollider> ();
		rb = GetComponent<Rigidbody> ();
		initialPosition = this.transform.position;
		initialRotation = this.transform.rotation.eulerAngles;
		equippedScale = this.transform.localScale;
		equippedWeapon = player.GetComponentInChildren<EquippedWeapon> ();
		attackSpeed *= Mathf.Round((UnityEngine.Random.Range(0.0f, 1.0f) + (float)player.GetComponent<Stats>().level * 0.3f) * 100f) / 100f;
		damages = Mathf.CeilToInt((float)damages * (float)(UnityEngine.Random.Range(0.0f, 1.0f) + (float)player.GetComponent<Stats>().level * 0.3f));
		if (state == w_state.INVENTORY)
			addToInventory ();
		else if (state == w_state.GROUNDED) {
			throwWeapon ();
			this.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		} 
		else if (state == w_state.EQUIPINVENTORY)
			addToEquipSlot ();
		else if (state == w_state.EQUIPPED)
			equip ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == w_state.INVENTORY || state == w_state.EQUIPINVENTORY) {
			if (this.inventory.canvas.alpha == 1)
			{
				this.GetComponent<MeshRenderer>().enabled = true;
				coll.enabled = true;
			}
			else
			{
				this.GetComponent<MeshRenderer>().enabled = false;
				coll.enabled = false;
			}
		}
	}
	
	void OnMouseDown(){
		if (state == w_state.INVENTORY) {
			initialPosition = this.transform.position;
			initialRotation = this.transform.rotation.eulerAngles;
			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}

	void OnMouseDrag(){
		if (state == w_state.INVENTORY)
		{
			Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
			transform.position = cursorPosition;
		}
	}

	void OnMouseUp()
	{
		if (state == w_state.INVENTORY) {
			RaycastHit[] hit;
			Vector3 point = this.GetComponentInParent<CanvasRenderer> ().transform.InverseTransformPoint (this.transform.position);
			if (point.x > this.GetComponentInParent<RectTransform> ().rect.max.x
				|| point.y < this.GetComponentInParent<RectTransform> ().rect.min.y
				|| point.y > this.GetComponentInParent<RectTransform> ().rect.max.y)
			{
				inventory.setEmpty(currentSlot);
				currentSlot = null;
				throwWeapon ();
			}
			else if ((hit = Physics.RaycastAll (Camera.main.ScreenPointToRay (Input.mousePosition))) != null)
			{
				foreach(RaycastHit currentHit in hit)
				{
					if (currentHit.collider.tag == "slot")
					{
						Weapon tmp = inventory.checkFree (currentHit.collider.gameObject);
						inventory.setFull (currentHit.collider.gameObject, this);
						this.transform.position = currentHit.collider.transform.position - new Vector3(-0.35f, 0.2f, -0.32f);
						if (tmp != null) {
							inventory.setFull (currentSlot, tmp);
							tmp.transform.position = initialPosition;
							tmp.transform.rotation = Quaternion.Euler (initialRotation);
						} else
							inventory.setEmpty (currentSlot);
						currentSlot = currentHit.collider.gameObject;
						break;
					}
					else if (currentHit.collider.tag == "equipWeapon")
					{
						equippedWeapon.onEquip(this);
						inventory.setEmpty(currentSlot);
						currentSlot = null;
					}
				}
			}
		}
	}

	public void addToEquipSlot()
	{
		rb.isKinematic = true;
		rb.useGravity = false;
		coll.enabled = true;
		this.transform.SetParent(inventory.transform);
		this.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
//		this.transform.localRotation = Quaternion.identity;
		this.transform.localScale = new Vector3(1f, 1f, 1f);
//		state = w_state.EQUIPINVENTORY;
	}

	public void addToInventory()
	{
		rb.isKinematic = true;
		rb.useGravity = false;
		coll.enabled = true;
		this.transform.SetParent(inventory.transform);
		this.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		this.transform.rotation = this.transform.parent.rotation;
		this.transform.localScale = new Vector3(1f, 1f, 1f);
		currentSlot = null;
		foreach (GameObject slot in inventory.slots) {
			if (inventory.checkFree(slot) == null)
			{
				this.transform.position = slot.transform.position - new Vector3(-0.35f, 0.2f, -0.32f);
				inventory.setFull(slot, this);
				currentSlot = slot;
				break;
			}
		}
		if (currentSlot == null) {
			throwWeapon();
			state = w_state.GROUNDED;
		}
		else
			state = w_state.INVENTORY;
	}

	public void throwWeapon()
	{
		this.transform.localScale = new Vector3 (2f, 2f, 2f);
		state = w_state.GROUNDED;
		rb.isKinematic = false;
		rb.useGravity = true;
		coll.enabled = true;
		this.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		this.transform.parent = null;
	}

	public void equip()
	{
		inventory.setEquipWeapon (this);
		this.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.Euler (0.0f, 0.0f, -90.0f);
		this.transform.localScale = equippedScale;
		rb.isKinematic = true;
		rb.useGravity = false;
		coll.enabled = false;
		state = w_state.EQUIPPED;
	}
}
