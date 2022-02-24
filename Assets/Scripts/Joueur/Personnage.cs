using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script qui controle le deplacement du perso et met a jour ses ressources
/// </summary>
public class Personnage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtNaturePoint; // acces prive pour le champs de texte des points de nature du joueur
    [SerializeField] private TextMeshProUGUI _txtNaturePower; // acces prive pour le champs de texte de la puissance naturelle du joueur
    [SerializeField] private TextMeshProUGUI _txtSeed;
    [SerializeField] private PlayerRessources _ressourcesPlayer; // reference de PlayerRessources du joueur
    [SerializeField] private TaskManager _taskManager;
    [SerializeField] private GameObject _goArbre;
    public PlayerRessources ressourcesPlayer{
        get=> _ressourcesPlayer;
    }
    [SerializeField] private BasicStats _basicStats; // reference au BasicStats
    [SerializeField] private LayerMask _layerTache;

    private float _mouvementSpeed = 5; // acces prive pour _mouvementSpeed
    private float _axeX = 0f; // acces prive pour _axeX du Input Horizontal
    private float _axeY = 0f; // acces prive pour _axeY du Input Vertical

    Rigidbody2D _rb; // on stoc le rigidBody2D
    Animator _anim;
    SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>(); // anim s'associr au AnimatorController du perso
        _rb = GetComponent<Rigidbody2D>(); // _rb s'associe au RigidBody 2D du perso
        _sr = GetComponent<SpriteRenderer>(); // _sr s'associe au SpriteRenderer du perso
        _mouvementSpeed = _basicStats.mouvementSpeed; // _mouvementSped devient la valeur du mouvementSpeed du BasicStats
        _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString(); // on affiche les points de nature dans le champs approprie
        _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on affiche la puissance naturelle dans le champs approprie
        _txtSeed.text = _ressourcesPlayer.seedAmount.ToString();
    }

    /// <summary>
    /// Fonction qui ajuste les points de nature, de puissance et de graines du joueur et 
    /// met a jour le champs de texte apprporie
    /// </summary>
    /// <param name="ressources">Type de ressources que l'on modifie</param>
    /// <param name="valeur">valeur que l'on doit ajuster a la ressources</param>
    public void AjusterPoint(string ressources , int valeur){
        switch (ressources) // selon le type de ressource
        {
            case "naturePower": // si la ressource est "naturePower"
                _ressourcesPlayer.naturePower += valeur; // on change le naturePower des _ressourcesPlayer selon la valeur
                _txtNaturePower.text = _ressourcesPlayer.naturePower.ToString(); // on met a jour l'affichage des point de nature
                break; // on sort de la condition
            case "seed": // si la ressources est "seed"
                _ressourcesPlayer.seedAmount += valeur; // on change la quantite de graine du joueur
                _txtSeed.text = _ressourcesPlayer.seedAmount.ToString();
                break; // on sort de la condition
            case "naturePoint": // si la ressources est "naturePoint"
                _ressourcesPlayer.naturePoint += valeur; // on change les naturePoint du _ressourcesPlayer selon la valeur
                _txtNaturePoint.text = _ressourcesPlayer.naturePoint.ToString();
                break; // on sort de la condition
        }
    }

    /// <summary>
    /// Fonction public pour mettre a jour la vitesse de deplacement du joueur
    /// </summary>
    public void ModifierMoveSpeed(){
        _mouvementSpeed = _basicStats.mouvementSpeed; // _mouvementSped devient la valeur du mouvementSpeed du BasicStats
    }

    /// <summary>
    /// Fonction qui met prend les valuers des Input Horizontal et Vertical du joueur
    /// </summary>
    private void Mouvement(){
        _axeX = Input.GetAxisRaw("Horizontal"); // _axeX prend la valeur du Input de l'axe Horizontal
        _axeY = Input.GetAxisRaw("Vertical"); // _axeY prend la valeur du Input de l'axe Vertical
        bool enMouvement = _axeX != 0 || _axeY != 0; // enMouvement est true si le personnage bouge
        _anim.SetBool("Move", enMouvement); // on met le bool "Move" selon enMouvement
        if(_axeX < 0){ // si _axeX est plus petit que 0
            _sr.flipX = true; // on inverse l'orientation du perso en X
        }
        else if(_axeX > 0){ // si _axeX est plus grand que 0
            _sr.flipX = false; // on inverse pas l'orientation du perso en x
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Mouvement(); // on appel Mouvement
        if(Input.GetKeyDown(KeyCode.Space)){
            if(_ressourcesPlayer.seedAmount >=1){
                PlanterArbre();
            }
        }
    }

    private void PlanterArbre(){
        int posX = Mathf.FloorToInt(transform.position.x);
        int posY = Mathf.FloorToInt(transform.position.y);
        Vector2 posPossible = new Vector2(posX, posY);

        bool placePrise = Physics2D.Raycast(posPossible, Vector2.right, 0.1f, _layerTache);
        if(!placePrise){
            Instantiate(_goArbre, posPossible, Quaternion.identity);
            AjusterPoint("seed", -1);
            _taskManager.AjouterPoint(TypeTache.Arbre, 10);
            AjusterPoint("naturePoint", 10 + (int)_basicStats.npGain);
        }
        else{
            Debug.Log("Il y a deja un arbre a cet endroit");
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_axeX * _mouvementSpeed, _axeY * _mouvementSpeed); // on bouge le RigidBody2D du perso ave un Vector2 de _axeX et _axeY multiplie par la _mouvementSpeed
    }

}
