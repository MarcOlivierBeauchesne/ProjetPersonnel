using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Skill Explication", fileName = "SkillExplication")]
public class SkillExplication : ScriptableObject
{
    [SerializeField] private string _nomSkill;
    public string nomSkill{ get => _nomSkill; }
    [TextArea]
    [SerializeField] private string _explication;
    public string explication{ get => _explication; }
    private string _actualStack;
    public string actualStack{ get => _actualStack; }
}
