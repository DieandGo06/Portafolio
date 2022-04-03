using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEnArea : MonoBehaviour
{
    [Header("Propiedades")]
    [Range(-1, 1)] public int direccion;
    [Range(0, 360)] public int rotacionInicial;
    [Range(0, 360)] public float velocidadAngulos;
    [Range(0.1f, 1f)] public float duracionFades;


    [Header("Logica")]
    [Range(0f, 5f)] public float previewTime;
    public bool canDamage;

    [Header("Privadas")]
    Animator animator;
    Rigidbody2D rBody;
    SpriteRenderer render;
    float opacidad;
    float anguloActual;
    float tiempoTranscurrido;




    private void Awake()
    {
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (animator.GetBool("isPreview")) {
            tiempoTranscurrido += Time.deltaTime;
            if (tiempoTranscurrido >= previewTime) {
                animator.SetBool("isActivo", true);
            }
        }

        if (animator.GetBool("isActivo")) {
            animator.SetBool("isFadeIn", false);
            tiempoTranscurrido += Time.fixedDeltaTime;
            canDamage = true;
        }

        if (animator.GetBool("isFadeOut")) {
            animator.SetBool("isActivo", false);
            tiempoTranscurrido = 0;
            canDamage = false;
        }
    }


    private void FixedUpdate()
    {
        if (animator.GetBool("isActivo")) Girar();
    }




    void Girar()
    {
        anguloActual = MetodosDeExtension.MoverAngulo(anguloActual, velocidadAngulos, direccion);
        rBody.MoveRotation(anguloActual);//MoveRotation funcina en angulos
    }


    //Se ejecuta en la linea de tiempo del animation
    public void ActivarDamage()
    {
        animator.SetBool("isActivo", true);
    }

    void FadeIn()
    {
        opacidad += Time.deltaTime / duracionFades;
    }

    void SetOpacidad(float _opacidad)
    {
        Color _color = render.material.color;
        render.material.color = new Color(_color.r, _color.g, _color.b, _opacidad);
    }

}
