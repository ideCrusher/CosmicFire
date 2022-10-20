﻿using UnityEngine;
using UnityEngine.UI;

public class ShipStats : Ship
{
    //// MOVMETY FOR BOTS
    private bool _Move = false;

    private Transform _Children;

    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed = 1.0f;
    public Vector3 _target;

    //// FIND ON ENEMY
    Collider[] _EnemyShipsLocator;
    Collider[] _EnemyShipsForShooting;


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
            MoveTo();

            if (_EnemyShipsLocator.Length != 0)
            {              
                foreach(var Enemy in _EnemyShipsLocator)
                {
                    if(Enemy.gameObject.CompareTag("Ship"))
                    {
                        if(Enemy.gameObject.GetComponentInParent<ShipStats>().Faction != Faction)
                        {                           
                                float New = (Enemy.gameObject.transform.parent.position - transform.position).sqrMagnitude, Old = (Target - transform.position).sqrMagnitude;
                                if (New < Old)
                                {

                                    EnemyTarget = Enemy.gameObject;
                                    print($"{Enemy.gameObject.name}");
                                }                            
                        }                     
                    }                 
                }
            }
            if (Target != null)
            {
                _target = Target;
                _Move = true;
            }
        }
    }
    private void FixedUpdate()
    {
        Pos = transform;
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
        Rocket.GetComponentInChildren<Rocket>(true)._EnemyTarget = Target;
        Rocket.GetComponentInChildren<Rocket>(true).Faction = Faction;
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
    }
}
