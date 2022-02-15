using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject pour afficher l'explication d'un skill lors du passage de la souris
/// </summary>
[CreateAssetMenu(menuName ="Skill Explication", fileName = "SkillExplication")]
public class SkillExplication : ScriptableObject
{
    [SerializeField] private string _nomSkill; // champs prive pour le nom du skill
    public string nomSkill{ // acces public pour le nom du skill
        get => _nomSkill; // par nomSkill, on retourne la valeur _nomSkill
    } 
    [TextArea] // zone de texte
    [SerializeField] private string _explication; // champs prive pour l'explication du skill
    public string explication{ // acces public pour l'explication du skill
         get => _explication; // par explication, on retourne la valeur _explication
    }
    private string _actualStack; // acces prive au nombre de stack actuel du skill
    public string actualStack{ // acces public au nombre de stack actuel du skill
         get => _actualStack; // par actualSkill, on retourne la valuer _actualSkill
    }
}
