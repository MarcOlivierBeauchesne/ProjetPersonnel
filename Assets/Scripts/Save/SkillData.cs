using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] 
public class SkillData
{
    public int actualStack;
    public int skillCost;

    public SkillData(SkillInfos infos){
        actualStack = infos.actualStack;
        skillCost = infos.skillCost;
    }
}
