using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Object
{
    [Range(0, 100)]
    public int defense;

    Structure(string _name, string _status, int _ID)
    {
        this.name = _name;
        this.status = _status;
        this.ID = _ID;
        this.type = ObjectType.Structure;
        this.health = 100;
        this.defense = 100;
    }
}
