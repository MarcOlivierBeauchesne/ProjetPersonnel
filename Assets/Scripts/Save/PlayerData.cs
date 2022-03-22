using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui stock les donnees a sauvegarder du joueur
/// </summary>
[System.Serializable]
public class PlayerData
{
    public int seed; // va contenir le nombre de noix du joueur
    public int naturePoints; // va contenir le nombre de point de nature du joueur
    public int naturePower; // va contenir le nombre de point de puissance naturelle du joueur

    /// <summary>
    /// Fonction publique qui stock les donnes pertinentes a la sauvegarde du joueur
    /// </summary>
    /// <param name="player">Reference au Personnage</param>
    public PlayerData(Personnage player){
        seed = player.ressourcesPlayer.seedAmount; // seed prend la valeur du seedAmount de ressourcesPlayer
        naturePoints = player.ressourcesPlayer.naturePoint; // naturePoints prend la valeur de naturePoint de ressourcesPlayer
        naturePower = player.ressourcesPlayer.naturePower; // naturePower prend la valeur de naturePower de ressourcesPlayer
    }
}
