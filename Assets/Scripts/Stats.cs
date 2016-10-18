using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	
	public int		STR;
	public int		AGI;
	public int		CON;
	public int		INT;
	public int		ARMOR;
	[HideInInspector] public int	maxMana;
	[HideInInspector] public int	up;
	[HideInInspector] public int	talent;
	[HideInInspector] public int	maxHP;
	[HideInInspector] public int	HP;
	[HideInInspector] public int	minDamage;
	[HideInInspector] public int	maxDamage;
	[HideInInspector] public int	curMana;
	public int		level;
	public int		XP;
	public int		money;
	
	void Start ()
	{
		updateStat ();
		HP = maxHP;
		up = 0;
		talent = 0;
		maxMana = 100;
		curMana = maxMana;
		StartCoroutine (manaCoroutine ());
	}
	
	public void		updateStat()
	{
		maxHP = 5 * CON;
		minDamage = STR / 2;
		maxDamage = minDamage + 4;
		maxMana = INT * 10;
	}
	
	public void		attackTaget(Stats target, int weaponDamage, float weaponWeight)
	{
		if (calcTouch (target.AGI, weaponWeight))
			target.HP -= calcDam (target.ARMOR, weaponDamage);	
	}
	
	private bool	calcTouch(int targetAGI, float weight)
	{
		int hit = 75 + AGI - targetAGI - Mathf.FloorToInt(weight * ((float)level / 10f));
		int rand = Random.Range (0, 101);
		if (hit > rand)
			return (true);
		else
			return (false);
	}
	
	private int		calcDam(int targetARMOR, int weaponDamage)
	{
		int baseDamage = Random.Range (minDamage, maxDamage + 1 + weaponDamage);
		return (baseDamage * (1 - (targetARMOR / 200)));
	}
	
	/*
	 *	erwan le gros bebe
	 */
	IEnumerator manaCoroutine()
	{
		while (true)
		{
			if (curMana < maxMana)
				curMana += level / 2 + 1;
			yield return new WaitForSeconds (1.0F); 
		}
	}
	
}