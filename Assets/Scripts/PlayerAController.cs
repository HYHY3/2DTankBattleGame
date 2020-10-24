using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAController : MonoBehaviour
{
    [SerializeField]
    float _tankSpeed = 2.0f;
    [SerializeField]
    GameObject _bulletPrefab = null;

    private const string _LaunchPortNameOfPlayerTank = "LaunchPort";
    private const float _LaunchBulletTimeInterval = 0.6f;

    private static string _BulletNameOfPlayerTankLaunched = "BulletOfPlayerA";
    private static bool _isMovingToForward = false;

    private int _tankLifeCount;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private GameObject _bullet;
    private bool _wasBulletLaunched;
    private float _timeCDOfBulletLaunch;


    public static string getBulletNameLaunched()
    {
        return _BulletNameOfPlayerTankLaunched;
    }

    void Awake()
    {
        _tankLifeCount = 2;
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _isMovingToForward = false;
        _wasBulletLaunched = false;
        _timeCDOfBulletLaunch = 0.0f;
    }

    void Update()
    {
        //GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePosition;
        judgeAnyKeyPressed();
        moveTank();
    }

    void judgeAnyKeyPressed()
    {
        judgeTankAttack();
        judgeTankMove();

    }

    void judgeTankAttack()
    {
        if (_wasBulletLaunched)
        {
            _timeCDOfBulletLaunch += Time.deltaTime;
            if (_timeCDOfBulletLaunch > _LaunchBulletTimeInterval)
            {
                _timeCDOfBulletLaunch = 0.0f;
                _wasBulletLaunched = false;
            }
        }

        if (!_wasBulletLaunched && Input.GetKeyDown(KeyCode.V))
        {
            //Debug.Log("PlayerAController.judgeTankAttack() B key is down.");
            foreach(Transform child in this.transform)
            {
                if (child.name == _LaunchPortNameOfPlayerTank)
                {
                    _bullet = Instantiate(_bulletPrefab, child.position, child.rotation);
                    _bullet.name = _BulletNameOfPlayerTankLaunched;
                    _bullet.transform.parent = transform;
                    _wasBulletLaunched = true;
                }
            }
            return;
        }
    }

    void judgeTankMove()
    {
        bool val = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        if (_isMovingToForward && !val)
        {
            _isMovingToForward = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            _isMovingToForward = true;
            //Debug.Log("PlayerAController.judgeTankMove() W key is down.");
            return;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            _isMovingToForward = true;
            //Debug.Log("PlayerAController.judgeTankMove() S key is down.");
            return;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            _isMovingToForward = true;
            //Debug.Log("PlayerAController.judgeTankMove() A key is down.");
            return;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            _isMovingToForward = true;
            //Debug.Log("PlayerAController.judgeTankMove() D key is down.");
            return;
        }
    }

    void moveTank()
    {
        if (_isMovingToForward)
        {
            transform.Translate(Vector3.up * Time.deltaTime * _tankSpeed);
            return;
        }

        transform.Translate(Vector3.zero);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("PlayerAController.OnCollisionEnter2D Collider Name = " + collision.collider.name);

        BulletController bullet = collision.collider.GetComponent<BulletController>();
        if (bullet != null)
        {
            if (bullet.name == EnemyController.getBulletNameLaunched())
            {
                Destroy(bullet.gameObject);
                --_tankLifeCount;
                if (_tankLifeCount == 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = _initialPosition;
                    transform.rotation = _initialRotation;
                }
                return;
            }
        }

        //Debug.Log("PlayerAController.OnCollisionEnter2D is done.");
    }
}
