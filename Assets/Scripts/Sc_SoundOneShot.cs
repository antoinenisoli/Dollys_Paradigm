using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_SoundOneShot : MonoBehaviour
{
    AudioSource sound => GetComponent<AudioSource>();

    private void Awake()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(sound.clip.length);
        Destroy(gameObject);
    }
}
