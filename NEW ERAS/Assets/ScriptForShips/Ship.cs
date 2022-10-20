using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public int Tier;

    public string Faction;

    public GameObject EnemyTarget;
    public Vector3 Target;

    public int HealsPoint;
    public int Armor;
    public float Speed;

    public float RadiusForLook;
    public float RadiusForShooting;

    public List<Guns> GunList = new List<Guns>();

    public Hangar[] HangarList;
}
