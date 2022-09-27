using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMovement : MonoBehaviour
{
    public enum MovementType
    {
        linear
    }
    public List<GameObject> movementPoints = new List<GameObject>();
    int movementIndex = 0;
    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position == movementPoints[movementIndex].transform.position)
        {
            movementIndex = (movementIndex+1)%movementPoints.Count;
        }
        gameObject.GetComponent<Rigidbody2D>().MovePosition(Vector2.MoveTowards(transform.position, movementPoints[movementIndex].transform.position, movementSpeed*Time.fixedDeltaTime));
    }
}
