using UnityEngine;

public class AttackCircleManager : MonoBehaviour
{
    public GameObject attackCirclePrefab,circulo;
    private GameObject currentAttackCircle;

    public ParticleSystem attackParticles; // Adicione particulas

   /* private void Start()
    {
        circulo = this.gameObject;

    }*/

    public void ShowAttackCircle(Vector3 position)
    {
        if (currentAttackCircle != null)
        {
            Destroy(currentAttackCircle); // Remove o c�rculo anterior
        }

        // Cria um c�rculo de ataque 
        position.y = 0.1f;
        currentAttackCircle = Instantiate(attackCirclePrefab, position, Quaternion.identity);
        currentAttackCircle.transform.localScale = new Vector3(1, 1f, 1);

        // Ativa o efeito de part�culas
        if (attackParticles != null)
        {
            ParticleSystem particles = Instantiate(attackParticles, position, Quaternion.identity);

            particles.Play();
        }
    }

    public void HideAttackCircle()
    {
        Destroy(circulo);
    }
}
