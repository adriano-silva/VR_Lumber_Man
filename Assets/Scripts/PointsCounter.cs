using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    public GameObject scoreNumberText; // Text gameobject that displays the score in game
    public string sceneToRestart; // Scene to launch when restarting the game

    public int numberLimitPerfect; // Difference limit for the score calculation, less means Perfect
    public int numberLimitSuper; // Difference limit for the score calculation, less means Super
    public int pointsPerfect; // Number of points scored when having a Perfect cut
    public int pointsSuper; // Number of points scored when having a Super cut
    public int pointsOk; // Number of points scored when having a OK cut

    public AudioClip soundBig; // Soundclip for big cuts
    public AudioClip soundMedium; // Soundclip for medium cuts
    public AudioClip soundSmall; // Soundclip for small cuts

    private const int MULTIPLICATOR = 1000; // Multiplicator constant for the volume calculcation
    private int score;
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = scoreNumberText.GetComponent<TextMesh>();
        score = 0;
    }

    /// <summary>
    /// Calculates and updates the score according to the difference of volumes between the two given Hulls.
    /// The smaller volume difference between the two Hulls, the more points the player scores.
    /// </summary>
    /// <param name="hull">The full hull before being cut</param>
    /// <param name="lowerHull">The lower hull result of the cut</param>
    /// <param name="upperHull">The upper hull result of the cut</param>
    public void CalculateScore(GameObject hull, GameObject lowerHull, GameObject upperHull)
    {

        Mesh lowerMesh = lowerHull.GetComponent<MeshFilter>().sharedMesh;
        Mesh upperMesh = upperHull.GetComponent<MeshFilter>().sharedMesh;

        AudioSource audio = upperHull.GetComponent<AudioSource>();
        audio.enabled = true;
        audio.spatialize = true;

        Vector3 lowerSize = lowerMesh.bounds.size;
        Vector3 upperSize = upperMesh.bounds.size;

        // Calculating the difference of volumes between the two hulls
        float difference = Mathf.Abs(upperSize.magnitude - lowerSize.magnitude) * MULTIPLICATOR;

        // Defining the number of points scored
        if (difference <= numberLimitPerfect)
        {
            score += pointsPerfect;
            audio.clip = soundBig;
        }else if (difference <= numberLimitSuper && difference > numberLimitPerfect)
        {
            score += pointsSuper;
            audio.clip = soundMedium;
        }else
        {
            score += pointsOk;
            audio.clip = soundSmall;
        }
        if (!audio.isPlaying)
        {
            audio.Play();
        }

        Debug.Log("Difference: " + difference);
        Debug.Log("Score: " + score);

        // Updating the in game score board
        textMesh.text = score.ToString();
    }


}
