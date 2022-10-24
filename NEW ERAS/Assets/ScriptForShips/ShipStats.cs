﻿using UnityEngine;
using UnityEngine.UI;

public class ShipStats : Ship
{
    //// MOVEMETY FOR BOTS
    private bool _Target = false, _Move = false, _Shoot = false;
    private Vector3 _StayPoint;
    private GameObject Empty;

    //// Patrol movement bots   
    public bool Patrol = false;
    public Vector3 _PositionRotateOne, _PositionRotateTwo;

        
    private Transform _Children;

    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed = 1.0f;
    public Vector3 _target;

    //// FIND ON ENEMY
    private bool StarterLocator = false;
    private Collider[] _EnemyShipsLocator;
    private Collider[] _EnemyShipsForShooting;


    //// OTHER
    public bool isBot = false;

    private GameObject _MainCamera;
    private Transform Pos;

    GameObject Rocket;
    GameObject Bullet;

    private bool _FireFirstSlot = true, _FireSecondSlot = true;
    private int _ProjectileCounterOne = 0, _ProjectileCounterTwo = 0;

    private bool _ReloadingFirstSlot = false, _ReloadingSecondSlot = false;
    private int _ReloadingCounterOne = 0, _ReloadingCounterTwo = 0;
    private Text _ReloadingRocket, _ReloadingMachingun;
      

