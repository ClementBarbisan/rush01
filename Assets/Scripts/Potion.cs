using UnityEngine;
using System.Collections;

public class Potion : MonoBehaviour {

	void Update ()
	{
		this.transform.Rotate(0, Time.deltaTime * 100F, 0, Space.World);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Player")
		{
			Player player;

			player = collider.GetComponent<Player>();
			player.stat.HP += (int)(player.stat.maxHP * 0.3F);
			if (player.stat.HP > player.stat.maxHP)
				player.stat.HP = player.stat.maxHP;
			Destroy(gameObject);
		}
	}
}
