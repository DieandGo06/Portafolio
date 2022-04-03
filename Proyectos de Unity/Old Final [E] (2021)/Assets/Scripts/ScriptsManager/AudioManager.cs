using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip disparoBala, impactoEnemigo, hitNave;
    public AudioClip[] musica = new AudioClip[9];
    AudioSource audioSource;

    bool isMenuMusic;
    bool isGameMusic;



    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!isMenuMusic)
            {
                isMenuMusic = true;
                StartCoroutine(PlayMenuMusic(musica[0], musica[1]));
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 2) { 
            if (!isGameMusic)
            {
                audioSource.Stop();
                isGameMusic = true;
                StartCoroutine(PlayGameMusic(musica[2], musica[3], musica[4], musica[5], musica[6], musica[7]));
            }
        }
    }

    private void LateUpdate()
    {
        //Debug.Log(GetComponent<AudioSource>().clip);
        //Debug.Log(GetComponent<AudioSource>().clip.length);
    }




    IEnumerator PlayMenuMusic(AudioClip clip, AudioClip transicion)
    {
        audioSource.volume = 0.6f;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);

        audioSource.loop = true;
        audioSource.clip = transicion;
        audioSource.Play();
    }


    IEnumerator PlayGameMusic(AudioClip transicion_1, AudioClip clip_1, AudioClip transicion_2, AudioClip clip_2, AudioClip transicion_3, AudioClip clip_3)
    {
        audioSource.volume = 0f;
        if (GameManager.fase == 1)
        {
            PlayTransicion(transicion_1);
            yield return new WaitForSeconds(transicion_1.length);

            PlayClip(clip_1);
            yield return new WaitUntil(() => GameManager.fase == 2);
            audioSource.loop = false;
            yield return new WaitUntil(() => !audioSource.isPlaying);

            PlayTransicion(transicion_2);
            yield return new WaitForSeconds(transicion_2.length);
        }

        yield return new WaitUntil(() => !audioSource.isPlaying);
        print("audio stop");

        if (GameManager.fase == 2)
        {
            PlayClip(clip_2);
            yield return new WaitForSeconds(clip_2.length);

            PlayTransicion(transicion_3);
            yield return new WaitForSeconds(transicion_3.length);

            PlayClip(clip_3);
        }
    }

    void PlayTransicion(AudioClip _transicion)
    {
        audioSource.loop = false;
        audioSource.clip = _transicion;
        audioSource.Play();
    }
    void PlayClip(AudioClip _clip)
    {
        audioSource.loop = true;
        audioSource.clip = _clip;
        audioSource.Play();
    }




    public static void PlaySound(AudioClip clip)
    {
        GameObject sound = new GameObject("sonido");
        AudioSource audioSource = sound.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
        audioSource.volume = 0.2f;
        Destroy(sound, 1f);
    }

    public static void PlayExplosion(AudioClip clip)
    {
        GameObject sound = new GameObject("sonido");
        AudioSource audioSource = sound.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
        audioSource.volume = 0.3f;
        Destroy(sound, 3f);
    }
}