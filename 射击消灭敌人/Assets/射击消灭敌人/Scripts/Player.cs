using System;
using UnityEngine;
public class Player : MonoBehaviour
{
    public float speed = 1000f;
    Vector3 moveInput;
    public float health;
    public bool dead = false;
    Plane plane;
    Rigidbody rig;
    Vector3 Velocity;
    public Transform gun;
    public Transform spawn;
    public Projectile projectile;
    float timer;
    Vector3 veloccity;
    float angle;
    float speedref;
    public static Player Instance;
    protected  void Start()
    {

           health = 10;
        gun = GameObject.Find("Gun").transform;
        spawn = GameObject.Find("Projectile Spawn").transform;
        projectile = Resources.Load<Projectile>("Projectile") as Projectile;
    }
    private void Awake()
    {
        Instance = this;
        rig = GetComponent<Rigidbody>();
      
    }
    void OnGameOver()
    {
        Game.Insatnce.End(false);
        Debug.LogError("jieshu");
    }
    void Update()
    {
        if (transform.position.y < -5)
        {
            Die();
        }
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Velocity = moveInput * speed;
            plane = new Plane(Vector3.up, Vector3.zero );
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
            Debug.DrawLine(ray.origin, point, Color.red);

        }
        if (Input.GetMouseButton(0))
        {
            
           Shoot();
        }
        GunUpdate();
    }
    public void Shoot()
    {
        if (Time.time > timer)
        {
            Game.Insatnce.MP3(true);
            timer = Time.time + 0.1f; //��һ�ο����ʱ�� 1+0.1
             Instantiate(projectile, spawn.position, spawn.rotation);
            gun.transform.localPosition -= Vector3.forward;
        }
    }
    void GunUpdate()
    {
        gun.transform.localPosition = Vector3.SmoothDamp(gun.transform.localPosition, Vector3.zero, ref veloccity, 0.1f);
        angle = Mathf.SmoothDampAngle(angle, 0, ref speedref, 0.1f);
        gun.transform.localEulerAngles = new Vector3(angle, gun.transform.localEulerAngles.y, gun.transform.localEulerAngles.z);
    }
    public  void Die()
    {
        dead = true;
        OnGameOver();
        Spawn.Instance.isDisabled = false;
        this.gameObject.transform.localScale = Vector3.zero;
    }
    public  void TashDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !dead) { 
            Die();
        }
    }
    private void FixedUpdate()
    {
        rig.AddForce(Velocity);
        if (Velocity.x == 0)
            rig.linearVelocity = new Vector3(0, rig.linearVelocity.y, rig.linearVelocity.z);
        if (Velocity.z == 0)
            rig.linearVelocity = new Vector3(rig.linearVelocity.x, rig.linearVelocity.y, 0);
        rig.linearVelocity = new Vector3(Mathf.Clamp(rig.linearVelocity.x, -5, 5), rig.linearVelocity.y, Mathf.Clamp(rig.linearVelocity.z, -5, 5));
    }
}
