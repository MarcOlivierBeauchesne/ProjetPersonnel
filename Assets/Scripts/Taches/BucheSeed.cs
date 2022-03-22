using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script qui gere le comportement d'une souche
/// </summary>
public class BucheSeed : MonoBehaviour
{
    [Header("Information de la souche")] // identification de la section Information de la souche
    [SerializeField] private int _minSeed; //nombre minimal de noix necessaire
    [SerializeField] private int _maxSeed; // nombre maximal de noix necessaire
    [SerializeField] private int _taskValue; // valeur de la tache
    [Header("Visuels")] // identification de la section Visuels
    [SerializeField] private SpriteRenderer[] _tSeedVisual; // tableau des visuels de noix de la tache
    [SerializeField] private Sprite _imgEmptySeed; // image d'un emplacement de noix vide
    [SerializeField] private Sprite _imgFullSeed; // image d'un emplacement de noix plein
    [Header("Sons")] // identification de la section Sons
    [SerializeField] AudioClip _sonSouche; // son quand le joueur met une noix dans la souche

    private bool _isFull = false; // indicatif si la souche est pleine ou non
    private int _seedAmount = 0; // nombre de noix actuel dans la souche
    private int _seedNeeded = 0; // nombre de noix necessaire pour remplir la souche
    private bool _playerClose = false; // indicatif si le joueur est proche de la souche ou non

    GenerateurSalle _genSalle; // reference au GenerateurSalle
    Personnage _perso; // reference au Personnage
    PlayerRessources _resPlayer;  // reference au PlayerRessources
    // Start is called before the first frame update
    void Start()
    {
        _genSalle = GetComponentInParent<GenerateurSalle>(); // _genSalle prend la valeur du GenerateurSalle du parent
        _perso = _genSalle.perso; // _perso prend la valeur du perso du _genSalle
        _resPlayer = _perso.ressourcesPlayer; // _resPlayer prend la valeur du ressourcesPlayer de _perso
        _seedNeeded = CalculerSeedNeeded(); // _seedNeeded prend la valeur de retour de CalculerSeedNeeded
        AjusterSeedNeeded(); // on appel AjusterSeedNeeded
    }

    /// <summary>
    /// fonction qui calcule le nombre de noix necessaire pour remplir la souche
    /// </summary>
    /// <returns>quantite de noix necessaire pour remplir la souche</returns>
    private int CalculerSeedNeeded(){
        int seed = Random.Range(_minSeed, _maxSeed +1); // seed prend une valeur aleatoire entre _minSeed et _maxSeed +1
        return seed; // on retourne seed
    }

    /// <summary>
    /// fonction qui ajuste le visuel des noix sur la tache
    /// </summary>
    private void AjusterSeedNeeded(){
        for (int i = 0; i < _tSeedVisual.Length; i++) // pour selon la taille du tableau _tSeedVisual
        {
            if(i<_seedNeeded){ // si i est plus petit que _seedNeeded
                _tSeedVisual[i].sprite = _imgEmptySeed; // on prend le visuel de _tSeedVisual[i] et on affiche une noix vide
            }
            else{ // si i est plus grand ou egal a _seedNeeded
                _tSeedVisual[i].gameObject.SetActive(false); // on desactive le gameObject de _tSeedVisual[i]
            }
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isFull){ // si le tage du collider entrant est Player et que _isFull est false
            _playerClose = true; // _playerClose devient true
            if(_genSalle.tuto.dictTips["TipsSouche"] == false){ // si dictTips avec la key TipsSouche est false
                _genSalle.tuto.gameObject.SetActive(true); // on active la fenetre de tutoriel
                _genSalle.tuto.OuvrirTips(5); // on active le tips a l'index 5
            }
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && !_isFull){ // si le tage du collider entrant est Player et que _isFull est false
            _playerClose = false; // _playerClose devient false
        }
    }

    /// <summary>
    /// fonction qui ajuste le visuel des noix presentes dans la souche
    /// </summary>
    private void AjusterSeedVisuel(){
        for (int i = 0; i < _seedAmount; i++) // boucle selon _seedAmount
        {
            _tSeedVisual[i].sprite = _imgFullSeed; // le sprite de _tSeedVisual[i] devient une noix pleine
        }
    }

    /// <summary>
    /// fonction qui ajoute une noix dans la souche
    /// </summary>
    private void AjouterSeed(){
        if(_resPlayer.seedAmount >= 1){ // si le joueur possede au moins 1 noix
            _seedAmount++; // on augmente le nombre de noix dans la souche de 1
            _perso.AjusterPoint("seed", -1, TypeTache.Aucun); // on enleve une noix au joueur
            GameAudio.instance.JouerSon(_sonSouche); // on joue un son quand le joueur ajoute une noix dans la souche
            AjusterSeedVisuel(); // on appel AjusterSeedVisuel
            if(_seedAmount == _seedNeeded){ // si le nombre de noix dans la souche a atteint son objectif
                _isFull = true; // _isFull est true (souche pleine)
                _playerClose = false; // le joueur n'est plus proche
                int totalPoint = _seedAmount * _taskValue; // le total de point donne le nombre de noix multiplie par _taskValue
                GetComponent<Tache>().FinirTache(totalPoint); // on accede au Tache du gameObject et on finit la tache en envoyant totalPoint
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _playerClose && !_isFull){ // si le joueur appuie sur E ET qu'il est proche ET que la souche n'est pas pleine
            AjouterSeed(); // on appel AjouterSeed
        }
    }
}
