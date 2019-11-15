using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class Slicer : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public ISteamVR_Action_Vibration vibration;

    public Material innerWood;
    public GameObject gameEngine;
    public string toolTag = "Tool";
    public string playerTag = "Player";
    public string cuttableTag = "Cuttable";

    private PointsCounter pointsCounter;

    private void Start()
    {
        pointsCounter = gameEngine.GetComponent<PointsCounter>();
    }

    /// <summary>
    /// On trigger enter, checks if the collisions is with a "cuttable" item or a "Board".
    /// If it is a "cuttable" item, calls SliceObject function and cuts the game object.
    /// If it is a "Board" item, restarts the game.
    /// </summary>
    /// <param name="collision">Collision on the gameObject</param>
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.tag == "Cuttable")
        {
            SliceObject(collision);
        }
        else if (collision.gameObject.tag == "Board")
        {
            SliceObject(collision);
            SteamVR_LoadLevel.Begin(gameEngine.GetComponent<PointsCounter>().sceneToRestart);
            DestroyWithTag(toolTag);
            DestroyWithTag(cuttableTag);
            DestroyWithTag(playerTag);
        }
    }

    /// <summary>
    /// Adds MeshCollider, Rigidbody, Throwable, Interactable and AudioSource components on the gameObject.
    /// Also sets the tag "Tool".
    /// </summary>
    /// <param name="hull">GameObject that will get the components</param>
    private void AddComponentsOnHull(GameObject hull)
    {
        hull.AddComponent<MeshCollider>().convex = true;
        hull.AddComponent<Rigidbody>().useGravity = true;
        hull.AddComponent<Throwable>();
        hull.AddComponent<Interactable>();
        hull.AddComponent<AudioSource>();
        hull.tag = toolTag;
    }

    /// <summary>
    /// Slices a gameObject using the Slice library, by disabling current object, and creating two
    /// different gameObjects resulted from the cut
    /// </summary>
    /// <param name="collision">Collision on the gameObject. This will be the first point of the cut</param>
    private void SliceObject(Collider collision)
    {
    SlicedHull hull = collision.gameObject.Slice(transform.position, transform.up, new TextureRegion(0.0f, 0.7f, 0.1f, 0.8f),innerWood);
    Debug.Log(hull);
    if (hull != null)
    {
        GameObject lowerHull = hull.CreateLowerHull(collision.gameObject, null);
        GameObject upperHull = hull.CreateUpperHull(collision.gameObject, null);

            Hand hand = this.GetComponentInParent<Hand>();
            if (hand != null)
            {
                hand.hapticAction.Execute(0, 0.1f, 100, 1, hand.handType);
            }

            AddComponentsOnHull(lowerHull);
            AddComponentsOnHull(upperHull);
 
        if(collision.gameObject.tag == "Cuttable")
            {
                pointsCounter.CalculateScore(collision.gameObject, lowerHull, upperHull);
            }
            collision.gameObject.SetActive(false);

        }
    }

    /// <summary>
    /// Destroys all the objects found with the given tag
    /// </summary>
    /// <param name="tag">Tag for which gameobjects will be destroyed</param>
    private void DestroyWithTag(string tag)
    {
        GameObject[] itemsToDestroy = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject item in itemsToDestroy)
        {
            Destroy(item);
        }
    }
}
