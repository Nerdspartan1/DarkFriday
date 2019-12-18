using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;

    [Header ("Player")]
    [FMODUnity.EventRef]
    public string playerMov;
    [FMODUnity.EventRef]
    public string playerFS;
    [FMODUnity.EventRef]
    public string playerDeath;
    [FMODUnity.EventRef]
    public string playerHide;

    [Header("AI")]
    [FMODUnity.EventRef]
    public string aiMov;
    [FMODUnity.EventRef]
    public string aiExpChase;
    [FMODUnity.EventRef]
    public string aiExpDetected;
    [FMODUnity.EventRef]
    public string aiExpPatrol;
    [FMODUnity.EventRef]
    public string aiExpSearch;

    [Header("Triggers")]
    [FMODUnity.EventRef]
    public string mannequin;

    [Header("UI")]
    [FMODUnity.EventRef]
    public string pickCloth;
    [FMODUnity.EventRef]
    public string pickNote;
    [FMODUnity.EventRef]
    public string flashlightButton;

    [Header("Soundtrack")]
    [FMODUnity.EventRef]
    public string music;
    [FMODUnity.EventRef]
    public string menu;


    void Awake()
    {
        if (sm != null)
        {
            Destroy(this);
        }
        sm = this;
    }

}
