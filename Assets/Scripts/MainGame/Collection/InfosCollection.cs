using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject des informations d'un mimo
/// </summary>
[CreateAssetMenu(fileName = "infosCollection", menuName = "infosCollection")] // ajout de l'option dans le menu contextuel pour creer un InfosCollection
public class InfosCollection : ScriptableObject
{
    [SerializeField] private string _nomMimo; // nom du mimo
    public string nomMimo{ // acces public au nom du mimo
        get => _nomMimo; // par nomMimo, on retourne_nomMimo
    }

    [SerializeField] private Sprite _imageMimo; // image du mimo 
    public Sprite imageMimo{ // acces public a l'image du mimo
        get => _imageMimo; // par imageObjet, on retourne _imageObjet
    }
    [TextArea] // zone de texte
    [SerializeField] private string _loreText; // texte pour une descrition du mimo
    public string loreText{ // acces public  pour le texte de descrition du mimo
        get => _loreText; // par loreText, on retourne _loreText
    }

    [SerializeField] private bool _isFound = false; // bool si le mimo est trouve ou non
    public bool isFound{ // acces public si le mimo est trouve ou non
        get => _isFound; // par isFound, on retourne _isFound
        set{ // on change la valeur de _isFound
            _isFound = value; // _isFound prend la valeur recu
        }
    }

    int _mimoValue = 0; // valeur du mimo
    public int mimoValue{ // acces public pour la valeur du mimo
        get=> _mimoValue; // par mimoValue, on retourne _mimoValue
        set{ // on change la valeur de _mimoValue
            _mimoValue = value; // _mimoValue prend la valeur recu
        }
    }
}
