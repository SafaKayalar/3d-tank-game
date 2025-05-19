using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEditor.Build;

public class GameManager : MonoBehaviour
{
    public Player player;
    public int wave;
    public List<Spawner> spawners = new List<Spawner>() ;
    public List<Enemy> enemies = new List<Enemy>();
    public MenuManager menuManager;
    public GameObject upgradesScreen;
    public GameObject deadMenu;
    public Image fillImage;
    public bool isPaused;
    public PlayerUpgrades upgrader;
    public Button u1;
    public Button u2;
    public Button u3;
    public bool isUpgrade = false;
    private List<KeyValuePair<string, Action>> selectedUpgrades = new List<KeyValuePair<string, Action>>();
    public Text healthText;
    public Text waveText;
    


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Mouse'u ekranýn ortasýna kilitle
        Cursor.visible = false; // Mouse imlecini gizle
        player.StartPlayer();
        wave = 0;
        enemies.Clear();
        isPaused = false;
        upgradesScreen.SetActive(false);
        isUpgrade = false;
        deadMenu.SetActive(false);
    }



    private void Update()
    {
        waveText.text =  $"Wave: {wave}";
        SpawnManager();
        EnemyController();
        UpgradePlayer();
        Pause_Menu();
        PlayerDead();
        UpdateHealthbar();
    }


    public void UpdateHealthbar()
    {
        fillImage.fillAmount = (float)player.currentHealthPoint / player.maxHealthPoint;
        healthText.text = healthText.text = $"{player.currentHealthPoint}/{player.maxHealthPoint}";
        if (player.currentHealthPoint <= 40)
        {
            healthText.color = Color.red;
        }
        else
        {
            healthText.color = Color.green;
        }
    }

    public void PlayerDead()
    {
        if (player.currentHealthPoint <= 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Destroy(gameObject);
            Time.timeScale = 0f;
            deadMenu.SetActive(true);
        }
    }

    private void Pause_Menu()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            menuManager.PauseGame();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {

            menuManager.ContiuneGame();

        }
    }

    private void EnemyController()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].isDead == true)
            {
                Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
            }
        }
    }

    private void SpawnManager()
    {
        if (enemies.Count == 0)
        {
            wave++;
            isUpgrade = false;
            foreach (var s in spawners)
            {
                s.SpawnEnemy();
            }
        }
    }

    private void UpgradePlayer()
    {
        // Buton listesi
        List<Button> buttons = new List<Button> { u1, u2, u3 };

        // Upgrade seçenekleri
        Dictionary<string, Action> upgradeDictionary = new Dictionary<string, Action>
        {
            { "Max HP +20", upgrader.UpgradeMaxHp },
            { "Hýz +0.5", upgrader.UpgradeMovementSpeed },
            { "Atak Hýzý +0.2", upgrader.UpgradeAttackSpeed },
            { "Hasar +5", upgrader.UpgradeAttackDamage },
            { "Can Yenileme +25", upgrader.CurrentHP }
        };

        if (wave % 3 == 0 && isUpgrade == false)
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            upgradesScreen.SetActive(true);
            // Eðer daha önce seçim yapýlmadýysa rastgele 3 upgrade seç
            if (selectedUpgrades.Count == 0)
            {
                List<KeyValuePair<string, Action>> upgradeOptions = new List<KeyValuePair<string, Action>>(upgradeDictionary);

                // Karýþtýr
                for (int i = upgradeOptions.Count - 1; i > 0; i--)
                {
                    int j = UnityEngine.Random.Range(0, i + 1);
                    (upgradeOptions[i], upgradeOptions[j]) = (upgradeOptions[j], upgradeOptions[i]);
                }

                // Ýlk 3 tanesini kaydet
                selectedUpgrades = upgradeOptions.Take(3).ToList();
            }

            // Butonlara seçili yükseltmeleri ata
            for (int i = 0; i < 3; i++)
            {
                int index = i;
                buttons[i].onClick.RemoveAllListeners(); // Önceki eventleri temizle
                buttons[i].GetComponentInChildren<Text>().text = selectedUpgrades[i].Key; // Buton metnini ayarla
                buttons[i].onClick.AddListener(() =>
                {
                    selectedUpgrades[index].Value(); // Seçilen upgrade'i uygula
                    selectedUpgrades.Clear(); // Seçimleri sýfýrla
                    Time.timeScale = 1f;
                    upgradesScreen.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    isUpgrade = true;                 
                });
            }
        }
    }
}
