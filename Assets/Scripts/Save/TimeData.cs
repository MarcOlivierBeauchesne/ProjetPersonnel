using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script qui stock les donnees a sauvegarder du Timer
/// </summary>
[System.Serializable]
public class TimeData 
{
    public float minute; // va contenir les minutes de la journee actuelle
    public int seconde; // va contenir les secondes actuelles de la journee
    public int nbJour; // va contenir le nombre de jour actuelle de la partie

    /// <summary>
    /// Fonction publique qui stock les donnes pertinentes a la sauvegarde du Timer
    /// </summary>
    /// <param name="timer">Reference au Timer</param>
    public TimeData(Timer timer){
        minute = timer.minute; // minute prend la valeur de minute de timer
        seconde = timer.seconde; // seconde prend la valeur de seconde de timer
        nbJour = timer.nbJour; // nbJour prend la valeur de nbJour de timer
    }
}
