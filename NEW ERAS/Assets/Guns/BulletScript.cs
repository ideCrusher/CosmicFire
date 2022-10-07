using UnityEngine;

public class BulletScript : BulletStats
{
    private Rigidbody _RB;
    private bool _Start = true;
    // Start is called before the first frame update
    void Start()
    {
        _RB = GetComponentInChildren<Rigidbody>();       
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
        Vector3 _tAR = new Vector3(_EnemyTarget.x, 1f, _EnemyTarget.z);
        Vector3 targetDirection = _tAR - transform.parent.position;
        Quaternion rott = Quaternion.LookRotation(targetDirection);
        Quaternion rottt = new Quaternion(0, rott.y, rott.z, rott.w);
        transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, rottt, 50f * Time.deltaTime);
        if(transform.parent.rotation == rottt && _Start)
        {
            _RB.velocity = transform.forward * Speed;
            _Start = false;
        }
    }
}
