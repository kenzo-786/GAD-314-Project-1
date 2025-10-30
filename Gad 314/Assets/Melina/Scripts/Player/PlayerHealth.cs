using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float regenRate = 5f;
    private bool isBeingChased = false;

    public Slider healthBar;
    public GameObject damageTextPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        if (!isBeingChased && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateHealthUI();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        ShowDamageText(damage);

        if (currentHealth <= 0)
        {
            Debug.Log("Player died!");
        }
    }

    public void SetChased(bool chased)
    {
        isBeingChased = chased;
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2f + Vector3.right * 1f);
            GameObject dmgText = Instantiate(damageTextPrefab, screenPos, Quaternion.identity);
            if (GameObject.Find("Canvas") != null)
                dmgText.transform.SetParent(GameObject.Find("Canvas").transform, false);

            Text text = dmgText.GetComponent<Text>();
            if (text != null)
            {
                text.text = "-" + damage.ToString("0");
                text.color = Color.red;
            }

            Destroy(dmgText, 1f);
        }
    }
}
