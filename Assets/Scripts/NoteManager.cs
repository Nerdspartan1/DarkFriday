using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
	public static NoteManager Instance;

	[Header("References")]
	public GameObject NoteReader;
	public Text NoteText;
	public Transform NoteSpawnLocations;
	public GameObject NotePrefab;

	private Transform[] _noteSpawnPoints;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		NoteReader.SetActive(false);
		//_noteSpawnPoints = new Transform[NoteSpawnLocations.childCount];
		//for(int i=0; i < _noteSpawnPoints.Length; ++i)
		//{
		//	_noteSpawnPoints[i] = NoteSpawnLocations.GetChild(i);
		//}
	}

	public void PlaceColorNotes()
	{
		List<int> _alreadyPlaced = new List<int>();
		
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
