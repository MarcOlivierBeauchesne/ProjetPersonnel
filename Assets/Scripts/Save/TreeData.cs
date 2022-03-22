using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Script qui stock les donnees a sauvegarder du SkillTree
/// </summary>
[System.Serializable]
public class TreeData
{
    public float absorbCount; // va contenir le nombre absorbCount du SkillTree

    /// <summary>
    /// Fonction publique qui stock les donnes pertinentes a la sauvegarde du SkillTree
    /// </summary>
    /// <param name="tree">Reference au SkillTree</param>
    public TreeData(Skilltree tree){
        absorbCount = tree.absorbCount; // absorbCount prend la valeur de absorbCount de tree
    }
}
