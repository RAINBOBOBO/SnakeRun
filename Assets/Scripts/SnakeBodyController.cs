using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyController : MonoBehaviour
{
    private Vector2 upcomingPosition;
    
    public Rigidbody2D nextRigidBody;
    public Rigidbody2D thisRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        upcomingPosition = nextRigidBody.position;
        thisRigidBody.position = nextRigidBody.position + Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (upcomingPosition != nextRigidBody.position) FollowNext();

        upcomingPosition = nextRigidBody.position;
    }

    void FollowNext()
    {
        thisRigidBody.position = upcomingPosition;
    }
}
