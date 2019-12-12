using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Pickable
{
	[TextArea(4,10)]
	public string Text;

	public override void PickUp()
	{
		NoteManager.Instance.ReadNote(this);
	}
}
