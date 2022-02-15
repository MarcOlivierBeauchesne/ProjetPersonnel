using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurSalle : MonoBehaviour
{
    [SerializeField] private GameObject _firstSalle;
    [SerializeField] private GameObject _salleForet;
    [SerializeField] private GameObject _salleCoupe;
    private int pourcentage = 20;
    [SerializeField] private int _nbSalle = 10;
    private int _qteSalleForet = 10;
    private int _qteSalleCoupe = 10;

    private List<GameObject> _listSalle = new List<GameObject> { };
    private List<Vector2> _listPosDispo = new List<Vector2> { };

    private /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {   
        GenererFirstSalle();
    }

    public void GenererFirstSalle(){
        ClearRooms();
        _qteSalleCoupe = Mathf.RoundToInt((_nbSalle * pourcentage)/100);
        _qteSalleForet = _nbSalle - _qteSalleCoupe;
        GameObject salle = Instantiate(_firstSalle, Vector3.zero, Quaternion.identity);
        salle.transform.SetParent(transform);
        salle.GetComponent<Salle>().genSalle = this;
        _listSalle.Add(salle);
    }

    private void ClearRooms(){
        foreach(GameObject salle in _listSalle){
            Destroy(salle);
        }
        _listSalle.Clear();
    }

    public void GenererSalles(List<Vector2> _listPos){
        _listPosDispo = _listPos;
        GameObject salle = null;
        if(_qteSalleCoupe + _qteSalleForet >= 1){
            //int nbSallePop = Random.Range(1, 4);
            int nbSalleSpawn = Mathf.Clamp(Random.Range(1,4), 1, _qteSalleCoupe+_qteSalleForet);
            nbSalleSpawn = Mathf.Clamp(nbSalleSpawn, 0, _listPosDispo.Count);
            for (int i = nbSalleSpawn; nbSalleSpawn > 0; nbSalleSpawn--){
                int posNewList = Random.Range(0, _listPosDispo.Count);
                int quelleSalle = Random.Range(0, 2);
                if (quelleSalle == 0)
                {
                    if (_qteSalleForet >= 1)
                    {
                        salle = Instantiate(_salleForet, _listPos[posNewList], Quaternion.identity);
                        _qteSalleForet--;
                    }
                    else
                    {
                        salle = Instantiate(_salleCoupe, _listPos[posNewList], Quaternion.identity);
                        _qteSalleCoupe--;
                    }
                }
                else if (quelleSalle == 1)
                {
                    if (_qteSalleCoupe >= 1)
                    {
                        salle = Instantiate(_salleCoupe, _listPos[posNewList], Quaternion.identity);
                        _qteSalleCoupe--;
                    }
                    else
                    {
                        salle = Instantiate(_salleForet, _listPos[posNewList], Quaternion.identity);
                        _qteSalleForet--;
                    }
                }
                _listPosDispo.RemoveAt(posNewList);
                salle.transform.SetParent(transform);
                salle.GetComponent<Salle>().genSalle = this;
                _listSalle.Add(salle);
            }
        }
        else{
            Debug.Log("Il n'y a plus de salle a faire spawn");
        }
    }

    public void ChangerPourcent(string value){
        pourcentage = System.Convert.ToInt32(value);
        Debug.Log(pourcentage + "%");
    }
}
