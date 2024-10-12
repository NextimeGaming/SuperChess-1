using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes, rawImage; 
    public AudioSource selectSound;
    private Animator animatorRawImage;

    
    void Start()
    {
        // Verifica��o se rawImage n�o � nulo antes de ativar
        if (rawImage != null)
        {
            rawImage.SetActive(false); 
            animatorRawImage = rawImage.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("RawImage is not assigned!");
        }

        // Verifica��o se selectSound n�o � nulo
        if (selectSound == null)
        {
            Debug.LogWarning("Select Sound is not assigned!");
        }
    }

    
    void Update()
    {
        // Verifica se o v�deo n�o est� tocando e se alguma tecla foi pressionada
        if (videoPlayer != null && !videoPlayer.isPlaying && Input.anyKeyDown)
        {
            // Toca o som de sele��o, se n�o for nulo
            if (selectSound != null)
            {
                selectSound.Play();
            }

            
            videoPlayer.Play();

            // Ativa a anima��o e os objetos do menu
            if (animatorRawImage != null)
            {
                animatorRawImage.SetTrigger("fadeIn");
            }
            rawImage.SetActive(true); 
            menuOpcoes.SetActive(true); 
        }
    }
}
