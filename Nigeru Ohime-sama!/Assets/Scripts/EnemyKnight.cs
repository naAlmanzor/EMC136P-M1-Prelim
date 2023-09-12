using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyKnight : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10.0f;
    private Vector2 waypointFlatPosition;
    private Vector2 playerFlatPosition;
    private Transform currentWaypoint;
    [SerializeField] private PatrolPoints patrolPoints;
    private Animator anims;
    private Vector2 lastFramePosition;
    private bool isWaiting = false;

    private enum State
    {
        Patrol,
        Alert,
        Chase,
    };

    private State state;

    private Transform playerTransform;
    private PlayerController player;

    [Header("SphereCast Settings")]
    public float sphereRadius = 1f; // Radius of the sphere
    public float sphereCastDistance = 5f; // How far the SphereCast will check
    public LayerMask hitLayers; // Layers the SphereCast will interact with
    private RaycastHit2D hitInfo; // Info about what the SphereCast hit

    [Header("SphereCast Direction")]
    public bool castHorizontally = true;

    [SerializeField] private GameStats gameStats;




    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerTransform = player.transform;

        currentWaypoint = patrolPoints.GetNextWaypoint(currentWaypoint);
        anims = GetComponent<Animator>();
        lastFramePosition = transform.position;  // Initialize the last frame position

        state = State.Patrol;
    }

    // Update is called once per frame
    private void Update()
    {
        PerformSphereCast();

        switch(state)
        {
            case State.Patrol:
                MoveTowardsWaypoint();
                break;
            case State.Alert:
                Alert();
                break;
            case State.Chase:
                Chase();
                break;
        }

        HandleAnims();
        
        lastFramePosition = transform.position;  // Update the last frame position after all movement calculations
    }

    private void OnDrawGizmos()
    {
        // Visualize SphereCast/CircleCast
        Gizmos.color = Color.green; // Green color for SphereCast gizmo

        Vector3 castDirection;
        Vector3 sideDirection;
        castDirection = DetermineCastDirection(out sideDirection); 
        // castDirection = Vector3.up;

        Vector3 endPos = transform.position + (castDirection * (hitInfo.distance == 0 ? sphereCastDistance : hitInfo.distance));
        
        // Draw lines representing the direction and distance of the SphereCast
        Gizmos.DrawLine(transform.position, endPos);
        Gizmos.DrawLine(transform.position + (sideDirection * sphereRadius), endPos + (sideDirection * sphereRadius));
        Gizmos.DrawLine(transform.position - (sideDirection * sphereRadius), endPos - (sideDirection * sphereRadius));
        
        // Draw wire spheres at the start and end of the SphereCast to represent its radius
        Gizmos.DrawWireSphere(transform.position, sphereRadius);
        Gizmos.DrawWireSphere(endPos, sphereRadius);
    }


    private void PerformSphereCast()
    {
        Vector2 castDirection;
        Vector3 sideDirection; // We'll just discard this for now
        castDirection = DetermineCastDirection(out sideDirection); // Use the method to determine cast direction

        hitInfo = Physics2D.CircleCast(transform.position, sphereRadius, castDirection, sphereCastDistance, hitLayers);

        if (hitInfo.collider != null)
        {
            Debug.Log($"Hit: {hitInfo.collider.name}");

            // If the spherecast hits the player, the knight is in Patrol mode, and the player is not hiding
            if (hitInfo.collider.CompareTag("Player") && state == State.Patrol && !player.IsHiding())
            {
                state = State.Alert;
            }
        }
    }


    private Vector3 DetermineCastDirection(out Vector3 sideDirection)
    {
        Vector3 direction;
        if (castHorizontally)
        {
            direction = transform.localScale.x > 0 ? transform.right : -transform.right;
            sideDirection = transform.up; // Side lines for horizontal SphereCast
        }
        else
        {
            direction = (transform.position.y - lastFramePosition.y) > 0 ? Vector3.up : -Vector3.up;
            sideDirection = transform.right; // Side lines for vertical SphereCast
        }
        return direction;
    }



    private void MoveTowardsWaypoint()
    {
        if (currentWaypoint != null && !isWaiting) // Check if knight is not waiting
        {
            waypointFlatPosition = new Vector2(currentWaypoint.position.x, currentWaypoint.position.y);
            playerFlatPosition = new Vector2(transform.position.x, transform.position.y);

            // Determine the direction the sprite should face
            FlipSprite(waypointFlatPosition.x - playerFlatPosition.x);

            transform.position = Vector2.MoveTowards(transform.position, waypointFlatPosition, moveSpeed * Time.deltaTime);

            if (playerFlatPosition == waypointFlatPosition)
            {
                StartCoroutine(WaitAndProceed()); // Start waiting coroutine
            }
        }
    }

    private void Alert()
    {
        StartCoroutine(AlertCoroutine());
    }

    private IEnumerator AlertCoroutine()
    {
        isWaiting = true;
        yield return new WaitForSeconds(1); // Pause for 2 seconds
        AudioManager.instance.Play("Alert");
        isWaiting = false;

        if(patrolPoints.GetComponent<Collider2D>().bounds.Contains(playerTransform.position))
        {
            Debug.Log("Chase");
            state = State.Chase;
        }
        else
        {
            state = State.Patrol;
        }
    }

    private void Chase()
    {
        // Only chase the player if they are not hiding and are inside the PatrolPoints collider
        if (!player.IsHiding() && patrolPoints.GetComponent<Collider2D>().bounds.Contains(playerTransform.position))
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.position += (Vector3)directionToPlayer * moveSpeed * Time.deltaTime;

            FlipSprite(directionToPlayer.x);
        }
        else
        {
            state = State.Patrol; // If the player is hiding or out of bounds, revert back to patrol state
        }
    }
 

    private IEnumerator WaitAndProceed()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.5f);
        Transform nextWaypoint = patrolPoints.GetNextWaypoint(currentWaypoint);
        currentWaypoint = nextWaypoint;
        isWaiting = false;
    }

    private void FlipSprite(float direction)
    {
        if (direction > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0)
        {
            // Face left
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void HandleAnims()
    {
        anims.SetBool("isMoving", !isWaiting);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.CompareTag("Player"))
        {
            gameStats.playerStamina = 100;
            gameStats.playerHealth -= 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}

