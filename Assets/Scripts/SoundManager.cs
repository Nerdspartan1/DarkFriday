using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager sm;

    [Header ("Player")]
    [FMODUnity.EventRef]
    public string playerMov;

    [Header("AI")]
    [FMODUnity.EventRef]
    public string aiMov;
    [FMODUnity.EventRef]
    public string aiExpressions;

    [Header("Triggers")]
    [FMODUnity.EventRef]
    public string mannequin;

    [Header("Soundtrack")]
    [FMODUnity.EventRef]
    public string music;
    [FMODUnity.EventRef]
    public string intro;


    void Awake()
    {
        if (sm != null)
        {
            Destroy(this);
        }
        sm = this;
    }

}
