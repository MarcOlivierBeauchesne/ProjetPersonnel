using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JointTache : MonoBehaviour
{
    [SerializeField] TacheDestruction _tacheDestruction;
    [SerializeField] int _minClic = 0;
    [SerializeField] int _maxClic = 0;
    private int _actualClic = 0;
    public int actualClic{
        get=>_actualClic;
    }
    Timer _timer;
    public Timer timer{
        get=>_timer;
        set{
            _timer = value;
        }
    }
    private int _goalClic = 0;

    private void Start()
    {
        _goalClic = Random.Range(_minClic, _maxClic +1) * _timer.nbJour;
        _actualClic = _goalClic;
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = _actualClic.ToString();
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        _actualClic--;
        transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = _actualClic.ToString();
        _tacheDestruction.UserJoint(_actualClic, _goalClic);
    }
}
