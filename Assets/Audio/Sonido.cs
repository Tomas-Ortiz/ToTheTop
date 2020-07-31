using System;
using UnityEngine;

// clase serializable para que unity puede almacenar y recuperar la configuración para cada sonido
[Serializable]
public class Sonido
{
    public AudioClip audio;

    // variable publica (se usa en otra clase) pero se esconde en el inspector
    [HideInInspector]
    public AudioSource fuente;
    public string nombre;

    [Range(0f, 1f)]
    public float volumen;

    [Range(0f, 3f)]
    public float pitch;

    public bool enBucle;

    public bool reproducirAlPrincipio;

}
