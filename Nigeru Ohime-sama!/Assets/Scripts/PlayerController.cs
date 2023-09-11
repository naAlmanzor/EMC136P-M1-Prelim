using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Movement speed of the player
    private Vector2 moveDirection = Vector2.zero;

    private Rigidbody2D rb; // Reference to Rigidbody2D component
    private new CapsuleCollider2D collider;
    private Animator anims;

    private Tilemap hideables;

    private bool isHiding;
    private Vector2 lastPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponent<Animator>();
        hideables = GameObject.FindGameObjectWithTag("Interactable").GetComponent<Tilemap>();
        collider = GetComponent<CapsuleCollider2D>();

        List<TileBase> tiles = GetTilesFromTilemap();

        // foreach(TileBase tile in tiles)
        // {
        //     Debug.Log("Tile Name: " + tile.name);
        // }
    }

    private void Update()
    {
        HandleAnims();

        // Check for 'E' key press
        if (Input.GetKeyDown(KeyCode.E) && !isHiding)
        {
            rb.velocity = Vector2.zero;
            collider.isTrigger = true;
            isHiding = true;

            GetComponent<SpriteRenderer>().enabled = false;

            Vector3Int closestTileCellPosition = GetClosestInteractableTilePosition();
            Vector3 closestTileWorldPosition = hideables.GetCellCenterWorld(closestTileCellPosition);

            // Move player to that position
            // You might want to smoothly move the player, but for simplicity, we'll directly set the position here.
            lastPosition = transform.position;
            transform.position = closestTileWorldPosition;
        }
        else if(Input.GetKeyDown(KeyCode.E) && isHiding)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            collider.isTrigger = false;
            isHiding = false;
            transform.position = lastPosition;
        }

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector2.zero;
        }

        // Get the horizontal and vertical input (from keyboard or controller)
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        moveDirection = new Vector2(horizontalMovement, verticalMovement).normalized;
    }
    
    private void FixedUpdate()
    {
        // Move the player
        rb.velocity = moveDirection * moveSpeed;
        // rb.AddForce(moveDirection * moveSpeed);
    }

    private void HandleAnims()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool isRight = Input.GetKey(KeyCode.D);
        bool isLeft = Input.GetKey(KeyCode.A);
        bool isUp = Input.GetKey(KeyCode.W);
        bool isDown = Input.GetKey(KeyCode.S);

        anims.SetBool("isMoving", isMoving);
        anims.SetBool("isRight", isRight);
        anims.SetBool("isLeft", isLeft);
        anims.SetBool("isUp", isUp && !isRight && !isLeft);
        anims.SetBool("isDown", isDown && !isRight && !isLeft);
    }

    public List<TileBase> GetTilesFromTilemap()
    {
        List<TileBase> tiles = new();

        BoundsInt bounds = hideables.cellBounds;
        TileBase[] allTiles = hideables.GetTilesBlock(bounds);

        for(int x = 0; x < bounds.size.x; x++)
        {
            for(int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile != null)
                {
                    tiles.Add(tile);
                }
            }
        }

        return tiles;
    }

    private Vector3Int GetClosestInteractableTilePosition()
    {
        // Convert player's world position to cell position
        Vector3Int playerCellPosition = hideables.WorldToCell(transform.position);

        // Iterate through the surrounding tiles to find the closest interactable one.
        // (This logic can be extended if you want to check more than just the direct neighbors.)
        int checkRadius = 1;
        for (int x = -checkRadius; x <= checkRadius; x++)
        {
            for (int y = -checkRadius; y <= checkRadius; y++)
            {
                Vector3Int offset = new Vector3Int(x, y, 0);
                TileBase tile = hideables.GetTile(playerCellPosition + offset);
                if (tile != null)
                {
                    // Assuming all tiles in this Tilemap are interactable.
                    // If you have specific criteria, add checks here.
                    return playerCellPosition + offset;
                }
            }
        }

        // If no interactable tile is found nearby, return the player's position.
        return playerCellPosition;
    }

}
