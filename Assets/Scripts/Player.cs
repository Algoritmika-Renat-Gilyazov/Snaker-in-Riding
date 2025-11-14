using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Slider healthMeter;

    public GameObject heakthMeter;

    [SerializeField]
    private BaseGameManager gameManager;

    [SerializeField]
    private float mouseSensitivity = 2f;
    private float xRotation = 0f;

    public Rigidbody playerRigidbody;

    [SerializeField]
    private float jumpForce = 7.5f;
    private float defaultJumpForce; // добавьте это поле
    [SerializeField]
    private float groundCheckDistance = 10f;
    [SerializeField]
    private LayerMask groundMask;
    private bool isGrounded;
    [SerializeField]
    private float sideCheckDistance = 0.6f;

    // Новые поля для механики шага
    [SerializeField]
    private float stepHeight = 0.3f; // Максимальная высота, на которую можно "зашагнуть"
    [SerializeField]
    private float stepSmooth = 0.1f; // Скорость подъема

    private float ms;

    public Slider sm;

    public Image cursor;

    public float maxStamina = 100f;
    public float currentStamina = 100f;

    public bool isRidding = false;

    public bool isRunning = false;

    public float health = 100f;
    public float maxHealth = 100f;

    public float damage = 5f;

    public void Hurt(float level)
    {
        if (gamemode != "Creative")
        {
            health -= level;
            UpdateHealth();
            if (health <= 0f)
            {
                GameOver();
            }
        }
    }

    public void UpdateHealth()
    {
        if(healthMeter.IsActive())
        {
            healthMeter.value = health;
        }
    }

    public string gamemode = "Survival";

    public void Attack(Character target)
    {
        target.Hurt(damage);
    }

    public void GameOver()
    {
        gameManager.Pause();
    }

    void Start()
    {
        try
        {
            if (PlayerPrefs.HasKey("Gamemode"))
            {
                gamemode = PlayerPrefs.GetString("Gamemode");
            }
            if(gamemode == "Creative")
            {
                health = Mathf.Infinity;
                heakthMeter.SetActive(false);
            }
            ms = gameManager.mouseSent;
            defaultJumpForce = jumpForce; // сохраняем стандартную силу прыжка
            StartCoroutine(ScrollInventory());
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
            StartCoroutine(Regenerate());
            UpdateHealth();
        } catch
        {
            gameManager.Crash("PLAYER_INIT_ERROR");
        }
    }

    public bool isWaking = false;

    public bool needJump = false;

    void OnTriggerStay(Collider hit)
    {
        if(hit.transform.tag == "Jumpable" && !isRidding)
        {
            if(jumpForce == defaultJumpForce) // чтобы не делить несколько раз
            {
                jumpForce /= 10f;
            }
            needJump = true;
        }
    }

    IEnumerator Regenerate()
    {
        if (health < maxHealth)
        {
            health += 5f;
            UpdateHealth();
            yield return new WaitForSeconds(1f);
        }
    }

    public LayerMask rideLayer;
    void Update()
    {
        if (gameManager.paused)
        {
            return;
        }
        RaycastHit hit;
        bool isPointed = Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 20f, rideLayer);
        if (isPointed)
        {
            Debug.Log("Pointed at: " + hit.transform.tag);
        }
        if (isPointed && hit.transform.tag == "Mount")
        {
            cursor.color = Color.white;
            if (Input.GetMouseButtonDown(1) && !isRidding)
            {
                if (!hit.transform.GetComponent<Character>().isRidden)
                {
                    Character ch = hit.transform.GetComponent<Character>();
                    if (ch != null)
                    {
                        ch.Mount(this);
                        isRidding = true;
                    }
                }
                {
                    Character ch = hit.transform.GetComponent<Character>();
                    ch.Mount(this);
                    isRidding = true;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Character ch = hit.transform.GetComponent<Character>();
                Attack(ch);
            }
        }
        // --- PICKUP MECHANIC ---
        else if (isPointed && hit.transform.GetComponent<Pickup>() != null)
        {
            cursor.color = Color.yellow; // Показываем, что можно подобрать

            if (Input.GetMouseButtonDown(0))
            {
                Pickup pickup = hit.transform.GetComponent<Pickup>();
                if (inv != null && pickup != null)
                {
                    int selectedSlot = inv.selectedSlot; // Используем выбранный слот
                    inv.AddItem(pickup.item, selectedSlot);
                    Destroy(pickup.gameObject); // Удаляем предмет из мира
                }
            }
        }
        else if (isPointed && hit.transform.GetComponent<Character>() != null)
        {
            cursor.color = Color.red;
            if (Input.GetMouseButtonDown(0))
            {
                Character ch = hit.transform.GetComponent<Character>();
                Attack(ch);
            }
        }
        else
        {
            cursor.color = Color.black;
        }
        if (isRidding && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Character ch = transform.parent.GetComponent<Character>();
            if (ch != null && ch.isRidden)
            {
                ch.Dismount();
                isRidding = false;
            }
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            isWaking = true;
        }
        else
        {
            isWaking = false;
        }

        Vector3 moveDirection = playerCamera.transform.forward * vertical + playerCamera.transform.right * horizontal;
        moveDirection.y = 0f;

        // Обновление позиции игрока
        if (isRunning && currentStamina > 0)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection.normalized * Time.deltaTime * 10f);
        }
        else
        {
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection.normalized * Time.deltaTime * 5f);
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            inv?.UseSelectedItem();
        }

        // Логика поворота камеры
        float mouseX = Input.GetAxis("Mouse X") * ms;
        float mouseY = Input.GetAxis("Mouse Y") * ms;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        UpdateHealth();
    }

    IEnumerator ScrollInventory()
    {
        if (inv == null) yield break;

        while (true)
        {
            inv.Scroll();
            yield return null;
        }
    }

    public Inventory inv;

    [System.Obsolete]
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (isRunning && isWaking)
        {
            if (currentStamina > 0)
            {
                currentStamina -= 10f * Time.fixedDeltaTime;
                if (currentStamina < 0) currentStamina = 0;
            }
        }
        else if (isWaking)
        {

        }
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += 10f * Time.fixedDeltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }
        }
        sm.value = currentStamina;

        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Vector3 basePos = transform.position;
        float height = col.height;
        float radius = col.radius;

        // сколько проверок по высоте
        int steps = 5;

        for (int i = 0; i <= steps; i++)
        {
            float yOffset = i / (float)steps * height;
            Vector3 origin = basePos + Vector3.up * yOffset;

            Debug.DrawRay(origin, transform.forward, Color.red);

            if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, 0.5f, groundMask))
            {
                Debug.Log("Перед игроком препятствие на высоте: " + yOffset);

            }
        }
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.red);

        // Прыжок
        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || needJump))
        {
            Debug.Log("Jump");
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            needJump = false;
            jumpForce = defaultJumpForce; // всегда возвращаем стандартное значение
        }

        CheckForStep();
        Debug.Log("On ground: " + isGrounded);
    }

    private void CheckForStep()
{
#pragma warning disable CS0618
        Vector3 moveDir = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z).normalized;
#pragma warning restore CS0618 // Тип или член устарел
        if (moveDir == Vector3.zero) return;

    RaycastHit hitLower;
    if (Physics.Raycast(transform.position + Vector3.up * 0.1f, moveDir, out hitLower, 0.5f, groundMask))
    {
        RaycastHit hitUpper;
        if (!Physics.Raycast(transform.position + Vector3.up * stepHeight, moveDir, out hitUpper, 0.5f, groundMask))
        {
            // Подъём плавный через Lerp
            Vector3 stepPosition = new Vector3(transform.position.x, transform.position.y + stepSmooth, transform.position.z);
            playerRigidbody.MovePosition(stepPosition);
        }
    }
}

}