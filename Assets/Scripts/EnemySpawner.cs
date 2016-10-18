using UnityEngine;
using System.Collections;	
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
	public List<Enemy>	list;
	public Player		player;

	private Enemy		_current;

	void Start ()
	{
		StartCoroutine(randSpawn());
	}

	void Update ()
	{

	}

	IEnumerator	randSpawn()
	{
		int rand = 3;

		while (true)
		{
			if (_current != null)
				rand = 3;
			else 
				rand = Random.Range (0, 10);
			if (rand == 0 || rand == 1)
			{
				_current = Instantiate(list[rand], this.transform.position, this.transform.rotation) as Enemy;
				_current.player = player;
			}
			yield return new WaitForSeconds(1);
		}
	}
}
