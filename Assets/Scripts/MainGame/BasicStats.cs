using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui Stock les variables qui affecteront les differents
/// differents elements du jeu
/// </summary>
public class BasicStats : MonoBehaviour
{
    [SerializeField] PlayerRessources _playerRessources; // Reference pour le ScriptableObject PlayerRessources

    [Header("Player Updrades")] // identification de la section des ameliorations du joueur
    [SerializeField] private float _mouvementSpeedRef = 10; // acces prive pour la reference de la vitesse de deplacement du joueur
    [SerializeField] private float _mouvementSpeed = 10; // acces prive pour la vitesse de deplacement du joueur
    public float mouvementSpeed{ // acces public pour la vitesse de deplacement du joueur
        get => _mouvementSpeed; // par mouvementSpeed, on retourne la valeur _mouvementSpeed
        set{
            _mouvementSpeed = value; // par mouvementSpeed, on change la valeur de _mouvementSpeed
        }
    }

    [SerializeField] private float _npGainRef = 100; // acces prive a la reference du gain de point de nature 
    [SerializeField] private float _npGain = 100; // acces prive au gain de point de nature 
    public float npGain{ // acces public au gain de point de nature 
        get => _npGain; // par npGain, on retourne la valeur _npGain 
        set{
            _npGain = value; // par npGain, on change la valeur de _npGain
        }
    }

    [SerializeField] private float _npMaxPoolRef = 100; // acces prive a la reference du maximum de puissance de nature
    [SerializeField] private float _npMaxPool = 100; // acces prive au maximum de puissance de nature
    public float npMaxPool{ // acces public au maximum de puissance de nature
        get => _npMaxPool; // par npMaxPool, on retourne la valeur _npMaxPool 
        set{
            _npMaxPool = value; // par npMaxPool, on change la valeur de _npMaxPool
        }
    }

    [Header("Game Upgrades")] // Identification de la section des ameliorations des elements du jeu
    [SerializeField] private float _seedDropRef = 1; // acces prive pour la reference de la quantite de graines qui tombent
    [SerializeField] private float _seedDrop = 1; // acces prive pour la quantite de graines qui tombent
    public float seedDrop{ // acces public pour la quantite de graines qui tombent
        get => _seedDrop; // par seedDrop, on retourne la valeur _seedDrop 
        set{
            _seedDrop = value;  // par seedDrop, on change la valeur _seedDrop 
        }
    }

    [SerializeField] private float _dayTimeRef = 5; // acces prive a la reference de temps disponible par jour
    [SerializeField] private float _dayTime = 5; // acces prive au temps disponible par jour 
    public float dayTime{ // acces public au temps disponible par jour 
        get => _dayTime; // par dayTime, on retourne la valeur _dayTime
        set{
            _dayTime = value; // par dayTime, on change la valeur _dayTime
        }
    }

    [Header("Deforesation")] // Identification de la section des caracteristiques de la deforestation
    [SerializeField] private float _deforestAugmentRef = 10; // acces prive a la reference de l'augmentation de la deforestation par jour
    [SerializeField] private float _deforestAugment = 10; // acces prive a l'augmentation de la deforestation par jour 
    public float deforestAugment{ // acces public a l'augmentation de la deforestation par jour 
        get => _deforestAugment; // par deforestAugment, on retourne la valeur _deforestAugment
        set{
            _deforestAugment = value; // par deforestAugment, on change la valeur _deforestAugment
        }
    }

    [SerializeField] private float _deforestLevelRef = 20; // acces prive a la rerefence du niveau de deforestation globale
    [SerializeField] private float _deforestLevel = 20; // acces prive au niveau de deforestation globale
    public float deforestLevel{ // acces public au niveau de deforestation globale
        get => _deforestLevel; // par deforestLevel, on retourne la valeur _deforestLevel
        set{
            _deforestLevel = value; // par deforestLevel, on change la valeur _deforestLevel
        }
    }

    [SerializeField] private float _deforestPoolRef = 100; // acces prive a la reference du maximum de deforestation globale
    [SerializeField] private float _deforestPool = 100; // acces prive au maximum de deforestation globale 
    public float deforestPool{ // acces public au maximum de deforestation globale 
        get => _deforestPool; // par deforestPool, on retourne la valeur _deforestPool
        set{
            _deforestPool = value; // par deforestPool, on change la valeur _deforestPool
        }
    }

    /// <summary>
    /// Fonction qui change une statistique du jeu selon les parametres recu
    /// </summary>
    /// <param name="typeModif">Valeur qui identifie le type de variable a changer</param>
    /// <param name="value">valeur a ajouter a la statistique</param>
    /// <param name="absorb">bool qui indique si l'on doit absorber les toutes les valeurs et recommencer l'arbre des talent</param>
    public void ChangerStats(TypeStats typeModif, float value, bool absorb){
        // TypeStats type = typeModif;
        switch(typeModif){ // condition switch selon le TypeStats recu
            case TypeStats.Mouvement : // si le TypeStats est Mouvement 
                if (absorb) { mouvementSpeed = _mouvementSpeedRef+value; } // si l'on doit absorber, mouvementSpeed devient la valeur de reference + la valeur recu
                else{ mouvementSpeed += value;} // sinon, mouvementSpeed augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.NatureGain : // si le TypeStats est NatureGain 
                if (absorb) { npGain = _npGainRef+value; } // si l'on doit absorber, npGain devient la valeur de reference + la valeur recu
                else{ npGain += value;} // sinon, npGain augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.NatueMaxPool : // si le TypeStats est NatueMaxPool 
                if (absorb) { npMaxPool = _npMaxPoolRef+value; } // si l'on doit absorber, npMaxPool devient la valeur de reference + la valeur recu
                else{ npMaxPool += value;} // sinon, npMaxPool augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.SeeDrop : // si le TypeStats est SeeDrop 
                if (absorb) { seedDrop = _seedDropRef+value; } // si l'on doit absorber, seedDrop devient la valeur de reference + la valeur recu
                else{ seedDrop += value;} // sinon, seedDrop augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.DayTime : // si le TypeStats est DayTime 
                if (absorb) { dayTime = _dayTimeRef+value; } // si l'on doit absorber, dayTime devient la valeur de reference + la valeur recu
                else{ dayTime += value;} // sinon, dayTime augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.DefoAugment : // si le TypeStats est DefoAugment 
                if (absorb) { deforestAugment = _deforestAugmentRef+value; } // si l'on doit absorber, deforestAugment devient la valeur de reference + la valeur recu
                else{ deforestAugment += value;} // sinon, deforestAugment augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.DefoLevel : // si le TypeStats est DefoLevel 
                if (absorb) { deforestLevel = _deforestLevelRef+value; } // si l'on doit absorber, deforestLevel devient la valeur de reference + la valeur recu
                else{ deforestLevel += value;} // sinon, deforestLevel augmente selon la valeur recu
                break; // on sort de la condition
            case TypeStats.DefoPool : // si le TypeStats est DefoPool 
                if (absorb) { deforestPool = _deforestPoolRef+value; } // si l'on doit absorber, deforestPool devient la valeur de reference + la valeur recu
                else{ deforestPool += value;} // sinon, deforestPool augmente selon la valeur recu
                break; // on sort de la condition
        }
    }
}

public enum TypeStats{ // enum pour le type de stats du jeu
    Mouvement,
    NatureGain,
    NatueMaxPool,
    SeeDrop,
    DayTime,
    DefoAugment,
    DefoLevel,
    DefoPool
}
