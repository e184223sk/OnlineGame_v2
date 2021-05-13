using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 
public enum JointNormalVector
{
    [EnumKANA_("+x")] px,
    [EnumKANA_("-x")] mx,
    [EnumKANA_("+y")] py,
    [EnumKANA_("-y")] my,
    [EnumKANA_("+z")] pz,
    [EnumKANA_("-z")] mz,
    [EnumKANA_("None")] none,
}

[AttributeUsage(AttributeTargets.Field)]
public class EnumKANA_Attribute : Attribute
{
    public string Name { get; set; }
    public EnumKANA_Attribute(string name) => Name = name;
}