using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[System.Serializable]
//public class TownAmbientSound
//{
//    [HideInInspector] public string name;
//    public Sound sound;
//    public Vector2 waitTimeRange = new Vector2(4, 8);
//    public float distanceFromCamera = 2;
//    [HideInInspector] public float cooldown;
//    [HideInInspector] public Transform transform;

//    public void Play()
//    {
//        transform.localPosition = Random.insideUnitSphere * distanceFromCamera;
//        sound.Play(transform);
//        cooldown = Random.Range(waitTimeRange.x, waitTimeRange.y);
//    }
//}

public class TownMusicPlayer : MonoBehaviour
{
    public static TownMusicPlayer i;

    [Header("Track Management")]

    [SerializeField] List<Sound> allTracks = new List<Sound>();
    [SerializeField] Sound currentTrack;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {

        // instantiate all tracks

        for(int i = 0; i < allTracks.Count; i++)
        {
            allTracks[i] = Instantiate(allTracks[i]);   
        }
       
        // play all tracks silently

       /* for (int i = 0; i < allTracks.Count; i++)
        {
            allTracks[i].PlaySilent();
        }*/

    }

    public void PlayNewTrack(Sound newTrack)
    {
        bool changed = false;

        foreach (Sound track in allTracks) {
            if (track.TrackName == newTrack.TrackName) 
            {
                //turn down current track if there is one, turn up the new track
                if (currentTrack != null) {

                    StartCoroutine(FadeTrackOut(currentTrack));

                    //currentTrack.Stop();

                }


                track.PlaySilent();

                StartCoroutine(FadeTrackIn(track)); 

                //track.Play();

                currentTrack = track;

                changed = true;
            }
        }
        if(!changed) 
        {
            Debug.LogWarning("Track not found in AllTracks!");
        }
        
    }

    private IEnumerator FadeTrackIn(Sound sound)
    {
        float vol = sound.percentVolume;

        while(vol < 1)
        {
            vol += .01f;

            yield return new WaitForSeconds(.01f);
            sound.SetPercentVolume(vol);
        }
        
    }

    private IEnumerator FadeTrackOut(Sound sound)
    {

        float vol = sound.percentVolume;

        while (vol > 0)
        {
            Debug.Log(vol);

            vol -= .01f;

            yield return new WaitForSeconds(.01f);
            sound.SetPercentVolume(vol);
        }

        sound.Stop();
    }
    public void StopCurrentTrack()
    {
        currentTrack.Stop();
    }

}
