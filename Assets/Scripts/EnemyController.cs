using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float _tankSpeed = 2.0f;
    [SerializeField]
    GameObject _bulletPrefab = null;

    private const string _LaunchPortNameOfEnemyTank = "LaunchPort";
    private const float _LaunchBulletTimeInterval = 1.2f;

    private static string _BulletNameOfEnemyTankLaunched = "BulletOfEnemy";

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private GameObject _bullet;
    private bool _wasBulletLaunched;
    private float _timeCDOfBulletLaunch;
    private float _preTankDirection;


    public static string getBulletNameLaunched()
    {
        return _BulletNameOfEnemyTankLaunched;
    }


    void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _wasBulletLaunched = true;
        _timeCDOfBulletLaunch = -1.0f;
        _preTankDirection = transform.rotation.z;
    }

    void Update()
    {
        judgeTankProcess();

    }

    void judgeTankProcess()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _tankSpeed);
        judgeTankAttack();
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

        if (!_wasBulletLaunched)
        {
            foreach (Transform child in this.transform)
            {
                if (child.name == _LaunchPortNameOfEnemyTank)
                {
                    _bullet = Instantiate(_bulletPrefab, child.position, child.rotation);
                    _bullet.GetComponent<SpriteRenderer>().flipY = true;
                    _bullet.name = _BulletNameOfEnemyTankLaunched;
                    _bullet.transform.parent = transform;
                    _wasBulletLaunched = true;
                }
            }

            return;
        }
    }

    void changeTankDirection()
    {
        int directionCnt;

        //Debug.Log("EnemyController.changeTankDirection " + gameObject.name + " Predirection = " + gameObject.transform.rotation.z);
        while (Mathf.Abs(Mathf.Abs(_preTankDirection) - Mathf.Abs(transform.rotation.z)) <= 0.05f)
        {
            directionCnt = Random.Range(1, 4);
            while (directionCnt != 0)
            {
                transform.Rotate(new Vector3(0, 0, 90));
                --directionCnt;
            }
        }

        _preTankDirection = transform.rotation.z;
        //Debug.Log("EnemyController.changeTankDirection " + gameObject.name + " Curdirection = " + gameObject.transform.rotation.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("EnemyController.OnCollisionEnter2D Collider Name = " + collision.collider.name);

        BulletController bullet = collision.collider.GetComponent<BulletController>();
        if (bullet != null)
        {
            //Debug.Log("EnemyController.OnCollisionEnter2D Bullet Name = " + bullet.name);
            if (bullet.name == PlayerAController.getBulletNameLaunched() ||
                bullet.name == PlayerBController.getBulletNameLaunched())
            {
                Destroy(bullet.gameObject);
                Destroy(gameObject);
                return;
                //Debug.Log("EnemyController.OnCollisionEnter2D Enemy is destoried.");
            }
        }

        _timeCDOfBulletLaunch = 0.0f;
        changeTankDirection();
        //Debug.Log("EnemyController.OnCollisionEnter2D is done.");
    }

}
