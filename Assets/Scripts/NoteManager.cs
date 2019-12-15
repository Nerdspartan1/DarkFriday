using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NoteManager : MonoBehaviour
{
	public static NoteManager Instance;

	[Header("References")]
	public GameObject NoteReader;
	public Text NoteText;
	public Transform Notes;
	public Note SeasonNote;

	private Note _noteBeingRead = null;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		NoteReader.SetActive(false);
		PlaceColorNotes();
		SeasonNote.SetSeasonText(ColorManager.Instance.CurrentSeason);
	}

	public void PlaceColorNotes()
	{
		List<Note> possibleNotes = Notes.GetComponentsInChildren<Note>().ToList();
		possibleNotes.RandomizeList();
		int noteId=0;
		foreach(var note in possibleNotes)
		{
			if(noteId < 4)
			{
				note.gameObject.SetActive(true);
				note.SetClothingType((ClothingType)noteId++);
			}
			else
			{
				note.gameObject.SetActive(false);
			}

		}
	}

	public void ReadNote(Note note)
	{
		NoteReader.SetActive(true);
		NoteText.text = note.Text;
		_noteBeingRead = note;
	}

	public void StopReadingNote()
	{
		NoteReader.SetActive(false);
		if (_noteBeingRead)
		{
			_noteBeingRead.StopReading();
			_noteBeingRead = null;
		}

	}

}
