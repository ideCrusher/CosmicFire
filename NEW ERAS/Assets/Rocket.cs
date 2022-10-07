using UnityEngine;

public class Rocket : RocketStats
{
     //Start is called before the first frame update
    void Start()
    {
        _Move = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        
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
    
    void OnTriggerEnter(Collider other)
    {
        
    }

}
