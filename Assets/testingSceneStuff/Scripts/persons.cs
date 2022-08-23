using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System;
[Serializable]
[ProtoContract]
public class persons
{
    [ProtoMember(1)]
    public string GroupName;
    [ProtoMember(2)]
    public person[] GroupMembers;

}

[Serializable]
[ProtoContract]
public class person
{
    [ProtoMember(1)]
    public string Name;
    [ProtoMember(2)]
    public int Age;
}