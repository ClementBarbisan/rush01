using UnityEngine;
using System.Collections;

public class SkillFireBall : Skill {

	private bool	_targeting = false;

	void Update()
	{
		if (_targeting)
		{
			if (Input.GetMouseButtonDown(1))
			{
				_targeting = true;
				player.targetingSpell = false;
			}
			else if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
			{
				RaycastHit hit;
				
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit) && hit.collider.tag == "Enemy")
				{
					StartCoroutine(throwCoroutine(hit.collider.GetComponent<Enemy>()));
				}
			}
		}

	}

	public override void skillEffect()
	{
		if (curLevel == 0 || player.stat.curMana < this.cost && !player.targetingSpell)
			return;
		player.targetingSpell = true;
		_targeting = true;
	}

	IEnumerator throwCoroutine(Enemy target)
	{
		EffectSettings instanceEffect;

		player.stat.curMana -= this.cost;

		_targeting = false;
		instanceEffect = Instantiate (this.effect, player.transform.position + 2 * player.transform.up, player.transform.rotation) as EffectSettings;
		instanceEffect.Target = target.gameObject;
		instanceEffect.CollisionEnter += enemy_collision;

		instanceEffect.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.2F);
		player.targetingSpell = false;
	}

	void enemy_collision(object sender, CollisionInfo e)
	{
		Debug.Log ("touch  something : " + e.Hit.collider.tag);
		if (e.Hit.collider.tag == "Enemy")
		{
			Debug.Log ("touch ennemy");
			e.Hit.collider.GetComponent<Enemy>().stat.HP -= 4 * this.curLevel;
		}
	}
		
}
