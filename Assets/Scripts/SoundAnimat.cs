using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimat : MonoBehaviour
{
    public static SoundAnimat sap;
    // Start is called before the first frame update

    void PlayAiMov()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(SoundManager.sm.aiMov, this.gameObject);
    }
}
