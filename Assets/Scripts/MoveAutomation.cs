using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAutomation : MonoBehaviour
{

    public Transform [] goals; // Array of positions
    public int waitingTimeInSeconds; // Time to wait before moving to next goal

    private NavMeshAgent agent;
    private Animator anim;
    private float timer;
    private int index; // Index for the array
    private bool moving; // Bool indicating if the gameObject is moving or not. Used for animation

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        index = 0;
    }

    private void Update()
    {
        if (!agent.hasPath)
        {
            anim.SetBool("moving", false);
        }
        if (timer >= waitingTimeInSeconds)
        {
            agent.SetDestination(goals[index].position);
            anim.SetBool("moving", true);
            Debug.Log("moving");
            if (index == goals.Length-1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}
