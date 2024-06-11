using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "ObjectData/CreatObjectData" , order = int.MaxValue)]
public class ObjectStatus : ScriptableObject
{
    [Header("스테이터스")]
    [SerializeField] private int hp;
    public int HP {get { return hp; } }
    [SerializeField] private int power; 
    public int Power {get { return hp; } }
}
