using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] Mission[] _tMissions;
}

public enum TypeMission
{
    Tache,
    Mimo,
    Arbre
}
