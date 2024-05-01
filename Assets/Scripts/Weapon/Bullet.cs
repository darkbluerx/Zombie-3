using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage;
    public float lifeTime = 2f;

    private void OnEnable()
    {
        Invoke("DestroyBullet", lifeTime);
    }

    public void SetDamage(int newDamage) //newDamage is parameter that takes the damage amount
    {
        damage = newDamage;
    }

    private void DestroyBullet()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Map")) DestroyBullet(); //if bullet hits map, return bullet to pool

        //Check if the bullet hit a gameobject with health component
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            // Call the TakeDamage method and pass the damage amount
            int remainingHealth = health.TakeDamage(damage);
            //Debug.Log($"{collision.gameObject.name} ottaa vahinkoa: {damage}. Jäljellä oleva terveys: {remainingHealth}");

            DestroyBullet(); //return bullet to pool
        }
    }
}
