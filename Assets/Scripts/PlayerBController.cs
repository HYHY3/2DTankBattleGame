using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBController : MonoBehaviour
{
    [SerializeField]
    float _tankSpeed = 2.0f;
    [SerializeField]
    GameObject _bulletPrefab = null;

    private const string _LaunchPortNameOfPlayerTank = "LaunchPort";
    private const float _LaunchBulletTimeInterval = 0.6f;

    private static string _BulletNameOfPlayerTankLaunched = "BulletOfPlayerB";
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

        if (!_wasBulletLaunched && Input.GetKeyDown(KeyCode.M))
        {
            foreach (Transform child in this.transform)
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
        bool val = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);
        if (_isMovingToForward && !val)
        {
            _isMovingToForward = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            _isMovingToForward = true;
            //Debug.Log("PlayerBController.judgeTankMove() UpArrow key is down.");
            return;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            _isMovingToForward = true;
            //Debug.Log("PlayerBController.judgeTankMove() DownArrow key is down.");
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            _isMovingToForward = true;
            //Debug.Log("PlayerBController.judgeTankMove() LeftArrow key is down.");
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            _isMovingToForward = true;
            //Debug.Log("PlayerBController.judgeTankMove() RightArrow key is down.");
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
        Debug.Log("PlayerBController.OnCollisionEnter2D Collider Name = " + collision.collider.name);

        BulletController bullet = collision.collider.GetComponent<BulletController>();
        if (bullet != null)
        {
            //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
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

        //Debug.Log("PlayerBController.OnCollisionEnter2D is done.");
    }
}
