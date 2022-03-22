using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject pour les preferences audio du joueur
/// </summary>
[CreateAssetMenu(menuName = "pref Audio", fileName = "audioPrefs")]
public class AudioPrefs : ScriptableObject
{
    [SerializeField]private float _volumeValue = 1; // acces prive a la valeur du volume du joueur
    public float volumeValue{ // acces public a la valeur du volume du joueur
        get => _volumeValue; // par volumeValue on retourne la valeur _volumeValue
        set{ // on change la valeur de _volumeValue
            _volumeValue = value; // par volumeValue on change la valeur _volumeValue
        }
    }

    [SerializeField] private bool _muted = false; // acces prive a la valeur du "mute" du volume du joueur
    public bool muted{ // acces public a la valeur du "mute" du volume du joueur
        get => _muted; // par muted on retourne la valeur _muted
        set{ // on change la valeur de _muted
            _muted = value; // par muted on change la valeur _muted
        }
    }
}
