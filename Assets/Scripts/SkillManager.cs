using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class SkillManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public int		step;
	public Skill	skill;

	private bool	_buyable = false;
	private Image	_img;
	private Color	_saveColor;

	private Vector3	_origin;

	void Start ()
	{
		GetComponent<Image> ().sprite = skill.GetComponent<Image> ().sprite;
		_img = GetComponent<Image> ();
		_saveColor = _img.color;
	}
	
	void Update ()
	{
		if (skill.player.stat.talent > 0)
		{
			//checker step;
			if (skill.curLevel < 6 && (skill.step - 1) * 6 < skill.player.stat.level) {
				_img.color = Color.green;
				_buyable = true;
			} else
			{
				_img.color = Color.red;
				_buyable = false;
			}
		}
		else 
		{
			_buyable = false;
			// select to panel;
			if (skill.curLevel == 0)
				_img.color = Color.gray;
			else
				_img.color = _saveColor;
		}
	}

	public void getSkill()
	{
		if (_buyable)
		{
			skill.curLevel++;
			skill.player.stat.talent--;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_origin = transform.position;
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		if (skill.curLevel == 0)
			return ;
		transform.position = Input.mousePosition;
		
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		if (skill.curLevel == 0)
			return;
		transform.position = _origin;
	}


}
