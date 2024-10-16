using UnityEngine;

public class AttackCircleManager : MonoBehaviour
{
    public GameObject attackCirclePrefab; 
    private GameObject currentAttackCircle;

    public void ShowAttackCircle(Vector3 position)
    {
        if (currentAttackCircle != null)
        {
            Destroy(currentAttackCircle); // Remove o c�rculo anterior
        }

        // Cria um novo c�rculo de ataque 
        currentAttackCircle = Instantiate(attackCirclePrefab, position, Quaternion.identity);
        currentAttackCircle.transform.localScale = new Vector3(1, 0.1f, 1); 
    }

    public void HideAttackCircle()
    {
        if (currentAttackCircle != null)
        {
            Destroy(currentAttackCircle);
        }
    }
}        // essa bosta ta me dando dor de cabe�a , quero dormi kkk
