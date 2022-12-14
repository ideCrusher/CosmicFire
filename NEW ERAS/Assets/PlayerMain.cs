using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private bool _Move = false;
    private GameObject _MainCamera;
    private Transform _Cam;

    private Transform _Children;

    // The target marker.
    public Transform target;

    // Angular speed in radians per sec.
    public float speed = 3f;
    private Vector3 _target;   



    void Start()  
    {
        _MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _Cam = _MainCamera.GetComponent<Transform>();
        _target = transform.position;
        Rigidbody f = GetComponent<Rigidbody>();
        f.freezeRotation = true;
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, 2f, transform.position.z);
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _target = new Vector3(movement.a.x, 2.0f, movement.a.z);
            _Move = true;
        }
        if (_Move)
        {
            MoveTo();
        }
    }

    void MoveTo()
    {                 
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
        _Cam.position = new Vector3(transform.position.x, 8, transform.position.z - 5f);
        rotTo();
        if (transform.position == _target)
        {
            _Move = false;
        }            
    }

    void rotTo()
    {
        Vector3 targetDirection = _target - transform.position;       
        Quaternion rott = Quaternion.LookRotation(targetDirection);
        // Rotate the cube by converting the angles into a quaternion.
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, rott.y, rott.z, rott.w), 3f * Time.deltaTime);       
    }
}
