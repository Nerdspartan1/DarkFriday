using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
	public static NoteManager Instance;

	public GameObject NoteReader;
	public Text NoteText;

	public GameObject[] NoteSpawnLocations;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		NoteReader.SetActive(false);
	}

	public void ReadNote(Note note)
	{
		NoteReader.SetActive(true);
		NoteText.text = note.Text;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E)) StopReadingNote();
	}

	public void StopReadingNote()
	{
		NoteReader.SetActive(false);
	}

}
