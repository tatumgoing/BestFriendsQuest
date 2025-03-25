using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownAmbientSound
{
    [HideInInspector] public string name;
    public Sound sound;
    public Vector2 waitTimeRange = new Vector2(4, 8);
    public float distanceFromCamera = 2;
    [HideInInspector] public float cooldown;
    [HideInInspector] public Transform transform;

    public void Play()
    {
        transform.localPosition = Random.insideUnitSphere * distanceFromCamera;
        sound.Play(transform);
        cooldown = Random.Range(waitTimeRange.x, waitTimeRange.y);
    }
}

public class TownMusicPlayer : MonoBehaviour
{

    [Header("Track Management")]

    [SerializeField] List<Sound> allTracks = new List<Sound>();
    [SerializeField] Sound currentTrack;

    private void Start()
    {

        // instantiate all tracks

        for(int i = 0; i < allTracks.Count; i++)
        {
            allTracks[i] = Instantiate(allTracks[i]);   
        }
       
        // play all tracks silently

        for (int i = 0; i < allTracks.Count; i++)
        {
            allTracks[i].PlaySilent();
        }

    }

    void PlayNewTrack(Sound newTrack)
    {
        currentTrack.SetPercentVolume(0, 2f);
        newTrack.SetPercentVolume(100, 2f);

        currentTrack = newTrack;
    }
}
