using UnityEngine;
//×Óµ¯
public class Projectile : MonoBehaviour
{
    float moveDistance;
    public LayerMask collisionMask;
    private float damage = 1;
    private void Start()
    {
        Destroy(gameObject, 2);
        Collider[] initalCollisions = Physics.OverlapSphere(transform.position, .5f, collisionMask);
        if (initalCollisions.Length > 0)
            OnHitObject(initalCollisions[0], transform.position);
    }
    void Update()
    {
        moveDistance = 20 * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
        CheckColsions(moveDistance);
    }
    void CheckColsions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, moveDistance + 0.1f, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }
    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        Enemy idamageble = c.GetComponent<Enemy>();
        if (idamageble != null)
        {
            idamageble.TaskHit(damage, hitPoint, transform.forward);
        }
        Destroy(gameObject);
    }
}
