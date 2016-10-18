using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHandler : MonoBehaviour
{
	public Player			player;
	public Text				hpText;
	public RectTransform	hpImg;
	public Text				level;
	public RectTransform	xpImg;
	public Text				xpNext;
	public RectTransform	manaImg;

	public RectTransform	enPanel;
	public Text				enHPtext;
	public RectTransform	enHPimg;
	public Text				enName;
	public Text				enLevel;

	public RectTransform	mainPanel;
	public RectTransform	endPanel;

	public RectTransform	upButton;

	private Vector3			enPanelSave;
	private Enemy			_target;

	public CanvasRenderer 	toolTip;
	public Text 			wRare;
	public Text 			wSpeed;
	public Text 			wDamages;
	public Text				wWeight;

	void Start ()
	{
		_target = null;
		enPanelSave = enPanel.transform.localScale;
		enPanel.transform.localScale = Vector3.zero;
		endPanel.transform.localScale = Vector3.zero;
		upButton.transform.localScale = Vector3.zero;
		toolTip.gameObject.SetActive (false);
		foreach (Text panel in toolTip.GetComponentsInChildren<Text>()) {
			if (panel.name == "rareness")
				wRare = panel;
			else if (panel.name == "speed")
				wSpeed = panel;
			else if (panel.name == "damages")
				wDamages = panel;
			else if (panel.name == "weight")
				wWeight = panel;
		}
	}

	void Update ()
	{
		/*
		 *	Player UI
		 */
		hpText.text = player.stat.HP.ToString ();
		hpImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (player.stat.HP * 1.0F / (player.stat.CON * 5.0F)) * 300F);
		level.text = "Lv. " + player.stat.level.ToString ();
		xpNext.text = player.stat.XP.ToString() + " / " + player.nextLevel.ToString ();
		xpImg.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, (player.stat.XP * 1.0F / player.nextLevel * 1.0F) * 300F);
		manaImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (player.stat.curMana * 1.0F / (player.stat.maxMana * 1.0F)) * 300F);
		
//		Debug.Log ((player.stat.XP / player.nextLevel) * 300F);

		/*
		 *	Enemy UI
		 */
		getTarget ();
		if (_target != null && _target.stat.HP > 0)
		{

			enPanel.transform.localScale = enPanelSave;
			enHPtext.text = _target.stat.HP.ToString();
			enHPimg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_target.stat.HP * 1.0F / (_target.stat.CON * 5.0F)) * 300F);
			enName.text = _target.name.ToString();
			enLevel.text = "Lv. " + _target.stat.level.ToString();
		}
		else
			enPanel.transform.localScale = Vector3.zero;

		/*
		 * Game Over
		 */
		if (player.state == Player.e_state.DEAD)
		{
			mainPanel.transform.localScale = Vector3.zero;
			endPanel.transform.localScale = new Vector3(1, 1, 1);
			if (Input.GetKeyDown(KeyCode.Escape))
			    Application.Quit ();
			if (Input.GetKeyDown(KeyCode.Space))
				Application.LoadLevel(Application.loadedLevel);
	
		}

		/*
		 *	Up
		 */
		if (player.stat.up > 0)
			upButton.transform.localScale = new Vector3 (1F, 1F, 1F);
		else
			upButton.transform.localScale = Vector3.zero;
	}

	private void getTarget()
	{
		RaycastHit hit;
		if (player.target)
			_target = player.target;
		else if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
		{
			if (hit.collider.tag == "Enemy")
				_target = hit.collider.GetComponent<Enemy> ();
			else
				_target = null;
			if (hit.collider.tag == "Weapon") {
				toolTip.gameObject.SetActive (true);
				toolTip.transform.position = new Vector3 (Input.mousePosition.x + toolTip.GetComponent<RectTransform>().rect.width / 2.0f, Input.mousePosition.y - toolTip.GetComponent<RectTransform>().rect.height / 2.0f, this.transform.position.z);
				wRare.text = hit.collider.GetComponent<Weapon> ().rareness.ToString ();
				wSpeed.text = "Speed: " + hit.collider.GetComponent<Weapon> ().attackSpeed.ToString();
				wDamages.text = "Damages: " + hit.collider.GetComponent<Weapon> ().damages.ToString ();
				wWeight.text = "Weight: " + hit.collider.GetComponent<Weapon> ().weight.ToString ();
			}

			else
				toolTip.gameObject.SetActive (false);
		}
		else
			_target = null;
	}
}
