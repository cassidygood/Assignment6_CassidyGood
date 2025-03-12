using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour
{
    public float wanderRadius = 10f;         // How far the enemy can roam
    public float wanderTimer = 3f;           // Time between random movements
    public float chaseRange = 20f;           // How far the enemy can "see" the player
    public float viewAngle = 20f;            // Enemy's field of view (FOV)
    public float viewDistance = 120f;         // How far the enemy can detect the player

    public Transform player;                 // Reference to the player

    private NavMeshAgent agent;              // Reference to the NavMeshAgent
    private float timer;                     // Timer to track movement intervals

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        timer = wanderTimer;                 // Initialize the timer
    }

    void Update()
    {
        if (player != null && CanSeePlayer())
        {
            // Chase the player
            agent.SetDestination(player.position);
        }
        else
        {
            // Wander when player is out of sight
            Wander();
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;

        // When the timer hits the wander interval, pick a new random destination
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavMeshLocation(wanderRadius);
            agent.SetDestination(newPos);   // Move to the new random position
            timer = 0;                      // Reset the timer
        }
    }

    // Get a random point on the NavMesh within a radius
    Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius; // Random point in a sphere
        randomDirection += transform.position;                     // Offset it to the enemy's current position

        NavMeshHit hit;                                             // NavMesh hit data
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            return hit.position;                                    // Return the valid position
        }
        return transform.position;                                  // Fallback: stay in the current position
    }

    // Check if the enemy can see the player
    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Check distance and field of view
        if (directionToPlayer.magnitude <= viewDistance && angleToPlayer <= viewAngle / 2)
        {
            // Raycast to check for obstacles
            Ray ray = new Ray(transform.position, directionToPlayer.normalized);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, viewDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(transform.position, directionToPlayer.normalized * hit.distance, Color.red);
                    return true; // Player is visible
                }
            }
        }

        return false; // Player is not visible
    }
}
