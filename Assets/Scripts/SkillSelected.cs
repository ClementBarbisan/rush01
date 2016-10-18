using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;  
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillSelected : MonoBehaviour
{
	public Skill[]		skills;
	public Canvas		skillCanvas;
	public CanvasRenderer toolTip;
	public Text			level;
	public Text			cost;
	public Text			step;

	private bool		_canvasVisible = false;
	private AudioSource source;

	void Start ()
	{	
		source = this.GetComponent<AudioSource> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown ("1") && skills[0] != null)
			skills [0].skillEffect ();
		else if (Input.GetKeyDown ("2") && skills[1] != null)
			skills [1].skillEffect ();
		else if (Input.GetKeyDown ("3") && skills[2] != null)
			skills [2].skillEffect ();
		if (!_canvasVisible)
			skillCanvas.enabled = false;
		else
			skillCanvas.enabled = true;
		if (Input.GetKeyDown ("n")) {
			source.Play ();
			_canvasVisible = !_canvasVisible;
		}
		PointerEventData ped = new PointerEventData(EventSystem.current);  
		ped.position = Input.mousePosition;
		List<RaycastResult> hits = new List<RaycastResult>();
		EventSystem.current.RaycastAll(ped,hits);
		toolTip.gameObject.SetActive(false);		
		foreach (RaycastResult r in hits) {  
			if (r.gameObject.tag == "Skill") {				
				toolTip.gameObject.SetActive (true);
				toolTip.transform.position = new Vector3 (Input.mousePosition.x + toolTip.GetComponent<RectTransform>().rect.width, Input.mousePosition.y - toolTip.GetComponent<RectTransform>().rect.height, this.transform.position.z);
				level.text = "Level: " + r.gameObject.GetComponent<SkillManager> ().skill.curLevel;
				cost.text = "Cost: " + r.gameObject.GetComponent<SkillManager> ().skill.cost;
				step.text = "Step: " + r.gameObject.GetComponent<SkillManager> ().skill.step;
				break;
			}
			toolTip.gameObject.SetActive(false);
		}
	}

	public void disableCanvas()
	{
		_canvasVisible = false;
	}
}
