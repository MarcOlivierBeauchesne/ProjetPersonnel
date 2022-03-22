using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantage : MonoBehaviour
{
    [Header("Prefab arbre")] // identification de la section Prefab arbre
    [SerializeField] private GameObject _goArbre; // gameObject de l'arbre a planter
    [Header("Layers de permission")] // identification de la section Layers de permission
    [SerializeField] private LayerMask _layerSolDefo; // Layer du sol de deforestation
    [SerializeField] private LayerMask _layerTache; // layer des tache

    private List<GameObject> _listGoArbres = new List<GameObject>(); // liste pour contenir tous les arbres plantes

    Personnage _perso; // reference au Personnage

    private void Start()
    {
        _perso = GetComponent<Personnage>(); // _perso devient la composante Personnage du gameObject actuel
    }

    /// <summary>
    /// Fonction publique qui plante un arbre sur un sol de deforestation
    /// </summary>
    public void PlanterArbre(){
        int posX = Mathf.FloorToInt(transform.position.x); // posX prend la position actuelle en x du perso arrondi a l'inferieur
        int posY = Mathf.FloorToInt(transform.position.y); // posY prend la position actuelle en y du perso arrondi a l'inferieur
        Vector2 posPossible = new Vector2(posX, posY); // posPossible est forme par posX et posY

        bool solDefo = Physics2D.Raycast(posPossible, Vector2.down, 0.1f, _layerSolDefo); // solDefo verifie a la position posPossible si un ray touche a un sol de deforestation
        bool tache = Physics2D.Raycast(posPossible, Vector2.down, 0.1f, _layerTache); // tache verifie a la position posPossible si un ray touche a une tache
        if(solDefo && !tache){ // si le ray touche a un sol de deforestation et pas a une tache
            GameObject arbre = Instantiate(_goArbre, posPossible, Quaternion.identity); // on genere un arbre a la position posPossible
            _perso.AjusterPoint("seed", -1, TypeTache.Aucun); // on demande au Personnage de reduire sa quantite de noix de 1
            _perso.AjusterPoint("naturePoint", 10 + (int)_perso.basicStats.npGain, TypeTache.Arbre); // on demande au Personnage de se donner des points de type Arbre
            _perso.missionManager.AccomplirMission(TypeMission.Arbre); // on demande au MissionManager du Personnage d'accomplir la mission de plantage d'arbre
            _listGoArbres.Add(arbre); // on ajoute l'arbre a la liste des arbres plantes
        }
    }

    /// <summary>
    /// Fonction publique qui supprime tous les arbres qui ont ete generer 
    /// </summary>
    public void NettoyerArbre(){
        foreach (GameObject arbre in _listGoArbres) // pour chaque arbre dans la liste _listGoArbres
        {
            Destroy(arbre); // on supprimer l'arbre
        }
        _listGoArbres.Clear(); // on vide la liste d'arbre
    }
}
