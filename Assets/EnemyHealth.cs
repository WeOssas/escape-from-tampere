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
    }

    void Update()
    {
        slider.value = CalculateHealth();


        if (health < maxHealth)
        {
            healthUI.SetActive(true);
        }

        if (health <= 0)
        {
            healthUI.SetActive(false);
            Destroy(gameObject);
            defeated = true;
        }
           
        if (SceneManager.GetActiveScene().name == "BossBattle" & defeated)
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