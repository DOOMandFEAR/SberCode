using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip[] audios;
    public Vector3 soundPosition;

    public float minRange = 0;
    public float maxRange = 10;

    private float untilNext = 0;

    void Start()
    {
        untilNext = Random.Range(minRange, maxRange);
    }

    void Update()
    {
        untilNext -= Time.deltaTime;

        if (untilNext <= 0)
        {
            AudioClip audio = audios[Random.Range(0, audios.Length)];

            untilNext = audio.length + Random.Range(minRange, maxRange);

            AudioSource.PlayClipAtPoint(audio, soundPosition, 0.5f);
        }
    }
}
