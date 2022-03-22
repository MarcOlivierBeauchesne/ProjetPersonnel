using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fonction qui permet a l'animator de la fenetre de chargement de desactiver son gameObject
/// </summary>
public class Loading : MonoBehaviour
{
    /// <summary>
    /// fonction qui desactive la fenetre de chargement
    /// </summary>
    public void Desactiver(){
        gameObject.SetActive(false); // on desactive la fenetre de chargement
    }
}
