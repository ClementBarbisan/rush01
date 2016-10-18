using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SkillDropHandler : MonoBehaviour, IDropHandler
{
	public int	n;

	void Start ()
	{
	
	}

	void Update ()
	{
	
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		if (eventData == null || eventData.pointerDrag == null || eventData.pointerDrag.GetComponent<SkillManager> ().skill.curLevel == 0)
			return;
		this.GetComponent<Image>().sprite = eventData.pointerDrag.GetComponent<Image>().sprite;
		eventData.pointerDrag.GetComponent<SkillManager> ().skill.player.skillSelected.skills [n] = eventData.pointerDrag.GetComponent<SkillManager> ().skill;
	}


}
