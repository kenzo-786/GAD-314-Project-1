using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float regenRate = 5f;
    public float regenDelay = 3f;

    [Header("UI References")]
    public GameObject healthBarContainer;
    public Image healthFillImage;
    public GameObject deathScreenPanel;
    public TextMeshProUGUI deathText;

    public Image damageFlashImage;

    [Header("Respawn")]
    public Transform respawnPoint;

    private float _lastDamageTime;
    private bool _isDead = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (healthBarContainer) healthBarContainer.SetActive(false);
        if (deathScreenPanel) deathScreenPanel.SetActive(false);

        if (respawnPoint == null)
        {
            GameObject spawnObj = GameObject.Find("SpawnPoint_Arrival");
            if (spawnObj) respawnPoint = spawnObj.transform;
        }
    }

    private void Update()
    {
        if (_isDead) return;

        if (Time.time > _lastDamageTime + regenDelay && currentHealth < maxHealth)
        {
            Heal(regenRate * Time.deltaTime);
        }

        if (damageFlashImage && damageFlashImage.color.a > 0)
        {
            Color c = damageFlashImage.color;
            c.a -= Time.deltaTime * 2f;
            damageFlashImage.color = c;
        }
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        currentHealth -= amount;
        _lastDamageTime = Time.time;

        if (healthBarContainer) healthBarContainer.SetActive(true);

        if (damageFlashImage)
        {
            Color c = Color.red;
            c.a = 0.5f;
            damageFlashImage.color = c;
        }

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            if (healthBarContainer) healthBarContainer.SetActive(false);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthFillImage)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
            healthFillImage.color = Color.Lerp(Color.red, Color.green, currentHealth / maxHealth);
        }
    }

    private void Die()
    {
        _isDead = true;
        Debug.Log("Player Died!");

        if (GameManager.Instance) GameManager.Instance.SetState(GameState.Paused);

        StartCoroutine(RespawnRoutine());
    }


    private IEnumerator RespawnRoutine()
    {
        if (deathScreenPanel) deathScreenPanel.SetActive(true);

        float countdown = 4f;
        while (countdown > 0)
        {
            if (deathText) deathText.text = $"Respawning in {Mathf.Ceil(countdown)}...";
            yield return null;
            countdown -= Time.deltaTime;
        }

        Respawn();
    }

    private void Respawn()
    {
        _isDead = false;
        currentHealth = maxHealth;
        UpdateUI();

        if (deathScreenPanel) deathScreenPanel.SetActive(false);
        if (healthBarContainer) healthBarContainer.SetActive(false);

        if (respawnPoint)
        {
            CharacterController cc = GetComponent<CharacterController>();
            if (cc) cc.enabled = false;

            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;

            if (cc) cc.enabled = true;
        }

        if (GameManager.Instance) GameManager.Instance.SetState(GameState.Gameplay);
    }
}