    // Start is called before the first frame update
    void Start()
    {      
        if(!isBot)
        {
            _ReloadingRocket = GameObject.Find("RocketCooldown").GetComponent<Text>();
            _ReloadingMachingun = GameObject.Find("MachingunCooldown").GetComponent<Text>();
            _MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }      
        if(isBot)
        {
            _StayPoint = transform.position;
            Empty = new GameObject();
            Target = Empty;
            TargetForShooting = Empty;
            EnemyTarget = new Vector3(100,100,100);
            InvokeRepeating("Locator", 1f, 1f);           
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(!isBot)
        {
            if (HealsPoint < 0)
            {
                Destroy(gameObject);
            }
            if (Input.GetKeyDown(KeyCode.E) && _FireFirstSlot)
            {
                _FireFirstSlot = false;
                InvokeRepeating("ShootingMachingun", 0, 0.3f);
            }
            if (Input.GetKeyDown(KeyCode.R) && Target != null && _FireSecondSlot)
            {
                _FireSecondSlot = false;
                InvokeRepeating("ShootingRocketLuncher", 0, 1.0f);
            }
        }      
        if(isBot)
        {          
            if(StarterLocator)
            {
                foreach (var Enemy in _EnemyShipsLocator)
                {
                    if (Enemy.gameObject.CompareTag("Ship"))
                    {                     
                        if (Enemy.gameObject.GetComponentInParent<ShipStats>().Faction != Faction)
                        {
                            float New = (Enemy.gameObject.transform.parent.position - transform.position).sqrMagnitude, Old = (EnemyTarget - transform.position).sqrMagnitude;
                            print($"Distance for Enemy: {New}");
                            if (New < Old)
                            {
                                _Target = true;
                                Target = Enemy.gameObject;
                                _Move = true;
                            }
                        }                       
                    }
                    else
                    {                       
                        Target = Empty;
                        _Target = false;
                        _Move = false;
                        _target = _StayPoint;                      
                    }
                }
                foreach(var EnemyShoot in _EnemyShipsForShooting)
                {
                    if(EnemyShoot.gameObject == Target)
                    {
                        _Shoot = true;
                    }
                    else
                    {
                        _Shoot = false;
                    }
                }
            }          
        }
    }
    private void FixedUpdate()
    {
        Pos = transform;
        if (isBot)
        {
            if (_Move && !_Shoot && !_Target)
            {
                if(Target != Empty)
                {
                    _target = Target.transform.position;
                }
                MoveTo();
            }
            if(_Move && _Shoot && _Target)
            {
                print("СТОЙ СУКА!!!");
                _Move = false;
            }
            if (Target == Empty)
            {
                MoveTo();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rocket"))
        {
            if (other.gameObject.GetComponentInParent<Rocket>().Faction != Faction)
            {
                int dd = other.gameObject.GetComponentInChildren<Rocket>().Damage;
                HealsPoint = HealsPoint - dd;
                print($"{HealsPoint}");
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (other.gameObject.GetComponentInParent<BulletScript>().Faction != Faction && other.gameObject.GetComponentInParent<BulletScript>().Ready)
            {
                int dd = other.gameObject.GetComponentInChildren<BulletScript>().Damage;
                HealsPoint = HealsPoint - dd;
                print($"{HealsPoint}");
                Destroy(other.gameObject);
            }
        }
    }

    void ShootingMachingun()
    {
        _ProjectileCounterOne++;
        Bullet = Instantiate(GunList[0].Prefab, new Vector3(Pos.position.x, 1.5f, Pos.position.z), Pos.rotation);
        Bullet.GetComponentInChildren<BulletScript>(true)._EnemyTarget = movement.a;
        Bullet.GetComponentInChildren<BulletScript>(true).Faction = Faction;
        if (_ProjectileCounterOne == GunList[0].ProjectileForOneShot)
        {           
            _ProjectileCounterOne = 0;
            InvokeRepeating("ReloadingFirstSlot", 1.0f, 1.0f);
            CancelInvoke("ShootingMachingun");          
        }
        Bullet = null;
    }    

    void ShootingRocketLuncher()
    {
        _ProjectileCounterTwo++;
        Rocket = Instantiate(GunList[1].Prefab, new Vector3(Pos.position.x, 1f, Pos.position.z), Pos.rotation);
        Rocket.GetComponentInChildren<Rocket>().EnemyTarget = Target;
        Rocket.GetComponentInChildren<Rocket>().Faction = Faction;
        if (_ProjectileCounterTwo == GunList[1].ProjectileForOneShot)
        {
            _ProjectileCounterTwo = 0;
            InvokeRepeating("ReloadingSecondSlot", 1.0f, 1.0f);
            CancelInvoke("ShootingRocketLuncher");
        }
    }

    void ReloadingFirstSlot()
    {
        _ReloadingMachingun.text = (GunList[0].Cooldown - _ReloadingCounterOne).ToString();
        if (_ReloadingCounterOne == GunList[0].Cooldown)
        {
            _FireFirstSlot = true;
            _ReloadingCounterOne = 0;
            _ReloadingMachingun.text = "Ready";
            CancelInvoke("ReloadingFirstSlot");
        }
        _ReloadingCounterOne = _ReloadingCounterOne + 1;
    }
    void ReloadingSecondSlot()
    {
        _ReloadingRocket.text = (GunList[1].Cooldown - _ReloadingCounterTwo).ToString();       
        if (_ReloadingCounterTwo == GunList[1].Cooldown)
        {
            _FireSecondSlot = true;
            _ReloadingCounterTwo = 0;
            _ReloadingRocket.text = "Ready";
            CancelInvoke("ReloadingSecondSlot");
        }
        _ReloadingCounterTwo = _ReloadingCounterTwo + 1;
    }


    void MoveTo()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, 3f * Time.deltaTime);
        rotTo();
    }

    void rotTo()
    {
        Vector3 targetDirection = _target - transform.position;
        Quaternion rott = Quaternion.LookRotation(targetDirection);
        // Rotate the cube by converting the angles into a quaternion.
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, rott.y, rott.z, rott.w), 3f * Time.deltaTime);
    }



    void Locator()
    {

        _EnemyShipsLocator = Physics.OverlapSphere(transform.position, RadiusForLook);
        _EnemyShipsForShooting = Physics.OverlapSphere(transform.position, RadiusForShooting);
        if(!StarterLocator)
        {
            StarterLocator = true;
        }
    }
    //void OnDrawGizmosSelected()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(transform.position, RadiusForLook);
    //
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, RadiusForShooting);
    //}
}
