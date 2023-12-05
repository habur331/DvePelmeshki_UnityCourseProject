using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolume : MonoBehaviour
{
    public void OnVolumeChange(float value)
    {
        AudioListener.volume = value;
    }
}
