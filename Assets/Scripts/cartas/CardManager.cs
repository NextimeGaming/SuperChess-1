using UnityEngine;

public enum TipoCarta
{
    transformacao,
    HabilidadeEspecial,
    magia
}

public class Carta : MonoBehaviour
{
    public string nome; // Nome da carta
    public TipoCarta tipo; // Tipo de carta (Magia, Habilidade, transforma�ao )
    public string descricao; // (explica��o do efeito)
    public Sprite imagem; // Imagem da carta,

    // Efeitos espec�ficos 
    public void UsarCarta(TabuleiroDamas tabuleiro, PieceManager pieceManager)
    {
        switch (tipo)
        {
            case TipoCarta.transformacao:
                AplicarMelhoria(tabuleiro, pieceManager);
                break;
            case TipoCarta.HabilidadeEspecial:
                AplicarHabilidadeEspecial(tabuleiro, pieceManager);
                break;
            case TipoCarta.magia:
                AplicarEvento(tabuleiro, pieceManager);
                break;
        }
    }

    // Fun��o para aplicar melhorias nas pe�as 
    private void AplicarMelhoria(TabuleiroDamas tabuleiro, PieceManager pieceManager)
    {
        // Implementar o efeito de melhoria 
        Debug.Log($"Melhoria aplicada: {nome}");
    }

    // Fun��o para habilidades especiais 
    private void AplicarHabilidadeEspecial(TabuleiroDamas tabuleiro, PieceManager pieceManager)
    {
        // Exemplo: invocar um arqueiro ou mago
        Debug.Log($"Habilidade especial usada: {nome}");
    }

    // Fun��o para eventos no tabuleiro ( criar buracos, alterar tabuleiro)
    private void AplicarEvento(TabuleiroDamas tabuleiro, PieceManager pieceManager)
    {
        // Exemplo: criar um buraco ou eleva��o no tabuleiro
        Debug.Log($"Evento aplicado: {nome}");
    }
}
