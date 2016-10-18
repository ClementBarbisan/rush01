using UnityEngine;
using System.Collections;

public class SkillRock : Skill
{
	private int		_save;

	public override void skillEffect()
	{
		if (curLevel == 0 || player.stat.curMana < this.cost)
			return;
		Debug.Log ("Use Rock");
		player.stat.curMana -= this.cost;
		_save = player.stat.ARMOR;
		player.stat.ARMOR *= 2;
		StartCoroutine (rockCoroutine ());
	}
	
	IEnumerator rockCoroutine()
	{
		yield return new WaitForSeconds(10.0F * (this.curLevel / 2));
		player.stat.ARMOR = _save;
		Debug.Log ("Rock End");
	}
}
