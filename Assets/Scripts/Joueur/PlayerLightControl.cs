using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightControl : MonoBehaviour
{
    [SerializeField] Personnage _perso;

    public void AjusterPersoLight(string intensity){
        _perso.AjusterLeafLight(intensity);
    }

}
