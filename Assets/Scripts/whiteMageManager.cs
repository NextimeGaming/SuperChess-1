using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class whiteMageManager : MonoBehaviour
{

    private static whiteMageManager selectedMage = null;

    private bool mouseOver = false;
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;

    public Vector2Int[] asd;
    public WhitePawnManager peaoBranco;
    public whiteArcherManager arqueiroBranco;
    public TabuleiroDamas tabuleiro;
    public AttackCircleManager attackCircleManager;

    private List<GameObject> casasDisponiveis = new List<GameObject>();
    private Dictionary<GameObject, Color> casaCoresOriginais = new Dictionary<GameObject, Color>();

    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private Vector2Int posAtual;

    public int vidaAtual, vidaMaxima, movimento, ataqueOriginal, ataqueAtual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaMaxima = 6;
        vidaAtual = vidaMaxima;
        movimento = 1;
        ataqueOriginal = 5;
        ataqueAtual = ataqueOriginal;

        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        tabuleiro = FindObjectOfType<TabuleiroDamas>();
        peaoBranco = FindObjectOfType<WhitePawnManager>();        
        foreach (var casa in GameObject.FindGameObjectsWithTag("Casa"))
        {
            casaCoresOriginais[casa] = casa.GetComponent<Renderer>().material.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
                isMoving = false;
                DeselectMage();
            }
        }
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (selectedMage != this)
        {
            rend.material.color = startColor;
        }
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        if (mouseOver)
        {
            if (selectedMage == this)
            {
                DeselectMage();
            }
            else
            {
                SelectMage();
            }
        }
    }

    private void SelectMage()
    {
        if (isMoving || selectedMage != null)
            return;
        selectedMage = this;
        rend.material.color = hoverColor;
        tentativa();
        MostrarAreaAtaque();
        StartCoroutine(WaitForClick());
    }

    public void DeselectMage()
    {
        selectedMage = null;
        rend.material.color = startColor;
        RestaurarCasasDisponiveis();
        casasDisponiveis.Clear();
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("circuloAtk"))
        {
            Destroy(c);
        }
    }

    private IEnumerator WaitForClick()
    {
        while (selectedMage == this)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("Casa"))
                    {
                        Vector3 targetPos = hit.point;
                        targetPos = new Vector3((int)targetPos.x, 1.1f, (int)targetPos.z);

                        if (IsPositionValid(targetPos))
                        {
                            tabuleiro.desocupaCasa((int)transform.position.x, (int)transform.position.z);
                            targetPosition = targetPos;
                            tabuleiro.ocupaCasa((int)targetPosition.x, (int)targetPosition.z);
                            isMoving = true;
                            DeselectMage();
                            yield break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    public Array MostrarAreaAtaque()
    {
        Vector2Int[] direcoes = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(2, 0),
            new Vector2Int(3, 0),
            
            new Vector2Int(0, 1),
            new Vector2Int(0, 2),
            new Vector2Int(0, 3),
            
            new Vector2Int(0, -1),
            new Vector2Int(0, -2),
            new Vector2Int(0, -3),
            
            new Vector2Int(-1, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(-3, 0),
        };

        foreach (var dir in direcoes)
        {
            Vector2Int novaPos = new Vector2Int(posAtual.x + dir.x, posAtual.y + dir.y);
            MostrarCírculoSeValido(novaPos);
        }
        return direcoes;
    }

    private void MostrarCírculoSeValido(Vector2Int novaPos)
    {
        if (novaPos.x >= 0 && novaPos.x < tabuleiro._casaOcupada.GetLength(0) &&
            novaPos.y >= 0 && novaPos.y < tabuleiro._casaOcupada.GetLength(1))
        {
            Vector3 posicaoCírculo = new Vector3(novaPos.x, 1f, novaPos.y);
            attackCircleManager.ShowAttackCircle(posicaoCírculo); // Exibe a merda do circulo
        }
    }

    private bool IsPositionValid(Vector3 targetPos)
    {
        return tabuleiro.IsPositionEmpty(targetPos) && IsAdjacent(targetPos) && IsDirectionFree(posAtual, targetPos);
    }

    private bool IsAdjacent(Vector3 targetPos)
    {
        Vector3 currentPos = transform.position;
        float distance = Vector3.Distance(new Vector3(currentPos.x, 0, currentPos.z), new Vector3(targetPos.x, 0, targetPos.z));
        return distance <= 1.0f; // Permite movimento nas diagonais
    }

    private bool IsDirectionFree(Vector2Int currentPos, Vector3 targetPos)
    {
        int deltaX = (int)targetPos.x - currentPos.x;
        int deltaY = (int)targetPos.z - currentPos.y;

        // Verifica se está movendo na horizontal
        if (deltaY == 0)
        {
            if (deltaX > 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x + 1, 0, currentPos.y));
            }
            else if (deltaX < 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x - 1, 0, currentPos.y));
            }
        }
        // Verifica se está movendo na vertical
        else if (deltaX == 0)
        {
            if (deltaY > 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x, 0, currentPos.y + 1));
            }
            else if (deltaY < 0)
            {
                return tabuleiro.IsPositionEmpty(new Vector3(currentPos.x, 0, currentPos.y - 1));
            }
        }
        // Para movimentos diagonais 
        return true;
    }

    public void tentativa()
    {
        casasDisponiveis.Clear();
        Vector3 currentPos = transform.position;
        posAtual = new Vector2Int((int)currentPos.x, (int)currentPos.z);

        if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z) == false) //checa a casa da frente
            AdicionarCasaSeVazia((int)currentPos.x + 1, (int)currentPos.z);        

        if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z) == false) //checa a casa de trás
            AdicionarCasaSeVazia((int)currentPos.x - 1, (int)currentPos.z);
        

        if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z + 1) == false) //checa casa da esquerda
            AdicionarCasaSeVazia((int)currentPos.x, (int)currentPos.z + 1);

        if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z - 1) == false) //checa casa da direita
            AdicionarCasaSeVazia((int)currentPos.x, (int)currentPos.z - 1);
    }

    private void AdicionarCasaSeVazia(int x, int y)
    {
        if (x >= 0 && x < tabuleiro._casaOcupada.GetLength(0) && y >= 0 && y < tabuleiro._casaOcupada.GetLength(1))
        {
            if (tabuleiro.IsPositionEmpty(new Vector3(x, 0, y)))
            {
                GameObject casa = GameObject.Find($"Casa {x},{y}");
                if (casa != null)
                {
                    casa.GetComponent<Renderer>().material.color = Color.yellow; // Muda a cor da casa
                    casasDisponiveis.Add(casa);
                }
            }
        }
    }

    private void RestaurarCasasDisponiveis()
    {
        foreach (var casa in casasDisponiveis)
        {
            if (casaCoresOriginais.TryGetValue(casa, out Color originalColor))
            {
                casa.GetComponent<Renderer>().material.color = originalColor; // Restaura a cor original
            }
        }
    }

}
