using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script qui gere les joints de la machine a detruire
/// </summary>
public class JointTache : MonoBehaviour
{
    [SerializeField] TacheDestruction _tacheDestruction; // reference au TacheDestruction
    [SerializeField] int _minClic = 0; // nombre de clic minimum pour detruire le joint
    [SerializeField] int _maxClic = 0; // nombre de clic maximum pour detruit le joint
    private int _actualClic = 0; // nombre de clic actuel sur le joint
    public int actualClic{ // acces public au nombre de clic actuel sur le join
        get=>_actualClic; // par actualClic, on retourne _actualClic
    }
    Timer _timer; // reference au Timer
    public Timer timer{ // acces public au timer 
        get=>_timer; // par timer, on retourne _timer
        set{ // on change la valeur du _timer
            _timer = value; // _timer prend al valeur de value
        }
    }
    private int _goalClic = 0; // nombre de clic necessire pour briser le joint

    private void Start()
    {
        _goalClic = Random.Range(_minClic, _maxClic +1) * _timer.nbJour; // _goalClic prend une valeur entre _minClic et _maxClic +1 et multiplie par le nombre de jour actuel
        _actualClic = _goalClic; // _actualClic prend la valeur de _goalClic
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = _actualClic.ToString(); // on affiche le nombre de clic necessaire sur le joint
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        _actualClic--; // on reduit _actualClic de 1
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = _actualClic.ToString(); // on affiche le nombre de clic necessaire sur le joint
        _tacheDestruction.UserJoint(_actualClic, _goalClic); // on demande a TacheDesctruction de mettre a jour le joint
    }
}
