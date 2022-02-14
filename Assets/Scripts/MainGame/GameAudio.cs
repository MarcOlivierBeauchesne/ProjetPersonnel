using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script qui controle et sauvegarde les options de volume
/// </summary>
public class GameAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource; // acces prive pour le AudioSource
    [SerializeField] private Slider _sliderVolume; // acces prive pour le slider de volume
    [SerializeField] private AudioPrefs _audioPrefs; // acces prive pour le _audioPrefs
    [SerializeField] private Toggle _muteButton; // acces prive pour toggle qui mute le volume
    [SerializeField] private GameObject _optionsWindow; // acces prive pour la fenetre des options

    private bool _isMuted = false; // acces prive pour le bool si le volume est mute 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if(PlayerPrefs.HasKey("PlayerVolume")){ // si le jeu a enregistre un "PlayerVolume"
            _audioPrefs.volumeValue = PlayerPrefs.GetFloat("PlayerVolume"); // le volumeValue de _audioPrefs prend la valeur du PlayerPrefs
        }
        if(PlayerPrefs.HasKey("Muted")){ // si le jeu a enregistre un "Muted"
            if(PlayerPrefs.GetString("Muted") == "Muted"){ // si la valeur du PlayerPrefs "Muted" est Muted
                _audioPrefs.muted = true; // le muted de _audioPrefs est true
            }
            else if(PlayerPrefs.GetString("Muted") == "NoMuted"){ // sinon si la valeur du PlayerPrefs "NoMuted" est Muted
                _audioPrefs.muted = false; // le muted de _audioPrefs est false
            }
        }
        _optionsWindow.SetActive(false); // on desactive la fenetre des options
        _muteButton.isOn = _audioPrefs.muted; // la valeur du toggle prend la valeur de muted du _audioPrefs
        _audioSource.volume = _audioPrefs.volumeValue; // le volume du _audioSource prend la valeur du volumeValue du _audioPrefs
        _sliderVolume.value = _audioPrefs.volumeValue; // la valeur du slider prend la valeur du volumeValue du _audioPrefs
        if(_audioPrefs.muted){ // si le muted de _audioPrefs est true
            _audioSource.volume = 0; // le volume du _audioSource devient 0
            _sliderVolume.interactable = false; // le slider de volume n'est plus interractible
            _muteButton.isOn = true; // la valeur du toggle de mute est true
        }
    }

    /// <summary>
    /// Fonction qui met a jour le volume (avec le slider)
    /// </summary>
    public void ChangerSon(){
        float son = _sliderVolume.value; // float son prend la valeur du slider de volume
        _audioSource.volume = son; // le volume du _audioSource prend la valeur de son
        _audioPrefs.volumeValue = son; // le volumeValue du _audioPrefs prend la valeur de son
        SaveVolume(); // on appel SaveVolume
    }

    /// <summary>
    /// Fonction qui allume ou eteint le son
    /// </summary>
    public void EteindreSon(){
        _isMuted = !_isMuted; // on inverse _isMuted
        _muteButton.isOn = _isMuted; // le toggle du mute prend la valeur de _isMuted
        _audioPrefs.muted = _isMuted; // muted du _audioPrefs prend la valeur de _isMuted
        if(_isMuted){ // si _isMuted est true
            _sliderVolume.interactable = false; // le slider de volume n'est plus interractible
            _audioSource.volume = 0; // le volume du _audioSource devient 0
        }
        else{ // sinon (_isMuted est false)
            _sliderVolume.interactable = true; // le slider de volume est interractible
            ChangerSon(); // on appel ChangerSon
        }
        SaveVolume(); // on appel SaveVolume
    }
    
    /// <summary>
    /// Fonction qui ouvre et ferme la fenetre des options
    /// </summary>
    public void ActiverFenetreOptions(){
        if(!_optionsWindow.activeInHierarchy){ // si _optionsWindow n'est pas active dans la hierarchie
            _optionsWindow.SetActive(true); // on active _optionWindow
        }
        else{ // sinon (_optionWindow est active dans la hierarchie)
            _optionsWindow.SetActive(false); // on desactive _optionWindow
        }
    }

    /// <summary>
    /// Fonction qui sauvegarde les derniers ajustements de volume du joueur
    /// </summary>
    public void SaveVolume(){
        PlayerPrefs.DeleteKey("PlayerVolume"); // on supprime le PlayerPrefs "PlayerVolume"
        PlayerPrefs.DeleteKey("Muted"); // on supprime le PlayersPrefs "Muted"
        PlayerPrefs.SetFloat("PlayerVolume", _audioPrefs.volumeValue); // on sauvegarde le PlayerPrefs "PlayerVolume" avec la valeur de volumeValue du _audioPrefs
        PlayerPrefs.SetString("Muted", _audioPrefs.muted? ("Muted"):("NoMuted")); // on sauvegarde le PlayerPrefs "Muted" avec la valeur de muted du _audioPrefs
        // "Muted" si le muted de _audioPrefs est true 
        // "NoMuted" si le muted de _audioPrefs est false 
    }
}
