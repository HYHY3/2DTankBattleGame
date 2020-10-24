using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //private const int _PlayerBulletDirection = 1; //up
    //private const int _EnemyBulletDirection = 2; //down


    private float _screenWidthLimit;
    private float _screenHeightLimit;

    [SerializeField]
    float _bulletSpeed = 10.0f;


    void Awake()
    {
        _screenWidthLimit = Screen.width / 2;
        _screenHeightLimit = Screen.height / 2;
    }

    void Update()
    {
        if(transform.position.x > _screenWidthLimit || transform.position.x < -_screenWidthLimit ||
            transform.position.y > _screenHeightLimit || transform.position.y < -_screenHeightLimit)
        {
            Destroy(gameObject);
        }

        if (gameObject.name == PlayerAController.getBulletNameLaunched() ||
          gameObject.name == PlayerBController.getBulletNameLaunched())
        {
            transform.Translate(Vector3.up * Time.deltaTime * _bulletSpeed);
        }
        else if (gameObject.name == EnemyController.getBulletNameLaunched())
        {
            transform.Translate(Vector3.down * Time.deltaTime * _bulletSpeed);
        }

    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
        //Debug.Log("BulletController.OnTriggerEnter2D name = " + collision.name);

        //PlayerAController tankA = collision.GetComponent<PlayerAController>();
        //if (tankA != null)
        //{
        //    return;
        //}

        //PlayerBController tankB = collision.GetComponent<PlayerBController>();
        //if (tankB != null)
        //{
        //    return;
        //}

        //EnemyController enemy = collision.GetComponent<EnemyController>();
        //if (enemy != null)
        //{
        //    return;
        //}

        //Destroy(gameObject);
        //Debug.Log("BulletController.OnTriggerEnter2D is done.");
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("BulletController.OnCollisionEnter2D Collider Name = " + collision.collider.name);

        PlayerAController tankA = collision.collider.GetComponent<PlayerAController>();
        if (tankA != null)
        {
            return;
        }

        PlayerBController tankB = collision.collider.GetComponent<PlayerBController>();
        if (tankB != null)
        {
            return;
        }

        //EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        //if (enemy != null)
        //{
        //    return;
        //}

        Destroy(gameObject);
        //Debug.Log("BulletController.OnCollisionEnter2D is done.");
    }

}
