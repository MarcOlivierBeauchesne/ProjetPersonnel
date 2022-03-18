using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// Script qui s'occupe du comportement d'une salle et de la generation des noix, des tache, des mimos et des ennemis
/// </summary>
public class Salle : MonoBehaviour
{
    [Header("Layers")] // identification de la section Layers
    [SerializeField] private LayerMask _layerSol; // layer du sol d'une tache
    [SerializeField] private LayerMask _layerTache; // layer d'une tache
    [SerializeField] private LayerMask _layerSolDefo; // layer du sol d'une salle de deforestation
    [Header("Positions dans la salle")] // identification de la section Positions dans la salle
    [SerializeField] GameObject[] _tDetecteurs; // tableaux des detecteurs de la salle
    [SerializeField] Transform[] _tPosTaches; // tableau des positions possible des taches
    [SerializeField] Transform[] _tPosMimo; // tableau des positions possible des Mimos
    [SerializeField] Transform[] _tPosSeed; // tableau des position possible pour une noix
    [SerializeField] Transform[] _tPosTransit; // tableau des position pour les transitions entre les salles
    [SerializeField] List<Transform> _listPosEnnemi; // liste des positions possible pour apparaitre une ennemi
    [Header("GameObject a generer")] // identification de la section GameObject a generer
    [SerializeField] GameObject _goMimo; // gameObject d'un mimo
    [SerializeField] GameObject _goSeed; // gameOjbect d'une noix
    [SerializeField] GameObject _goEnnemiForet; // gameOjbect d'une ennemis
    [SerializeField] GameObject[] _tGoTache; // tableau des taches possibles
    [Header("Noix et Varia")] // identification de la section Noix et Varia
    [SerializeField] private int _chanceSeed = 30; // chance qu'une noix apparaisse par position
    [SerializeField] private int _spawnNoixDelay = 10;
    [SerializeField] GameObject[] _tLayerTransit; // tableau pour les GameOjbect des layers de transition

    private List<Transform> _listPosEnnemiTemp = new List<Transform>(); // liste temporaire de toutes les positions possible des ennemis
    private List<Transform> _listPosEnnemiSpawn = new List<Transform>(); // liste active des positions utilises par ennemis
    private List<GameObject> _listProjectiles = new List<GameObject>(); // liste de tous les projectiles presents
    private int _actualRoomEnnemi; // nombre d'ennemis dans la salle
    private int _ennemiToSpawn; // nombre d'ennemis a generer
    private int _ennemiTaskValue; // valeur de la tache de destruction d'ennemis
    private int _spawnedEnnemy; // nombre d'ennemis apparu pour la tache

    private bool _peutGenererEnnemi = true; // bool pour savoir si on peut generer des ennemis
    private bool _toucheAuxBlocs; // bool qui determine si un detecteur touche a _layerTuile

    private GenerateurSalle _genSalle; // acces prive pour le GenerateurSalle
    public GenerateurSalle genSalle{ // acces public pour le GenerateurSalle
        get => _genSalle; // par genSalle, on retourne_genSalle
        set{
            _genSalle = value; // par genSalle, on change la valeur de _genSalle
        }
    }

