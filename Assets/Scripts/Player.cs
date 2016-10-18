using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public enum e_state
	{
		IDLE,
		RUN,
		ATTACK,
		DEAD
	};
	public Camera			cam;
	public e_state			state;
	public SkillSelected	skillSelected;
	public ParticleSystem	partUp;
	public RectTransform	upText;
	public List<Weapon>		weapons;
	[HideInInspector] public EquippedWeapon  	curWeapon;
	[HideInInspector] public Enemy			target;
	[HideInInspector] public Weapon			weapon;
	[HideInInspector] public Stats			stat;
	[HideInInspector] public bool			targetingSpell = false;

	private Animator 		_ani;
	private	NavMeshAgent	_nav;
	private	Vector3			_camVec;
	private	bool			_attacking;
	private AudioSource		_source;
	private bool			skillPassive = false;
	[HideInInspector] public int	nextLevel;

	void Start ()
	{
		curWeapon  = this.GetComponentInChildren<EquippedWeapon> (); 
		_ani 		= this.GetComponent<Animator> ();
		_nav 		= this.GetComponent<NavMeshAgent> ();
		stat 		= this.GetComponent<Stats> ();
		_camVec 	= this.transform.position - cam.transform.position;
		_source 	= this.GetComponent<AudioSource> ();
		target 	    = null;
		weapon      = null;
		_attacking 	= false;
		nextLevel 	= 100;
		state 		= e_state.IDLE;

		upText.transform.localScale = Vector3.zero;
	}
	

	void Update ()
	{
		/*
		 * 	Camera Position Upadte
		 */
		cam.transform.position = this.transform.position - _camVec;
		this.skillPassive = false;
		foreach (Skill skill in skillSelected.skills) 
		{
			if (skill && skill.gameObject.name == "passiveSkill")
			{
				this.skillPassive = true;
				break;
			}
		}
		/*
		 * Death
		 */
		if (stat.HP <= 0)
			death ();
		if (state == e_state.DEAD)
			return;

		if (Input.GetKeyDown ("w"))
			GameObject.Instantiate(weapons[Random.Range(0, weapons.Count - 1)], this.transform.position, this.transform.rotation);

		/*
		 * 	Mouse left button
		 */
		if (Input.GetMouseButton (0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && !targetingSpell)
		{
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit)) // 100, LayerMask.NameToLayer("Terrain")
			{
				if (hit.collider.tag == "Weapon")
					weapon = hit.collider.GetComponent<Weapon>();
				else if (hit.collider.tag == "Enemy" && hit.collider.GetComponent<Enemy>().stat.HP > 0)
					target = hit.collider.GetComponent<Enemy>();
				else
					target = null;
				if (weapon != null && Vector3.Distance(this.transform.position, weapon.transform.position) < 2.5f)
				{
					if (weapon != curWeapon.current)
						curWeapon.onEquip(weapon);
				}
				if (target != null && Vector3.Distance(this.transform.position, target.transform.position) < 1.5F	)
					state = e_state.ATTACK;
				else
				{
					state = e_state.RUN;
					_ani.SetBool ("run", true);
					_ani.SetBool ("attack", false);
					_nav.destination = hit.point;
				}
			}
		}
//		if (Input.GetMouseButtonUp (0))
//		{
//			state = e_state.IDLE;
//			//target = null;
//			_ani.SetBool ("attack", false);
//		}

		/*
		 *	Attack
		 */
		if (state == e_state.ATTACK && target != null)
		{
			this.transform.LookAt(target.transform.position);
			if (!_attacking) {
				_source.Play();
				_nav.Stop ();
				_ani.speed = curWeapon.current.attackSpeed;
				_ani.SetBool ("attack", true);
				_ani.SetBool ("run", false);
				_attacking = true;
			}
		}

		/*
		 *	Reach destination
		 */
		if (transform.position == _nav.destination)
		{
			state = e_state.IDLE;
			_ani.SetBool ("run", false);
		}

		if (Input.GetKeyDown ("u"))
			levelUP ();
	}

	public void hit()
	{
		if (target)
		{
			if (this.skillPassive)
				stat.attackTaget(target.stat, curWeapon.current.damages, curWeapon.current.weight - stat.level);
			else
				stat.attackTaget(target.stat, curWeapon.current.damages, curWeapon.current.weight);
		}
		if (target && target.stat.HP <= 0)
		{
			stat.money += target.stat.money;
			stat.XP += target.stat.XP;
			if (stat.XP >= nextLevel)
				levelUP();
			target = null;
		}
	}

	public void resetAttack()
	{
		_nav.Resume ();
		_ani.speed = 1f;
		_ani.SetBool ("attack", false);
//		_ani.SetBool ("run", false);
		_attacking = false;
	}


	/*
	 *	Level UP
	 */
	private void levelUP()
	{
		partUp.Play ();
		StartCoroutine (upTextCoroutine ());
		stat.level++;
		stat.XP = 0;
		stat.up += 5;
		stat.talent += 1;
		nextLevel = nextLevel * 2 + nextLevel / 2;
		stat.HP = stat.maxHP;
	}

	IEnumerator upTextCoroutine()
	{
		upText.localScale = new Vector3 (1F, 1F, 1F);
		yield return new WaitForSeconds (3F);
		upText.transform.localScale = Vector3.zero;
	}


	/*
	 *	Death stuff
	 */

	public void death()
	{
		if (state != e_state.DEAD)
			_ani.SetTrigger("death");
		state = e_state.DEAD;
		_nav.Stop ();
	}
	
	/*
	 *	Attack stuff
	 */
}
