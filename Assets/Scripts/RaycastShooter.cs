using UnityEngine;

public class RaycastShooter : MonoBehaviour
{
    public float rayDistance = 100f;
    public LayerMask raycastLayerMask; // Set to Enemy layer in inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, raycastLayerMask))
        {
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.green, 1f);

            // Check if the hit object has the EnemyWander script
            EnemyWander enemy = hit.collider.GetComponent<EnemyWander>();
            if (enemy != null)
            {
                ScoreManager.Instance.AddScore(5); // Add to score
                Destroy(enemy.gameObject); // Remove the enemy
            }
        }
    }
}


