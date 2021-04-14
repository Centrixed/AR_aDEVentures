using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Structure = 0,
    Worker = 1
}

public abstract class Object : MonoBehaviour
{
    public new string name;
    public string status;
    public int ID;
    public ObjectType type;

    [Range(0, 100)]
    public int health;

}
