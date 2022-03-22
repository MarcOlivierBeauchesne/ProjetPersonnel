using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjetCollection : MonoBehaviour
{
    [Header("Composante de collection")] // identification de la section Composante de collection
    [SerializeField] GameObject _boiteObjet;
    [Header("Information du mimo")] // identification de la section Information du mimo
    [SerializeField] InfosCollection _infosObjet; // reference au InfosCollection
    public InfosCollection infosObjet{ // acces public au InfosCollection
        get => _infosObjet; // par infosObjet, on retourne _infosObjet
    }
    [SerializeField] private Sprite _imageInconnu; // reference a l'image inconnu du mimo
    public Sprite imageInconnu{ // acces public a l'image inconnu du mimo
        get => _imageInconnu; // par imageInconnu, on retourne _imageInconnu
    }

    /// <summary>
    /// fonction qui change le nom du gameObject
    /// </summary>
    public void NommerObjet(){
        gameObject.name = _infosObjet.nomMimo; // on change le nom du gameOjbect dans la fenetre de collection pour le nomMimo du _infosObjet
    }

    /// <summary>
    /// fonction qui active la boite de description du mimo avec son image dans le bas de la collection
    /// </summary>
    public void ActiverBoite(){
        switch(_boiteObjet.activeInHierarchy){ //switch si la _boiteObjet est active
            case true : // si la _boiteObjet est active
                _boiteObjet.SetActive(false); // on desactive _boiteObjet
                break; // on sort de la condition
            case false: // si la _boiteObjet est inactive
                _boiteObjet.SetActive(true); // on active la _boiteObjet
                if(_infosObjet.isFound){ // si le mimo est trouve
                    _boiteObjet.transform.GetChild(0).GetComponent<Image>().sprite = _infosObjet.imageMimo; // on affiche l'image du mimo
                    _boiteObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = _infosObjet.loreText; // on affiche le loreText du mimo
                }
                else{ // si le mimo n'est pas trouve
                    _boiteObjet.transform.GetChild(0).GetComponent<Image>().sprite = _imageInconnu; // on affiche l'image inconnu
                    _boiteObjet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Trouvez ce mimo pour en apprend plus"; // on indique que le mimo n'est pas trouve
                }
                break; // on sort de la condition
        }
    }

}
