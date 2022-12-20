using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Slider slider;
    public GameObject healthUI;
    public string endScene;
    private bool defeated;
    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
        healthUI.SetActive(true);
    }

    void Update()
    {
        slider.value = CalculateHealth();


        if (health <= 0)
        {
            healthUI.SetActive(false);
            PlayerInstance.instance.zombiesKilled += 1;
            defeated = true;
            Destroy(gameObject);
            
        }
           
        if (SceneManager.GetActiveScene().name == "BossBattle" & defeated & endScene != null)
        {
            SceneManager.LoadScene(endScene);
        }

        


        if (health > maxHealth)
        {
            health = maxHealth;
        }

    }
    float CalculateHealth()
    {
        return health / maxHealth;
    }
}
