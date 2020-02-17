using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public Vector2 Target;
    public float Speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        Target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget() 
    {
        transform.position = Vector2.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
    }
}
