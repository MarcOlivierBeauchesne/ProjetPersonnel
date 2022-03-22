using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script qui stock les donnees a sauvegarder d'un skill
/// </summary>
[System.Serializable] 
public class SkillData
{
    public int actualStack; // va contneir le nombre actuel de stack du skill
    public int skillCost; // va contenir le cout actuelle du skill

   /// <summary>
   /// Fonction publique qui stock les donnes pertinentes a la sauvegarde d'un skill
   /// </summary>
   /// <param name="infos">Reference a un SkillInfos</param>
    public SkillData(SkillInfos infos){
        actualStack = infos.actualStack; // actualStack prend la valeur de actualStack de infos
        skillCost = infos.skillCost; // skillCost prend la valeur du skillCost de infos
    }
}
