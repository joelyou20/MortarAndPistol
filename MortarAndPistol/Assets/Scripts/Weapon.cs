using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private enum ShootType
    {
        Horizontal,
        TowardsMouse
    }

    public Transform firePoint;
    public bool holdToShoot = true;
    [Range(0, 1f)] public float timeBetweenShots = 0.3f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speed = 20;
    [SerializeField] private ShootType shootType = ShootType.Horizontal;

    private Rigidbody2D _playerRb;
    private float _timer = 0f;

    void Start ()
    {
        _playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (holdToShoot)
        {
            if (Input.GetButton("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }

    private Quaternion GetRotation()
    {
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }

    private void Shoot()
    {
        if (_timer >= timeBetweenShots)
        {
            _timer = 0;
            switch (shootType)
            {
                case ShootType.Horizontal:
                    Horizontal_Shoot();
                    break;
                case ShootType.TowardsMouse:
                    TowardsMouse_Shoot();
                    break;
                default:
                    break;
            }
        }
    }

    private void TowardsMouse_Shoot()
    {
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - transform.position;

        //...instantiating the bullet
        GameObject bulletInstance = Instantiate(
            bullet,
            firePoint.position,
            GetRotation());

        bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(
            shootDirection.x,
            shootDirection.y)
            .normalized * speed + _playerRb.velocity;
    }

    private void Horizontal_Shoot()
    {
        //...instantiating the bullet
        GameObject bulletInstance = Instantiate(
            bullet,
            firePoint.position,
            firePoint.rotation);

        bulletInstance.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }
}