    Text _txtTacheEnnemis; // on stock le champs de text de la boite d'ennemis
    Text _txtTacheProjectile; // on stock le champs de text de la boite de projectile

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _txtTacheEnnemis = _genSalle.boiteEnnemis.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Text>(); // on stock le champs Text du premier enfant de la boite ennemis dans _txtTacheEnnemis
        _txtTacheProjectile = _genSalle.boiteProjectiles.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>(); // on stock le champs Text du premier enfant de la boite projectile dans _txtTacheProjectile
        GenererTaches(); // on appel GenererTache
        SpawnMimo(); // on appel SpawnMimo
        if(_tPosSeed.Length>0){ // si la longueur du tableau _tPosSeed est plus grand que 0
            SpawnSeed(); // on appel SpawnSeed
            StartCoroutine(CoroutineSpawnSeed()); // on demarre la coroutine CoroutineSpawnSeed
        }
    }

    /// <summary>
    ///  Fonction qui cree un detecteur pour chaque detecteur de _tDetecteurs et supprime les tuiles en consequent
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
                mimo.transform.SetParent(transform); // le parent du gameObject devient la salle
                mimo.transform.GetChild(0).GetComponent<Collectible>().tuto = _genSalle.tuto; // on attribue le tuto du Collectible par le tuto de _genSalle
            }
        }
    }

    /// <summary>
    /// Coroutine qui fait apparaitre des noix
    /// </summary>
    /// <returns>temps d'attente</returns>
    private IEnumerator CoroutineSpawnSeed(){
        yield return new WaitForSeconds(_spawnNoixDelay); // temps d'attente selon _spawnNoixDelay
        SpawnSeed(); // on appel SpawnSeed
        int chancePop = Random.Range(0,101); // chancePoP donne une chiffre entre 0 et 100
        if(chancePop >= 50){ // si chancePop est plus grand ou egal a 50
            StartCoroutine(CoroutineSpawnSeed()); // on redemarre la coroutine actuelle
        }
    }

    /// <summary>
    /// Fonction qui ajoute un projectile a la liste des projectiles
    /// </summary>
    /// <param name="projectile">gameObject du projetile a ajouter</param>
    public void AjouterProjectile(GameObject projectile){
        _listProjectiles.Add(projectile); // on ajoute projectile a _listProjectiles
    }

    /// <summary>
    /// Fonction qui genere une noix
    /// </summary>
    private void SpawnSeed(){
        foreach (Transform posSeed in _tPosSeed) // boucle pour chaque position dans le tableau _tPosSeed
        {
            bool posOccupe = Physics2D.Raycast(posSeed.position, Vector2.right, 0.1f, _layerTache); // posOccupe est egal selon si le ray touche une noix ou pas
            if(!posOccupe){ // si posOccupe ne touche pas de noix
                int chanceSeed = Random.Range(0,101); // chanceSeed donne un chiffre entre 0 et 100
                if(chanceSeed >= _chanceSeed){ // si chanceSeed et plus grand ou egal a _chanceSeed
                    GameObject seed = Instantiate(_goSeed, transform.position, Quaternion.identity); // on genere une noix
                    seed.transform.SetParent(transform); // la noix devient enfant de la salle
                    seed.transform.position = posSeed.position; // on change la position de la noix pour la position de posSeed
                    seed.GetComponent<Seed>().tuto = _genSalle.tuto; // le tuto de Seed devient le tuto de _genSalle
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
            int chanceTache = Random.Range(0,4); // on genere un chiffre entre 0 et 1
            if(chanceTache < 3){ // si le chiffre est 0
                int randomTache = Random.Range(0, _tGoTache.Length); // on genere un chiffre entre 0 et la longueur du tableau des GameObject des taches
                GameObject tache = Instantiate(_tGoTache[randomTache], transform.position, Quaternion.identity); // on instantie la tache a la position de la salle
                tache.transform.SetParent(transform); // la salle devient le parent de la tache
                tache.transform.position = pos.position; // on change la position de la tache pour la position de pos
                tache.GetComponent<Tache>().perso = _genSalle.perso.GetComponent<Personnage>();
                tache.GetComponent<Tache>().tuto = _genSalle.tuto; // le tuto de salle Tache devient le tuto de _genSalle
            }
            else{ // si le chiffre est 1
            }
        }
    }

    /// <summary>
    /// Fonction qui prepare la salle a generer des ennemis
    /// </summary>
    /// <param name="taskValue">valur de la tache de generation d'ennemis</param>
    public void GenererEnnemi(int taskValue){
        if(_peutGenererEnnemi){
            _listPosEnnemiTemp.Clear(); // on vide la liste _listPosEnnemiTemp
            _listPosEnnemiSpawn.Clear(); // on vide la liste _listPosEnnemiSpawn
            _ennemiTaskValue = taskValue; // _ennemiTaskValue prend la valeur de taskValue
            _ennemiToSpawn = Mathf.Clamp(5 * genSalle.timer.nbJour, 5, _listPosEnnemi.Count); // _ennemiToSpawn prend une valeur aleatoire restreinte entre 5 et la longeur de _listPosEnnemi
            _spawnedEnnemy += _ennemiToSpawn; // _spawnedEnnemy s'ajoute la valeur de _ennemiToSpawn
            _genSalle.boiteEnnemis.SetActive(true); // on active la boite de description d'ennemis de _genSalle
            _txtTacheEnnemis.text = _actualRoomEnnemi.ToString(); // _txtTacheEnnemis affiche _actualRoomEnnemi
            SpawnEnnemiForet(); // on appel SpawnEnnemiForet
        }
    }

    /// <summary>
    /// Fonction qui retire un ennemi de la salle
    /// </summary>
    public void RetirerEnnemi(){
        _actualRoomEnnemi--; // on reduit _actualRoomEnnemi de 1
        _txtTacheEnnemis.text = _actualRoomEnnemi.ToString(); // _txtTacheEnnemis affiche _actualRoomEnnemi
        if(_actualRoomEnnemi == 0){ // si _actualRoomEnnemi est egal a 0
            _genSalle.boiteEnnemis.SetActive(false); // on desactive la boite de description d'ennemis de _genSalle
            int totalPoint = _spawnedEnnemy * _ennemiTaskValue; // totalPoint est egal au _spawnedEnnemy multiplie par _ennemiTaskValue
            _genSalle.perso.AjusterPoint("naturePoint", totalPoint, TypeTache.Tache); // on prend le perso de _genSalle et on lui demande de s'ajouter des naturePoint avec totalPoint
            _genSalle.perso.missionManager.AccomplirMission(TypeMission.Tache); // on prend le missionManager du perso de _genSalle et on demande d'accomplir la mission de type Tache
        }
    }

    /// <summary>
    /// Fonction qui fait augmenter la deforestation par les ennemis
    /// </summary>
    /// <param name="amount">augmentation de la deforestation</param>
    public void AugmenterDeforestation(float amount){
        _genSalle.deforestation.AugmentationEnnemi(amount * _genSalle.timer.nbJour); // on demande a deforestation du _genSalle d'agmenter la deforestation par (amount x nbJour) du Timer de _genSalle
    }

    /// <summary>
    /// Fonction qui fait detruire les ennemis et les projectiles
    /// </summary>
    public void DetuireEnnemis(){
        TacheCentre[] _tTacheCentre = GetComponentsInChildren<TacheCentre>(); // on stock tous les TacheCentre des enfants de la salle dans _tTacheCentre
        EnnemiForet[] _tEnnemisForet = GetComponentsInChildren<EnnemiForet>(); // on stock tous les EnnemiForet des enfants de la salle dans _tEnnemisForet
        foreach (TacheCentre tacheCentre in _tTacheCentre) // pour chaque TacheCentre dans _tTacheCentre
        {
            tacheCentre.ArreterSpawn(); // on demande a tacheCentre d'appeler ArreterSpawn
        }
        foreach (EnnemiForet ennemi in _tEnnemisForet) // pour chaque EnnemiForet dans _tEnnemisForet
        {
            ennemi.ArreterCoupe(); // on demande a ennemi d'appeler ArreterCoupe
        }
        if(_listProjectiles.Count > 0){ // si la longueur de _listProjectiles est plus grand que 0
            foreach (GameObject projectile in _listProjectiles) // poru chaque GameObject dans _listProjectiles
            {
                Destroy(projectile); // on detruit le projectile
            }
        }
    }

    /// <summary>
    /// Fonction qui place les transitions entre les salles de deforestation et de foret
    /// </summary>
    public void PlacerTransition(){
        for (int i = 0; i < _tPosTransit.Length; i++) // boucle tant que i est plus petit que la longueur de _tPosTransit
        {   
            bool toucheDefo = Physics2D.Raycast(_tPosTransit[i].position, Vector2.up, 0.1f, _layerSolDefo); // toucheDefo represente si le ray touche le layer _layerSolDefo ou non
            if(toucheDefo){ // si toucheDefo est vrai
                _tLayerTransit[i].SetActive(true); // on active la transition a la position i dans _tLayerTransit
            }
        }
    }

    /// <summary>
    /// Fonction qui fait apparaitre les ennemis dans la foret
    /// </summary>
    private void SpawnEnnemiForet(){
        for (int i = 0; i < _listPosEnnemi.Count; i++) // boucle tant que i est plus petit que la longueur de _listPosEnnemi
        {
            _listPosEnnemiTemp.Add(_listPosEnnemi[i]); // on ajoute la _listPosEnnemi[i] a _listPosEnnemiTemp
        }
        for (int i = 0; i < _ennemiToSpawn; i++) // boucle tant que i est plus petit que _ennemiToSpawn
        {
            int posRandom = Random.Range(0, _listPosEnnemiTemp.Count); // posRandom represente un chiffre entre 0 et la longueur de _listPosEnnemiTemp
            _listPosEnnemiSpawn.Add(_listPosEnnemiTemp[posRandom]); // on ajoute _listPosEnnemiTemp[posRandom] a _listPosEnnemiSpawn
            _listPosEnnemiTemp.RemoveAt(posRandom); // on enleve la position a l'index posRandom de _listPosEnnemiTemp
        }
        int refEnnemi = _ennemiToSpawn; // refEnnemis est egal a _ennemiToSpawn
        for (int i = 0; i < refEnnemi; i++) // boucle tant que i est plus petit que refEnnemi
        {
            _ennemiToSpawn--; // on reduit _ennemiToSpawn de 1
            GameObject ennemi = Instantiate(_goEnnemiForet, _listPosEnnemiSpawn[i].position, Quaternion.identity); // on genere un ennemis et on le stock dans ennemi
            _actualRoomEnnemi++; // on augmente _actualRoomEnnemi de 1
            ennemi.transform.SetParent(transform); // ennemi devient un enfant de la salle
        }
        _txtTacheEnnemis.text = _actualRoomEnnemi.ToString(); // _txtTacheEnnemis affiche _actualRoomEnnemi
    }

    /// <summary>
    /// Fonction qui permet d'activer la boite de description de la tache des projectiles
    /// </summary>
    public void AfficherTacheProjectile(){
        _genSalle.boiteProjectiles.SetActive(true); // on active la boiteProjectiles de _genSalle
    }

    /// <summary>
    /// Fonction qui permet d'ajuster l'affichage du nombre de projectile present dans la salle
    /// </summary>
    /// <param name="nbProjectile">nombre de projectile present</param>
    public void AjusterAffichageProjectiles(int nbProjectile){
        _txtTacheProjectile.text = nbProjectile.ToString(); // _txtTacheProjectile affiche nbProjectile
        if(nbProjectile == 0){ // si nbProjectile est egal a 0
            _genSalle.boiteProjectiles.SetActive(false); // on desactive la boiteProjectiles du _genSalle
        }
    }
}