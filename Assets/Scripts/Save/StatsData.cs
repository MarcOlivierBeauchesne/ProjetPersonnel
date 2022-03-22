using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui stock les donnees a sauvegarder du BasicStats
/// </summary>
[System.Serializable] 
public class StatsData
{
    public float speed; // va contenir la vitesse de deplacement du joueur
    public float npGain; // va contenir le gain de point de nature
    public float npMaxPool; // va contenu le maximum de puissance naturelle
    public float seedDrop; // va contenir la quantite de noix qui tombe
    public float daytime; // va contenir le temps par jour
    public float defoAugment; // va contenir al valeur d'augmentation de deforestation par jour
    public float defoLevel; // va contenir le niveau actuel de deforestation
    public float defoPool; // va contenir le maximum de deforestation possible

    /// <summary>
    /// Fonction publique qui stock les donnes pertinentes a la sauvegarde du BasicStats
    /// </summary>
    /// <param name="stats">Reference au BasicStats</param>
    public StatsData(BasicStats stats){
        speed = stats.mouvementSpeed; // speed prend la valeur de mouvementSpeed de stats
        npGain = stats.npGain; // npGain prend la valeur de npGain de stats
        npMaxPool = stats.npMaxPool; // npMaxPool prend la valeur de npMaxPool de stats
        seedDrop = stats.seedDrop; // seedDrop prend la valeur de seedDrop de stats
        daytime = stats.dayTime; // daytime prend la valeur de dayTime de stats
        defoAugment = stats.deforestAugment; // defoAugment prend la valeur de deforestAugment de stats
        defoLevel = stats.deforestLevel; // defoLevel prend la valeur de deforestLevel de stats
        defoPool = stats.deforestPool; // defoPool prend la valeur de deforestPool de stats
    }
}
