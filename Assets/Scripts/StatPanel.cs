using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatPanel : MonoBehaviour {

	public Player			player;
	public RectTransform	panel;
	public Text				pname;
	public Text				str;
	public Text				agi;
	public Text				con;
	public Text				armor;
	public Text				up;
	public Text				dam;
	public Text				maxhp;
	public Text				xp;
	public Text				xptonext;
	public Text				tune;

	public RectTransform	strButton;
	public RectTransform	agiButton;
	public RectTransform	conButton;
	private AudioSource		source;
	public Text				upgrade;

	[HideInInspector] public bool panelUp = false;

	void Start () 
	{	
		source = GetComponent<AudioSource> ();
		panel.transform.localScale = Vector3.zero;
	}

	void Update ()
	{
		if (Input.GetKeyDown ("c"))
		{
			if (panelUp)
			{
				source.Play ();
				panelUp = false;
			}
			else
			{
				source.Play ();
				panelUp = true;
			}
		}

		if (panelUp)
			panel.transform.localScale = new Vector3 (1F, 1F, 1F);
		else
			panel.transform.localScale = Vector3.zero;
		upgrade.text = "Upgrade points : " + player.stat.talent.ToString();
		pname.text = player.name + " (lvl " + player.stat.level + ")";
		str.text = "Strengh " + player.stat.STR.ToString ();
		agi.text = "Agility " + player.stat.AGI.ToString();
		con.text = "Constitution " + player.stat.CON.ToString();
		armor.text = "Armor " + player.stat.ARMOR.ToString();
		up.text = "Upgrade points " + player.stat.up.ToString ();
		dam.text = "Damages " + player.stat.minDamage.ToString () + "-" + player.stat.maxDamage.ToString ();
		maxhp.text = "Max HP " + player.stat.maxHP.ToString ();
		xp.text = "XP " + player.stat.XP.ToString ();
		xptonext.text = "XP to next " + player.nextLevel.ToString();
		tune.text = "Tune " + player.stat.money.ToString();

		if (player.stat.up > 0)
		{
			strButton.localScale = new Vector3(1F, 1F, 1f);
			agiButton.localScale = new Vector3(1F, 1F, 1f);
			conButton.localScale = new Vector3(1F, 1F, 1f);
		}
		else
		{
			strButton.localScale = Vector3.zero;
			agiButton.localScale = Vector3.zero;
			conButton.localScale = Vector3.zero;
		}

	}

	public void upSTR()
	{
		if (player.stat.up > 0)
		{	
			player.stat.up--;
			player.stat.STR++;
		}
		player.stat.updateStat ();
	}

	public void upAGI()
	{
		if (player.stat.up > 0)
		{
			player.stat.AGI++;
			player.stat.up--;
		}
		player.stat.updateStat ();
	}

	public void upCON()
	{
		if (player.stat.up > 0)
		{
			player.stat.up--;
			player.stat.CON++;
		}
		player.stat.updateStat ();
	}

	public void upButton()
	{
		source.Play ();
		panelUp = true;
	}

	public void closeButton()
	{
		source.Play ();
		panelUp = false;
	}
}
