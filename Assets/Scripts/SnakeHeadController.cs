using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHeadController : MonoBehaviour
{
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private GameState gameState;

    public Rigidbody2D headRigidBody;
    public GameObject headGameObject;
    public float nextMove = 0.5f;
    public float velocity = 1.0f;
    public float moveInterval = 0.25f;
    public GameObject gameStateObject;
    public enum Direction
    {
        Up, Down, Left, Right
    }
    public Direction headDirection;

    public void ResetHead()
    {
        headGameObject.GetComponent<SpriteRenderer>().enabled = true;
        nextMove = Time.time;
        headRigidBody.position = Vector2.zero;
        headDirection = Direction.Up;
    }

    public void DeactivateHead()
    {
        headGameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Start()
    {
        gameState = gameStateObject.GetComponent<GameState>();
    }

    void Update()
    {
        if (gameState.currentState != GameState.State.Endless) return;
        
        if (Time.time >= nextMove)
        {
            nextMove = nextMove + moveInterval;
            MoveHead();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touchPosition;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                touchEnd = touchPosition;
                ChangeDirection();
            }
        }
    }

    void MoveHead()
    {
        switch (headDirection)
        {
            case Direction.Up:
                headRigidBody.position += Vector2.up * velocity;
                break;
            case Direction.Down:
                headRigidBody.position += Vector2.down * velocity;
                break;
            case Direction.Left:
                headRigidBody.position += Vector2.left * velocity;
                break;
            case Direction.Right:
                headRigidBody.position += Vector2.right * velocity;
                break;
        }
    }

    void ChangeDirection()
    {
        Vector2 delta = touchEnd - touchStart;
        float largerDeltaMagnitude = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        float largerDelta;
        if (largerDeltaMagnitude == Mathf.Abs(delta.x))
        {
            largerDelta = delta.x;
        }
        else
        {
            largerDelta = delta.y;
        }
        // Debug.Log($"touchEnd: {touchEnd}, touchStart: {touchStart}, delta: {delta} largerDelta: {largerDelta}");

        bool ValidDirectionChange(Direction currentDirection, Direction potentialDirection)
        {
            if (currentDirection == Direction.Up && potentialDirection == Direction.Down) return false;
            if (currentDirection == Direction.Down && potentialDirection == Direction.Up) return false;
            if (currentDirection == Direction.Left && potentialDirection == Direction.Right) return false;
            if (currentDirection == Direction.Right && potentialDirection == Direction.Left) return false;
            return true;
        }
        
        void ChangeXDirection()
        {
            // Move left or right
            Direction potentialDirection;
            if (Mathf.Sign(largerDelta) == 1)
            {
                potentialDirection = Direction.Right;
            }
            else
            {
                potentialDirection = Direction.Left;
            }

            if (ValidDirectionChange(headDirection, potentialDirection))
            {
                headDirection = potentialDirection;
            }
        }

        void ChangeYDirection()
        {
            // Move up or down
            Direction potentialDirection;
            if (Mathf.Sign(largerDelta) == 1)
            {
                potentialDirection = Direction.Up;
            }
            else
            {
                potentialDirection = Direction.Down;
            }

            if (ValidDirectionChange(headDirection, potentialDirection))
            {
                headDirection = potentialDirection;
            }
        }
        
        if (largerDelta == delta.x)
        {
            ChangeXDirection();
        }
        else
        {
            ChangeYDirection();
        }
    }
}
