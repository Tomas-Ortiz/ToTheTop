using System;
using UnityEngine;

public class ControladorAudio : MonoBehaviour
{
    // la cantidad de sonidos se especifica en el inspector
    public Sonido[] sonidos;

    void Awake()
    {
        configuracionAudioSources();
    }

    void Start()
    {
        reproducirAudiosAutomaticos();
    }

    // se busca un nombre por audio y se lo reproduce
    public void reproducirAudio(string nombreAudio)
    {
        // se busca en el array de sonidos aquel sonido que tenga como nombre el pasado como argumento
        Sonido audio = Array.Find(sonidos, sonido => sonido.nombre == nombreAudio);

        // si el audio existe y se quiere reproducir el disparo del personaje o spitter
        // se reproduce aunque se esté reproduciendo ya el mismo audio de disparo
        if (audio != null && (audio.nombre.Equals("disparoPersonaje") || audio.nombre.Equals("ataqueSpitter")))
        {
            audio.fuente.Play();
        }
        // si el audio existe y no se está reproduciendo ningun audio
        else if (audio != null && !audio.fuente.isPlaying)
        {
            audio.fuente.Play();
        }
    }

    private void configuracionAudioSources()
    {
        // al principio, se recorre cada uno de los sonidos creados
        foreach (Sonido s in sonidos)
        {
            // por cada sonido se agrega un componente audioSource

            s.fuente = gameObject.AddComponent<AudioSource>();

            // el audio, volumen y pitch de cada sonido en el inspector se lo iguala al audioSource de cada audio

            s.fuente.clip = s.audio;
            s.fuente.volume = s.volumen;
            s.fuente.pitch = s.pitch;

            s.fuente.playOnAwake = s.reproducirAlPrincipio;
            s.fuente.loop = s.enBucle;
        }
    }

    private void reproducirAudiosAutomaticos()
    {
        Sonido musicaFondo = Array.Find(sonidos, sonido => sonido.nombre == "musicaFondo");
        Sonido musicaAmbiente = Array.Find(sonidos, sonido => sonido.nombre == "musicaAmbiente");

        // si existe el sonido musicaFondo y musicaAmbiente entonces se los reproduce automaticamente
        if (musicaFondo != null && musicaAmbiente != null)
        {
            // audios que empiezan a reproducirse desde el inicio
            reproducirAudio("musicaFondo");
            reproducirAudio("musicaAmbiente");
        }
    }
}
