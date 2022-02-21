using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurSalle : MonoBehaviour
{
    [SerializeField] private GameObject _firstSalle; // gameObject de la premiere salle
    [SerializeField] private GameObject[] _tSalleForet; // tableau des salle de foret
    [SerializeField] private GameObject[] _tSalleCoupe; // tableau des salles de deforestation
    [SerializeField] private BasicStats _basicStats; // refenrence au BasicStats
    [SerializeField] private GameObject _perso;
    private float _pourcentage; // pourcentage de la carte couvert par al deforestation
    [SerializeField] private int _nbSalle = 10; // nombre de salle a generer
    private int _qteSalleForet = 10; // quantite de salle de foret a generer
    private int _qteSalleCoupe = 10; // quantite de salle de deforestation a generer
    private bool _salleOuverte = false; // bool pour savoir si les salles sont ouvertes

    private List<GameObject> _listSalle = new List<GameObject> { }; // liste des salles generees
    private List<Vector2> _listPosDispo = new List<Vector2> { }; // liste des positions disponibles pour generer des salles

    private /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _pourcentage = _basicStats.deforestLevel; // le pourcentage prend la valeur de deforestLevel du BasicStats
        GenererFirstSalle(); // on appel GenererFirstSalle
    }

    /// <summary>
    /// Fonction qui genere la premiere salle et etablit la quantite de chaque salle a generer
    /// </summary>
    public void GenererFirstSalle(){
        _salleOuverte = false;
        _pourcentage = _basicStats.deforestLevel; // le pourcentage prend la valeur de deforestLevel du BasicStats
        ClearRooms(); // on appel ClearRooms
        _qteSalleCoupe = Mathf.RoundToInt((_nbSalle * _pourcentage)/100); // le nombre de salle de deforestaation prend la valeur en pourcentage selon le total de salle a generer
        _qteSalleForet = _nbSalle - _qteSalleCoupe; // la quantite de salle de forest est la balance du total de salle moins le nombre de salles de deforestation
        GameObject salle = Instantiate(_firstSalle, Vector3.zero, Quaternion.identity); // on genere la premiere salle
        salle.transform.SetParent(transform); // le generateur de salle devient le parent de la premiere salle
        salle.GetComponent<Salle>().genSalle = this; // on attribue le genSalle de la salle pour le script actuel
        _listSalle.Add(salle); // on ajoute la premiere salle dans la liste des salles
        _perso.transform.position = salle.transform.position;
    }

    /// <summary>
    /// Fonction qui detruit toutes les salles de la liste des salles generees
    /// </summary>
    private void ClearRooms(){
        foreach(GameObject salle in _listSalle){ // pour chaque salle dans la liste de _listeSalle
            Destroy(salle); // on detruit la salle
        }
        _listSalle.Clear(); // par securite, on vide la liste
    }

    /// <summary>
    /// Fonction qui genere des salles selons les positions disponibles recu
    /// </summary>
    /// <param name="_listPos">Liste des positions disponibles pour accueillir une salle</param>
    public void GenererSalles(List<Vector2> _listPos){
        _listPosDispo = _listPos; // on copie _listPos dans_listPosDispo
        GameObject salle = null; // on stock une salle vide
        if(_qteSalleCoupe + _qteSalleForet >= 1){ // si la quantite total de salle a generer est plus eleve ou egal a 1
            int nbSalleSpawn = Mathf.Clamp(Random.Range(1,4), 1, _qteSalleCoupe+_qteSalleForet); // on genere un nombre aleatoire de salle entre 1 et 3
            nbSalleSpawn = Mathf.Clamp(nbSalleSpawn, 0, _listPosDispo.Count); // on restreint le nombre de salle a generer entre 0 et le nombre de position disponibles
            for (int i = nbSalleSpawn; nbSalleSpawn > 0; nbSalleSpawn--){// boucle selon le nombre de salle a generer
                int posNewList = Random.Range(0, _listPosDispo.Count); // on trouve une position au hasard dans la lsite des positions disponibles
                int quelleSalle = Random.Range(0, 2); // on genere un nombre entre 0 et 1 pour le type de salle a generer
                int salleForetRand = Random.Range(0, _tSalleForet.Length); // on genere un nombre entre 0 et la longueur du tableau de salle de foret pour choisir une salle a generer
                int salleCoupeRand = Random.Range(0, _tSalleCoupe.Length); // on genere un nombre entre 0 et la longueur du tableau de salle de coupe pour choisir une salle a generer
                if (quelleSalle == 0) // si quelleSalle donne 0 (salle foret)
                {
                    if (_qteSalleForet >= 1) // si la quantite de salle de foret est superieur ou egal a 1
                    {
                        salle = Instantiate(_tSalleForet[salleForetRand], _listPos[posNewList], Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                        _qteSalleForet--; // on reduit la quantite de salle de foret a generer de 1
                    }
                    else // si la quantite de salle de foret est inferieur a 1
                    {
                        salle = Instantiate(_tSalleCoupe[salleCoupeRand], _listPos[posNewList], Quaternion.identity); // on genere une salle de coupe a la position posNewList de la _listPos
                        _qteSalleCoupe--; // on reduit la quantite de salle de coupe a generer de 1
                    }
                }
                else if (quelleSalle == 1) // si quelleSalle donne 1 (salle coupe)
                {
                    if (_qteSalleCoupe >= 1) // si la quantite de salle de coupe est superieur ou egal a 1
                    {
                        salle = Instantiate(_tSalleCoupe[salleCoupeRand], _listPos[posNewList], Quaternion.identity); // on genere une salle de coupe a la position posNewList de la _listPos
                        _qteSalleCoupe--; // on reduit la quantite de salle de coupe a generer de 1
                    }
                    else
                    {
                        salle = Instantiate(_tSalleForet[salleForetRand], _listPos[posNewList], Quaternion.identity); // on genere une salle foret a la position posNewList de la _listPos
                        _qteSalleForet--; // on reduit la quantite de salle de foret a generer de 1
                    }
                }
                _listPosDispo.RemoveAt(posNewList); // on enleve la position choisi de la liste des positions disponibles
                salle.transform.SetParent(transform); // on dit a la salle que son parent devient le generateur de salles
                salle.GetComponent<Salle>().genSalle = this; // on attribue le genSalle de la salle pour le script actuel
                _listSalle.Add(salle); // on ajoute la salle dans la liste des salle generees
            }
        }
        else{ // si la quantite total de salle a generer est inferieur a 1
            OuvrirSalle(); // on appel OuvrirSalle
        }
    }

    /// <summary>
    /// Fonction qui demande au salle d'ouvrir leurs porte
    /// </summary>
    public void OuvrirSalle(){
        if(_listSalle.Count == 1 ){ // si la quantite de salle dans la liste de salle est de 1 (premiere salle seulement)
            ClearRooms(); // on appel ClearRooms
            GenererFirstSalle(); // on appel GenererFirstSalle
            return; // on quitte la fonction
        }
        if(!_salleOuverte){ // si les salles ne sont pas ouverte
            _salleOuverte = true; // les salles devienent ouvertes
            foreach (GameObject salle in _listSalle) // pour chaque salle dans _listSalle
            {
                salle.GetComponent<Salle>().CreerDetecteurs(); // on demande a Salle de Creer des detecteur pour ouvrir les portes
            }
        }
    }
}
