using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class villagerwalk : MonoBehaviour
{
    public GameObject npc;
    public float moveSpeed = 2f;


    public enum State { Loop,PingPong, OneWay}
    public State state = State.OneWay;

    public enum Direction { Forwards,Backwards};
    public Direction direction = Direction.Forwards;

    public bool spawnAtStart = false; //if true, when level loads then spawns at first transform

    public Transform[] paths;
   

    public bool ReachedEnd { get; private set; }
    public bool ReachedStart { get; private set; }

    public int currentIndex = 0;
    public bool runOnce;
    public bool hasRotated;

    void Start()
    {
        transform.position = paths[currentIndex].transform.position;
        if (spawnAtStart)
        {
            //transform.position = GetCurrentTransform().position;
        }
    }
    private void Update()
    {
        Move();
        //GetCurrentTransform();
        //CheckPath(transform);
    }

    private void Move()
    {
        if(currentIndex <= paths.Length -1 && ReachedEnd == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, paths[currentIndex].transform.position, moveSpeed * Time.deltaTime);
        }
        if(transform.position == paths[0].transform.position && runOnce == true && hasRotated ==false)
        {
            transform.Rotate(0, 180, 0);
            hasRotated = true;
        }
       if (transform.position == paths[currentIndex].transform.position && ReachedEnd == false)
        {
            currentIndex += 1;
            hasRotated = false;
            runOnce = true;
        }
       if(currentIndex == paths.Length)
        {
            ReachedEnd = true;
            //idle animation switch here
            //wait for 2 seconds
            transform.Rotate(0, 180, 0);
            //swich animation back
            currentIndex -= 1;
        }
        if (ReachedEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, paths[currentIndex].transform.position, moveSpeed * Time.deltaTime);
        }
        if(ReachedEnd && transform.position == paths[currentIndex].transform.position)
        {
            currentIndex -= 1;
        }
        if (currentIndex == 0)
        {
            ReachedEnd = false;
        }
    }
    
    
    
    
    //public Transform GetCurrentTransform()
    //{
    //    return paths[currentIndex];
    //}

    //public void CheckPath(Transform other)
    //{
    //    Transform currentTransform = GetCurrentTransform();
    //    int range = (int)Vector2.Distance(currentTransform.position, other.position);
    //    if (range == 0)
    //    {
    //        NextPath();
    //    }
    //}
    //private void NextPath()
    //{
    //    int nextIndex = direction == Direction.Forwards ? currentIndex + 1 : currentIndex - 1;
    //    ReachedEnd = nextIndex >= paths.Length;
    //    ReachedStart = nextIndex == 0;
    //    if (state == State.PingPong && (ReachedEnd || ReachedStart))
    //    {
    //        //reverse direction
    //        switch (direction)
    //        {
    //            case Direction.Forwards:
    //                direction = Direction.Backwards;
    //                nextIndex--;
    //                break;

    //            case Direction.Backwards:
    //                direction = Direction.Forwards;
    //                nextIndex++;
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    else if (state == State.Loop && ReachedEnd)
    //    {
    //        nextIndex = 0;
    //    }
    //    else if (state == State.Loop && ReachedStart)
    //    {
    //        nextIndex = paths.Length - 1;
    //    }
    //    else if (ReachedStart)
    //    {
    //        nextIndex = 0;
    //    }
    //    else if (ReachedEnd)
    //    {
    //        nextIndex = paths.Length - 1;
    //    }
    //    currentIndex = nextIndex;
    //}
}