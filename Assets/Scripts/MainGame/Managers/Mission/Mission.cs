using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Mission")]
public class Mission : ScriptableObject
{
    [SerializeField] TypeMission _typeMission;
    public TypeMission typeMission{
        get=>_typeMission;
    }
    [SerializeField] string _nomMission = "Ramassage";
    public string nomMission{
        get=>_nomMission;
    }
    [SerializeField] int _missionAmount;
    public int missionAmount{
        get=>_missionAmount;
        set{
            _missionAmount = value;
        }
    }
    [TextArea]
    [SerializeField] string _descriptionMission;
    public string descriptionMission{
        get=>_descriptionMission;
    }
    [SerializeField] int _rewardValue = 0;
}
