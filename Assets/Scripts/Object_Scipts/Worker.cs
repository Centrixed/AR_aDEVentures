using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Object
{
    public string task;

    Worker(string _name, string _status, int _ID)
    {
        this.name = _name;
        this.status = _status;
        this.ID = _ID;
        this.type = ObjectType.Worker;
        this.health = 100;
        this.task = "Idle";
    }

}
