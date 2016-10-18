using UnityEngine;
using System.Collections;

public class SkillHeal : Skill
{
	
	public override void skillEffect()
	{
		if (curLevel == 0 || player.stat.curMana < this.cost)
			return;
		Debug.Log ("Use Healing");
		player.stat.curMana -= this.cost;
		StartCoroutine (healCoroutine ());
		player.stat.HP += 10 * curLevel;
		if (player.stat.HP > player.stat.maxHP)
			player.stat.HP = player.stat.maxHP;
	}

	IEnumerator healCoroutine()
	{
		EffectSettings instanceEffect;
		instanceEffect = Instantiate (this.effect, player.transform.position, player.transform.rotation) as EffectSettings;
		instanceEffect.transform.parent = player.transform;
		yield return new WaitForSeconds (3);
		instanceEffect.IsVisible = false;
	}
}
