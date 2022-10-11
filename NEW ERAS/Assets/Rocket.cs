using UnityEngine;

public class Rocket : RocketStats
{

    private int _Timer = 0;
     //Start is called before the first frame update
    void Start()
    {
        _Move = true;
        InvokeRepeating("TimerOfDestruction",1f,1f);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(HealsPoint < 0)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        _EnemyTargetPosition = _EnemyTarget;
        if (_Move)
        {
            MoveToEnemyShip();
        }
    }

    void MoveToEnemyShip()
    {
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(_EnemyTargetPosition.x, 1.5f, _EnemyTargetPosition.z), Speed * Time.deltaTime);
        rotTo();
        if (transform.parent.position == _EnemyTargetPosition)
        {
            _Move = false;
        }
    }
    
    void rotTo()
    {
        Vector3 targetDirection = _EnemyTargetPosition - transform.parent.position;
        Quaternion rott = Quaternion.LookRotation(targetDirection);
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, new Quaternion(0, rott.y, rott.z, rott.w), 4f * Time.deltaTime);
    }

    void TimerOfDestruction()
    {
        _Timer++;
        if (_Timer == 12)
        {
            CancelInvoke("TimerOfDestruction");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
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

}
