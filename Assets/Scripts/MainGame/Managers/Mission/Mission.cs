using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject qui represente les informations d'une mission
/// </summary>
[CreateAssetMenu(fileName = "Mission", menuName = "Mission")] // ajout de l'option dans le menu contextuel pour creer une Mission
public class Mission : ScriptableObject
{
    [SerializeField] TypeMission _typeMission; // reference pour le Type de mission
    public TypeMission typeMission{ // acces public pour le style de mission
        get=>_typeMission; // par typeMission, on retourne _typeMission
    }
    [SerializeField] string _nomMission = "Ramassage"; // nom de la mission
    public string nomMission{ // acces public pour le nom de la mission
        get=>_nomMission; // par nomMission, on retourne _nomMission
    }
    [SerializeField] int _missionAmount; // quantite necessaire pour la mission
    public int missionAmount{ // acces public pour la quantite necessaire pour la mission
        get=>_missionAmount; // par missionAmount, on retourne _missionAmount
        set{ // on change la valeur de _missionAmount
            _missionAmount = value; // _missionAmount prend la valeur recu
        }
    }
    int _missionTotal = 0; // montant accompli pour la mission
    public int missionTotal{ // acces public pour le montant accompli pour la mission
        get=>_missionTotal; // par missionTotal, on retourne _missionTotal
        set{ // on change la valeur de _missionTotal
            _missionTotal = value; // _missionTotal prend la valeur recu
        }
    }
    [TextArea] // zone de texte
    [SerializeField] string _descriptionMission; // description de la mission
    public string descriptionMission{ // acces public pour la description de la mission
        get=>_descriptionMission; // par descriptionMission, on retourne _descriptionMission
    }
    [SerializeField] int _rewardValue = 0; // recompense de la mission en point de nature
    public int rewardValue{ // acces public pour la recompense de la mission en point de nature
        get=>_rewardValue; // parrewardValue, on retourne _rewardValue
    }
}
