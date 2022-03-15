using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantage : MonoBehaviour
{
    [SerializeField] private GameObject _goArbre;
    [SerializeField] private LayerMask _layerSolDefo;
    [SerializeField] private LayerMask _layerTache;
    private List<GameObject> _tGoArbres = new List<GameObject>();

    Personnage _perso;

    private void Start()
    {
        _perso = GetComponent<Personnage>();
    }

    public void PlanterArbre(){
        int posX = Mathf.FloorToInt(transform.position.x);
        int posY = Mathf.FloorToInt(transform.position.y);
        Vector2 posPossible = new Vector2(posX, posY);

        bool solDefo = Physics2D.Raycast(posPossible, Vector2.down, 0.1f, _layerSolDefo);
        bool tache = Physics2D.Raycast(posPossible, Vector2.down, 0.1f, _layerTache);
        if(solDefo && !tache){
            GameObject arbre = Instantiate(_goArbre, posPossible, Quaternion.identity);
            _perso.AjusterPoint("seed", -1, TypeTache.Aucun);
            _perso.taskManager.AjouterPoint(TypeTache.Arbre, 10 + (int)_perso.basicStats.npGain);
            _perso.AjusterPoint("naturePoint", 10 + (int)_perso.basicStats.npGain, TypeTache.Arbre);
            _perso.missionManager.AccomplirMission(TypeMission.Arbre);
            _tGoArbres.Add(arbre);
        }
        else{
            Debug.Log("Il y a deja un arbre a cet endroit");
        }
    }

    public void NettoyerArbre(){
        foreach (GameObject arbre in _tGoArbres)
        {
            Destroy(arbre);
        }
        _tGoArbres.Clear();
    }
}
