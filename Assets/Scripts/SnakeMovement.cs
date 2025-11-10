using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [Header("Grid Settings")]
    public float moveStepTime = 0.2f; // Time between each tile move
    public Vector2Int gridSize = new Vector2Int(20, 20); // Optional bounds

    [Header("References")]
    public Transform segmentPrefab; // Assign a prefab for body segments
    private List<Transform> snakeSegments = new List<Transform>();

    private float moveTimer;
    private Vector2Int headGridPos;

    // The snake only moves if this has a value
    private Vector2Int? currentDirection = null;

    void Start()
    {
        snakeSegments.Add(this.transform); // Head
        headGridPos = Vector2Int.RoundToInt(transform.position);
    }

    void Update()
    {
        HandleInput();

        // Only move if a direction is pressed
        if (currentDirection.HasValue)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveStepTime)
            {
                moveTimer = 0f;
                MoveSnake();
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) && currentDirection != Vector2Int.down)
            currentDirection = Vector2Int.up;
        else if (Input.GetKey(KeyCode.S) && currentDirection != Vector2Int.up)
            currentDirection = Vector2Int.down;
        else if (Input.GetKey(KeyCode.A) && currentDirection != Vector2Int.right)
            currentDirection = Vector2Int.left;
        else if (Input.GetKey(KeyCode.D) && currentDirection != Vector2Int.left)
            currentDirection = Vector2Int.right;
    }

    void MoveSnake()
    {
        if (!currentDirection.HasValue) return;

        Vector3 prevPos = snakeSegments[0].position;
        headGridPos += currentDirection.Value; // Move head one tile
        snakeSegments[0].position = new Vector3(headGridPos.x, headGridPos.y, 0);

        // Update body segments
        for (int i = 1; i < snakeSegments.Count; i++)
        {
            Vector3 temp = snakeSegments[i].position;
            snakeSegments[i].position = prevPos;
            prevPos = temp;
        }
    }

    public void Grow()
    {
        Transform newSegment = Instantiate(segmentPrefab);
        newSegment.position = snakeSegments[snakeSegments.Count - 1].position; // Spawn at tail
        snakeSegments.Add(newSegment);
    }
}
