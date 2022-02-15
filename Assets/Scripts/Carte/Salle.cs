using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salle : MonoBehaviour
{
    [SerializeField] private LayerMask _layerSol;
    [SerializeField] private GameObject _balise;
    private List<Vector2> _listFreePos = new List<Vector2> { };
    private List<Vector2> _listPositions = new List<Vector2> 
    {   new Vector2(0,11),
        new Vector2(0,-11),
        new Vector2(-19,0),
        new Vector2(19,0),
    };

    private GenerateurSalle _genSalle;
    public GenerateurSalle genSalle{
        get => _genSalle;
        set{
            _genSalle = value;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Scan();
        if(_listFreePos.Count > 0){
            _genSalle.GenererSalles(_listFreePos);
        }
    }

    private void Scan(){
        for (int i = 0; i < 4; i++){
            bool detection = Physics2D.Raycast((Vector2)transform.position+_listPositions[i], _listPositions[i], 0.1f, _layerSol);
            if(!detection){
                _listFreePos.Add((Vector2)transform.position + _listPositions[i]);
                GameObject balise = Instantiate(_balise, transform.position, Quaternion.identity);
                balise.transform.SetParent(transform);
                balise.transform.position = ((Vector2)transform.position + _listPositions[i]);
            }
        }
        for (int i = 0; i < _listFreePos.Count; i++)
        {
            Debug.Log(_listFreePos[i] + gameObject.name);
        }
    }
}
