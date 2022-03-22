using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fonction qui controle la vitesse de l'animation du jour et de la nuit
/// </summary>
public class DayLightManager : MonoBehaviour
{
    [SerializeField] BasicStats _basicStats; // reference au BasicStats
    [SerializeField] Animator _animLumiere; // reference a l'animator de la lumiere

    // Start is called before the first frame update
    void Start()
    {
        _animLumiere.speed = 1 / _basicStats.dayTime; // on change la vitesse de _animLumiere par 1 divise par le dayTime de _basicStats
    }

    /// <summary>
    /// Fonction publique qui ajuste la vitesse de _animLumiere
    /// </summary>
    public void AjusterVitesseJour(){
        _animLumiere.speed = 1 / _basicStats.dayTime; // on change la vitesse de _animLumiere par 1 divise par le dayTime de _basicStats
        _animLumiere.SetTrigger("DayTime"); // on declanche le trigger DayTime de _animLumiere
    }
}
