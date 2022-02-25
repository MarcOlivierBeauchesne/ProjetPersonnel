using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Salle : MonoBehaviour
{
    [SerializeField] private LayerMask _layerSol; // layer du sol d'une tache
    [SerializeField] private LayerMask _layerTache; 
    [SerializeField] GameObject[] _tDetecteurs; // tableaux des detecteurs de la salle
    [SerializeField] Transform[] _tPosTaches; // tableau des positions possible des taches
    [SerializeField] Transform[] _tPosMimo; // tableau des positions possible des Mimos
    [SerializeField] Transform[] _tPosSeed;
    [SerializeField] GameObject _goMimo; // gameObject d'un mimo
    [SerializeField] GameObject _goSeed; 
    [SerializeField] GameObject[] _tGoTache; // tableau des taches possibles
    [SerializeField] private int _chanceSeed = 30;
    private List<Vector2> _listFreePos = new List<Vector2> { }; // liste des position disponible pour instancier une salle
    private bool _toucheAuxBlocs; // bool qui determine si un detecteur touche a _layerTuile
    
    private List<Vector2> _listPositions = new List<Vector2> // list de positions a verifier pour une salle
    {   new Vector2(0,22),
        new Vector2(0,-22),
        new Vector2(-26,0),
        new Vector2(26,0),
    };

    private GenerateurSalle _genSalle; // acces prive pour le GenerateurSalle
    public GenerateurSalle genSalle{ // acces public pour le GenerateurSalle
        get => _genSalle; // par genSalle, on retourne_genSalle
        set{
            _genSalle = value; // par genSalle, on change la valeur de _genSalle
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Scan(); // on appel Scan
        if(_tPosTaches.Length > 0){ // si la liste des positions de tache n'est pas vide
            GenererTaches(); // on appel GenererTache
        }
        SpawnMimo(); // on appel SpawnMimo
        if(_tPosSeed.Length>0){
            SpawnSeed();
            StartCoroutine(CoroutineSpawnSeed());
        }
    }

    /// <summary>
    /// Fonction qui verifie les positions disponibles pour generer une salle a proximite
    /// </summary>
    public void Scan(){
        for (int i = 0; i < _listPositions.Count; i++){ // boucle selon la taille de la liste de positions a verifier
            bool detection = Physics2D.Raycast((Vector2)transform.position+_listPositions[i], _listPositions[i], 0.1f, _layerSol); // on lance un rayon pour toucher le _layerSol
            if(!detection){ // si le rayon n'a pas toucher de sol
                _listFreePos.Add((Vector2)transform.position + _listPositions[i]); // on ajoute la position du laser dans la lsite des positions disponibles
            }
        }
        if(_listFreePos.Count > 0){ // s'il y a au moins 1 position disponible dans _listFreePos
            _genSalle.GenererSalles(_listFreePos); // on demande a _genSalle de generer les salle selon les position disponibles
        }
    }

    /// <summary>
    ///  Fonction qui cree un detecteur pour chaque detecteur de _tDetecteurs et supprime les tuiles en conséquent
    /// </summary>
    public void CreerDetecteurs()
    {
        Tilemap laCarte = transform.GetChild(4).GetComponent<Tilemap>(); // Tilemap des cote de la salle
        foreach (GameObject detecteur in _tDetecteurs) // boucle pour chaque detecteur dans _tDetecteurs
        {
            Transform leParent = detecteur.transform.parent; // on accede au transform du parent
            Collider2D col = Physics2D.OverlapCircle(detecteur.transform.position, 0.3f, _layerSol); // on cree un collider qui determine s'il touche au sol
            _toucheAuxBlocs = (col != null); //  la valeur de _toucheAuxBlocs est selon si le collider touche au sol
            Vector3 posParent2 = detecteur.transform.parent.position; // on accede a la position du parent
            if (_toucheAuxBlocs) // si _toucheAuxBlocs est vrai
            {
                Vector3Int posCell = laCarte.GetComponentInParent<Grid>().WorldToCell(posParent2); // on transform posParent2 en position d'une tuile et on stock cette position dans posCell 
                laCarte.SetTile(posCell, null); // on dit au tilemap des cotes de supprimer la tuile a posCell
                Destroy(leParent.gameObject); // on detruit le gameObject du parent
            }
        }
    }

    /// <summary>
    /// Fonction qui determine si un Mimo doit apparaitre
    /// </summary>
    private void SpawnMimo(){
        foreach (Transform posMimo in _tPosMimo) // boucle qui passe chaque position du tableau _tPosMimo
        {
            int spawnMimo = Random.Range(0, 2); // on fait un hasard entre 0 et 1
            if(spawnMimo == 1){ // si le resultat est 1
                GameObject mimo = Instantiate(_goMimo, posMimo.position, Quaternion.identity); // on genere un Mimo a la position PosMimo
                mimo.transform.SetParent(transform);
            }
        }
    }

    private IEnumerator CoroutineSpawnSeed(){

        yield return new WaitForSeconds(10);
        SpawnSeed();
        int chancePop = Random.Range(0,101);
        if(chancePop >= 50){
            StartCoroutine(CoroutineSpawnSeed());
        }
    }

    private void SpawnSeed(){
        foreach (Transform posSeed in _tPosSeed)
        {
            bool posOccupe = Physics2D.Raycast(posSeed.position, Vector2.right, 0.1f, _layerTache);
            if(!posOccupe){
                int chanceSeed = Random.Range(0,101);
                if(chanceSeed >= _chanceSeed){
                    GameObject seed = Instantiate(_goSeed, transform.position, Quaternion.identity);
                    seed.transform.SetParent(transform);
                    seed.transform.position = posSeed.position;
                }
            }
        }
    }

    /// <summary>
    /// Fonction qui genere aleatoirement des taches dans les salles
    /// </summary>
    private void GenererTaches(){
        foreach (Transform pos in _tPosTaches) // pour chaque Transform dans _tPosTaches
        {
            int chanceTache = Random.Range(0,2); // on genere un chiffre entre 0 et 1
            if(chanceTache == 0){ // si le chiffre est 0
                int randomTache = Random.Range(0, _tGoTache.Length); // on genere un chiffre entre 0 et la longueur du tableau des GameObject des taches
                GameObject tache = Instantiate(_tGoTache[randomTache], transform.position, Quaternion.identity); // on instantie la tache a la position de la salle
                tache.transform.SetParent(transform); // la salle devient le parent de la tache
                tache.transform.position = pos.position; // on change la position de la tache pour la position de pos
            }
            else{ // si le chiffre est 1
                // Debug.Log("Aucune tache instanciée");
            }
        }
    }
}
