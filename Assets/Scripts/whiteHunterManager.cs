using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteHunterManager : MonoBehaviour
{
    private static whiteHunterManager selectedHunter = null;

    //Stats
    public int vidaMax = 7, vidaAtual = 7, ataque = 3, ataqueAtual = 3;
    public float moveSpeed = 2f;

    //Controle de cor
    private bool mouseOver = false;
    public Color hoverColor;
    private Renderer rend;
    private Color startColor;

    //Listas
    private List<GameObject> casasDisponiveis = new List<GameObject>();
    private Dictionary<GameObject, Color> casaCoresOriginais = new Dictionary<GameObject, Color>();

    //Controle de classes
    public WhitePawnManager peaoBranco;
    public TabuleiroDamas tabuleiro;
    public AttackCircleManager attackCircleManager;
    public tileManager tileManager;

    //Controle de posição
    private Vector2Int posAtual;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        //Inicialização das cores
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        //Inicialização das classes
        tabuleiro = FindObjectOfType<TabuleiroDamas>();
        peaoBranco = FindObjectOfType<WhitePawnManager>();
        attackCircleManager = FindObjectOfType<AttackCircleManager>();

        //Define as cores de cada casa
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
                deSelecionar();
            }
        }
    }

    private IEnumerator WaitForClick()
    {
        while (selectedHunter == this)
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
                        targetPos = new Vector3(Mathf.Round(targetPos.x), 1.1f, Mathf.Round(targetPos.z));
                        tabuleiro.desocupaCasa((int)transform.position.x, (int)transform.position.z);
                        targetPosition = targetPos;
                        tabuleiro.ocupaCasa((int)targetPosition.x, (int)targetPosition.z);
                        isMoving = true;
                        deSelecionar();
                        yield break;
                    }
                }
            }
            yield return null;
        }
    }

    private void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        if (selectedHunter != this)
        {
            rend.material.color = startColor;
        }
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        if (mouseOver)
        {
            if (selectedHunter == this)
            {
                deSelecionar();
            }
            else
            {
                selecionar();
            }
        }
    }

    void mostrarMovimento()
    {
        casasDisponiveis.Clear();
        Vector3 currentPos = transform.position;
        posAtual = new Vector2Int((int)currentPos.x, (int)currentPos.z);
        if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z) == false) //checa a casa da frente
        {
            adicionarCasa((int)currentPos.x + 1, (int)currentPos.z);

            if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z) == false) //checa 2 casas a frente
            {
                adicionarCasa((int)currentPos.x + 2, (int)currentPos.z);

                if (tabuleiro.checaCasa((int)currentPos.x + 3, (int)currentPos.z) == false) //checa 3 casas a frente
                    adicionarCasa((int)currentPos.x + 3, (int)currentPos.z);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z + 1) == false) //checa 2 casas a frente e uma a esquerda
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z - 1) == false) //checa 2 casas a frente e uma a direita
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z - 1);
            }

            if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z + 1) == false) //checa a casa diagonal da esquerda
            {
                adicionarCasa((int)currentPos.x + 1, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z + 2) == false) //checa 1 casa para frente, 2 para esquerda
                    adicionarCasa((int)currentPos.x + 1, (int)currentPos.z + 2);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z + 1) == false) //checa 2 casas a frente e uma a esquerda
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z + 1);
            }

            if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z - 1) == false) //checa a casa diagonal da direita
            {
                adicionarCasa((int)currentPos.x + 1, (int)currentPos.z - 1);

                if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z - 2) == false) //checa 1 casa para frente, 2 para 
                    adicionarCasa((int)currentPos.x + 1, (int)currentPos.z - 2);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z - 1) == false) //checa 2 casas a frente e uma a direita
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z - 1);
            }
        }

        if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z) == false)
        {
            adicionarCasa((int)currentPos.x - 1, (int)currentPos.z);

            if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z) == false)
            {
                adicionarCasa((int)currentPos.x - 2, (int)currentPos.z);

                if (tabuleiro.checaCasa((int)currentPos.x - 3, (int)currentPos.z) == false)
                    adicionarCasa((int)currentPos.x - 3, (int)currentPos.z);

                if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z + 1) == false)
                    adicionarCasa((int)currentPos.x - 2, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z - 1) == false)
                    adicionarCasa((int)currentPos.x - 2, (int)currentPos.z - 1);
            }

            if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z + 1) == false)
            {
                adicionarCasa((int)currentPos.x - 1, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z + 2) == false)
                    adicionarCasa((int)currentPos.x - 1, (int)currentPos.z + 2);

                if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z + 1) == false)
                    adicionarCasa((int)currentPos.x - 2, (int)currentPos.z + 1);
            }

            if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z - 1) == false)
            {
                adicionarCasa((int)currentPos.x - 1, (int)currentPos.z - 1);

                if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z - 2) == false)
                    adicionarCasa((int)currentPos.x - 1, (int)currentPos.z - 2);

                if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z - 1) == false)
                    adicionarCasa((int)currentPos.x - 2, (int)currentPos.z - 1);
            }
        }

        if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z + 1) == false)
        {
            adicionarCasa((int)currentPos.x, (int)currentPos.z + 1);

            if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z + 2) == false)
            {
                adicionarCasa((int)currentPos.x, (int)currentPos.z + 2);

                if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z + 3) == false)
                    adicionarCasa((int)currentPos.x, (int)currentPos.z + 3);

                if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z + 2) == false)
                    adicionarCasa((int)currentPos.x + 1, (int)currentPos.z + 2);

                if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z + 2) == false)
                    adicionarCasa((int)currentPos.x - 1, (int)currentPos.z + 2);
            }

            if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z + 1) == false)
            {
                adicionarCasa((int)currentPos.x - 1, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z + 1) == false)
                    adicionarCasa((int)currentPos.x - 2, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z + 1) == false)
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z + 1);
            }

            if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z + 1) == false)
            {
                adicionarCasa((int)currentPos.x + 1, (int)currentPos.z + 1);

                if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z + 2) == false)
                    adicionarCasa((int)currentPos.x + 1, (int)currentPos.z + 2);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z + 1) == false)
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z + 1);
            }
        }

        if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z - 1) == false) 
        {
            adicionarCasa((int)currentPos.x, (int)currentPos.z - 1);
            
            if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z - 2) == false) 
            {
                adicionarCasa((int)currentPos.x, (int)currentPos.z - 2);
                
                if (tabuleiro.checaCasa((int)currentPos.x, (int)currentPos.z - 3) == false)
                    adicionarCasa((int)currentPos.x, (int)currentPos.z - 3);
                    
                if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z - 2) == false)
                    adicionarCasa((int)currentPos.x + 1, (int)currentPos.z - 2);
                
                if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z - 2) == false)
                    adicionarCasa((int)currentPos.x - 1, (int)currentPos.z - 2);                    
            }

            if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z - 1) == false)
            {
                adicionarCasa((int)currentPos.x + 1, (int)currentPos.z - 1);

                if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z - 2) == false)
                    adicionarCasa((int)currentPos.x + 1, (int)currentPos.z - 2);

                if (tabuleiro.checaCasa((int)currentPos.x + 2, (int)currentPos.z - 1) == false)
                    adicionarCasa((int)currentPos.x + 2, (int)currentPos.z - 1);
            }
                                        
            if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z - 1) == false)
            {
                adicionarCasa((int)currentPos.x - 1, (int)currentPos.z - 1);

                if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z - 2) == false)
                    adicionarCasa((int)currentPos.x - 1, (int)currentPos.z - 2);

                if (tabuleiro.checaCasa((int)currentPos.x - 2, (int)currentPos.z - 1) == false)
                    adicionarCasa((int)currentPos.x - 2, (int)currentPos.z - 1);
            }                                            
        }
                                                    
        //checagem de casas diagonais
        if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z + 1) == false)
            adicionarCasa((int)currentPos.x + 1, (int)currentPos.z + 1);
        
        if (tabuleiro.checaCasa((int)currentPos.x + 1, (int)currentPos.z - 1) == false)
            adicionarCasa((int)currentPos.x + 1, (int)currentPos.z - 1);
        
        if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z + 1) == false)
            adicionarCasa((int)currentPos.x - 1, (int)currentPos.z + 1);

        if (tabuleiro.checaCasa((int)currentPos.x - 1, (int)currentPos.z - 1) == false)
            adicionarCasa((int)currentPos.x - 1, (int)currentPos.z - 1);
    }

    private void adicionarCasa(int x, int y)
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

    void mostrarAtaque()
    {

    }

    void selecionar()
    {
        rend.material.color = hoverColor;
        selectedHunter = this;
        mostrarMovimento();
        mostrarAtaque();
        StartCoroutine(WaitForClick());
    }

    void deSelecionar()
    {
        //voltar cor original
        rend.material.color = startColor;

        //Define a peça selecionada como nenhuma
        selectedHunter = null;

        //apagar movimento
        RestaurarCasasDisponiveis();

        //Apaga a lista de movimentos válidos
        casasDisponiveis.Clear();


        //apagar ataque
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("circuloAtk"))
        {
            Destroy(c);
        }
    }

    void movimentar()
    {

    }

    private void RestaurarCasasDisponiveis()
    {
        foreach (var casa in casasDisponiveis)
        {
            if (casaCoresOriginais.TryGetValue(casa, out Color originalColor))
                casa.GetComponent<Renderer>().material.color = originalColor; // Restaura a cor original
        }
    }

}