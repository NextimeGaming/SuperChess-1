using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CadastroSystem : MonoBehaviour
{
   
    public InputField inputEmail;
    public InputField inputUsername;
    public InputField inputPassword;
    public Text feedbackText;
    public Button btnCadastrar;

    //   onde os dados ser�o armazenados 
    private string path = "cadastros.txt";

    void Start()
    {
        
        if (inputEmail == null || inputUsername == null || inputPassword == null || feedbackText == null || btnCadastrar == null)
        {
            Debug.LogError("Alguma refer�ncia n�o foi atribu�da no Inspetor!");
            return; 
        }

        
        btnCadastrar.onClick.AddListener(CadastrarUsuario);
    }

    void CadastrarUsuario()
    {
        // Verifica se todos os campos de InputField est�o atribu�dos
        if (inputEmail == null || inputUsername == null || inputPassword == null || feedbackText == null)
        {
            feedbackText.text = "Erro: Refer�ncia n�o atribu�da corretamente!";
            return;
        }

        string email = inputEmail.text;
        string username = inputUsername.text;
        string password = inputPassword.text;

        // Verifica se os campos n�o est�o vazios
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Preencha todos os campos!";
            return;
        }

       
        if (UsuarioExistente(username))
        {
            feedbackText.text = "Nome de usu�rio j� existe!";
            return;
        }

        //  ID �nico para o usu�rio
        string userId = GerarIdUnico();

        // Salva os dados do usu�rio no arquivo
        SalvarUsuario(userId, email, username, password);

        
        feedbackText.text = "Cadastro realizado com sucesso!";
    }

    bool UsuarioExistente(string username)
    {
        // Verifica se o arquivo existe 
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] dados = line.Split(';');
                if (dados.Length > 1 && dados[2] == username)  // Verifica se o nome de usu�rio j� est� registrado
                {
                    return true;
                }
            }
        }
        return false;
    }

    void SalvarUsuario(string userId, string email, string username, string password)
    {
        // Cria a string de dados do usu�rio
        string usuario = userId + ";" + email + ";" + username + ";" + password;

        // Salva os dados no arquivo de cadastro
        File.AppendAllText(path, usuario + "\n");
    }

    string GerarIdUnico()
    {
        int id = 1;
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            id = lines.Length + 1;
        }
        return id.ToString();
    }
}
