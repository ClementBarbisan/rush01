using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Skill : MonoBehaviour
{
	public Player			player;
	public EffectSettings	effect;
	public ParticleSystem	particles;
	public int				curLevel;
	public int				cost;
	public int				step;

	void Start ()
	{
		curLevel = 0;
	}

	void Update ()
	{
	
	}
	void OnTriggerStay(Collider coll)
	{
		if (coll.tag != "Player")
			Debug.Log (coll.tag);
		//		if (burn && coll.tag == "Enemy" && currentTime + 10f > Time.time && currentTime - 10f < Time.time) {
		//			Debug.Log ("burn");
		//			coll.GetComponent<Enemy>().stat.HP -= damages; 
		//		}
	}
	public abstract void skillEffect();
}
