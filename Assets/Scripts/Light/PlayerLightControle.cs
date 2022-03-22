using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui permet a l'animator de la lumiere ambiante de reduire al lumiere de la feuille du joueur
/// </summary>
public class PlayerLightControle : MonoBehaviour
{
    [SerializeField] Personnage _perso; // reference au Personnage

    /// <summary>
    /// Fonction publique qui permet d'enclancher un changement d'intensite dans la lumiere de al feuille du joueur
    /// </summary>
    /// <param name="intensity">direction de  l'intensite de la lumiere</param>
    public void AjusterPlayerLight(string intensity){
        _perso.AjusterLeafLight(intensity); // le Personnage ajuste sa lumiere selon la direction de l'intensite (up ou down)
    }
}
