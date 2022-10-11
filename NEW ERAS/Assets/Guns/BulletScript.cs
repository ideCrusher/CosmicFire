using UnityEngine;

public class BulletScript : BulletStats
{
    private Rigidbody _RB;
    private int _Timer = 0;
    public bool Ready = false;

    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponentInChildren<Rigidbody>();
        InvokeRepeating("TimerOfDestruction",1f,1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        rotTo();
    }

    void rotTo()
    {
        Vector3 _tAR = new Vector3(_EnemyTarget.x, 1.5f, _EnemyTarget.z);
        Vector3 targetDirection = _tAR - transform.parent.position;
        Quaternion rott = Quaternion.LookRotation(targetDirection);
        Quaternion rottt = new Quaternion(0, rott.y, rott.z, rott.w);
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rottt, 50f * Time.deltaTime);
        _RB.velocity = transform.forward * Speed;       
    }


    void TimerOfDestruction()
    {
        _Timer++;
        if(_Timer == 7)
        {
            CancelInvoke("TimerOfDestruction");
            Destroy(gameObject);          
        }
    }

    //Я знаю что частое обращение к компанентам вредит производительности!
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ship"))
        {
            Ready = true;
        }
    }
}
