using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour
{
	public enum e_state
	{
		IDLE,
		RUN,
		ATTACK,
		DEAD
	}
	public Player			player;
	public List<Weapon>		weapons;
	public e_state			state;
	public Potion			loot;
	public AudioClip		screamGoblin;
	[HideInInspector] public Stats			stat;

	private Animator 		_ani;
	private	NavMeshAgent	_nav;
	private bool			_attacking;
	private AudioSource		_source;
	private	float			_timeAudio;

	void Start ()
	{
		_timeAudio 	= Time.time + Random.Range (5f, 20f);
		_ani 		= this.GetComponent<Animator> ();
		_nav 		= this.GetComponent<NavMeshAgent> ();
		_source 	= this.GetComponent<AudioSource> ();
		stat 		= this.GetComponent<Stats> ();
		state 		= e_state.IDLE;

		stat.level = player.stat.level;
		stat.STR += (int)((stat.level - 1) * 0.15 * stat.STR);
		stat.AGI += (int)((stat.level - 1) * 0.15 * stat.AGI);
		stat.CON += (int)((stat.level - 1) * 0.15 * stat.CON);
		stat.updateStat ();
		stat.XP += (int)((stat.level - 1) * 0.15 * stat.XP);
		stat.money += (int)((stat.level - 1) * 0.15 * stat.money);
	}

	void Update ()
	{
		if (this.stat.HP > 0 && Time.time >= _timeAudio) {
			_source.Play ();
			_timeAudio = Time.time + Random.Range(5f, 20f);
		}
		if (player == null || player.stat.HP <= 0)
		{
			state = e_state.IDLE;
			_ani.SetBool("run", false);
			_nav.Stop ();
			return;
		}
		if (stat.HP <= 0)
			death ();
		else if (state == e_state.RUN || state == e_state.ATTACK)
		{
			this.transform.LookAt(player.transform.position);
			if (Vector3.Distance(this.transform.position, player.transform.position) < 1.5F)
			{
				state = e_state.ATTACK;
				if (!_attacking)
					StartCoroutine(attackCoroutine());
			}
			else
			{
				state = e_state.RUN;
				_ani.SetBool("run", true);
				_nav.destination = player.transform.position;
			}
		}
	}


	/*
	 *	Attack stuff
	 */
	IEnumerator attackCoroutine()
	{
		_nav.Stop ();
		_ani.SetBool ("attack", true);
		_attacking = true;
		yield  return new WaitForSeconds(0.7F);
		if (player.stat.HP > 0 && Vector3.Distance(this.transform.position, player.transform.position) < 1.5F)
			stat.attackTaget(player.stat, 0, 0f);
		_ani.SetBool ("attack", false);
		_attacking = false;
		_nav.Resume();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
			state = e_state.RUN;
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "Player" && state == e_state.IDLE)
			state = e_state.RUN;
	}


	/*
	 * Death Stuff
	 */
	public void death()
	{

		if (state != e_state.DEAD)
		{
			int tmp = 0;
			if (Random.Range (0, 3) == 0)
				Instantiate (loot, this.transform.position + Vector3.up, this.transform.rotation);
			if ((tmp = Random.Range (0, weapons.Count + 4)) < weapons.Count) {
				if (weapons[tmp].rareness == Weapon.w_rate.COMMON
					|| (weapons[tmp].rareness == Weapon.w_rate.NON_COMMON && Random.Range(0, 10) > 4)
					|| (weapons[tmp].rareness == Weapon.w_rate.MAGIC && Random.Range(0, 10) > 6)
					|| (weapons[tmp].rareness == Weapon.w_rate.LEGENDARY && Random.Range(0, 10) > 8))
				Instantiate (weapons [tmp], this.transform.position + Vector3.up / 2f, this.transform.rotation);
			}
			_source.clip = screamGoblin;
			_source.Play ();
			_ani.SetTrigger ("death");
		}
		state = e_state.DEAD;
		StartCoroutine (deathCoroutine());
		_nav.Stop ();
	}

	IEnumerator deathCoroutine()
	{
		yield return new WaitForSeconds(3);
		while (_nav.baseOffset > -0.008F)
		{
			_nav.baseOffset -= 0.00005F;
			yield  return new WaitForSeconds(0.5F);
		}
		Destroy (this.gameObject);
	}
}
