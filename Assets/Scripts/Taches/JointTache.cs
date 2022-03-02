using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointTache : MonoBehaviour
{
    [SerializeField] TacheDestruction _tacheDestruction;

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        _tacheDestruction.UserJoint();
    }
}
