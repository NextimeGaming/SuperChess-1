using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes, rawImage;
    public AudioSource selectSound;
    private Animator animatorRawImage;
    public GameObject painelOpcoes;

    void Start()
    {
        // Verificação se rawImage não é nulo antes de ativar
        if (rawImage != null)
        {
            rawImage.SetActive(false);
            animatorRawImage = rawImage.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("RawImage is not assigned!");
        }

        // Verificação se selectSound 
        if (selectSound == null)
        {
            Debug.LogWarning("Select Sound is not assigned!");
        }

        // Inicialmente, painel de opções está desativado
        painelOpcoes.SetActive(false);
    }

    void Update()
    {
        // Verifica se o vídeo não está tocando e se alguma tecla foi pressionada
        if (videoPlayer != null && !videoPlayer.isPlaying && Input.anyKeyDown)
        {
            // Toca o som de seleção
            if (selectSound != null)
            {
                selectSound.Play();
            }
            videoPlayer.Play();
            // Ativa a animação e os objetos do menu
            if (animatorRawImage != null)
            {
                animatorRawImage.SetTrigger("fadeIn");
            }
            rawImage.SetActive(true);
            menuOpcoes.SetActive(true);
        }
    }

    public void AbrirOpcoes()
    {
        // Ativa o painel de opções
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        // Desativa o painel de opções
        painelOpcoes.SetActive(false);
    }

    public void IniciarJogo()
    {
        // Carrega a gameplay
        SceneManager.LoadScene("SampleScene");
    }
}

