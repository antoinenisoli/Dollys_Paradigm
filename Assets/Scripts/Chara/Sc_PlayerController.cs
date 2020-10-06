using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_PlayerController : Sc_Character
{
    public Camera viewCam => Camera.main;
    public Rigidbody rb => GetComponent<Rigidbody>();
    Sc_Gun myGun => FindObjectOfType<Sc_Gun>();

    [Header("Clone corpse")]
    public bool HasCorpse;
    [SerializeField] Image hitScreen;
    [SerializeField] Image corpseIcon;
    [SerializeField] GameObject corpse;
    [SerializeField] GameObject SpawnCorpse;
    public GameObject lastSpawnedCorpse;
    GameObject lastDeadCorpse;

    [SerializeField] float moveSpeed = 800;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    float rotY;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDist = 1f;
    bool detectedGround;
    Light myLight;

    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource healSound;
    [SerializeField] AudioSource dropSound;
    public AudioSource lootSound;
    [SerializeField] AudioSource footstepSound;

    public override void Start()
    {
        base.Start();
        Respawn();
        myLight = GetComponentInChildren<Light>();
    }

    public override void Respawn()
    {
        manager.roomIndex = manager.savedRoomIndex;
        base.Respawn();
    }

    public override void Death()
    {
        if (lastDeadCorpse != null)
        {
            Destroy(lastDeadCorpse);
            lastDeadCorpse = null;
        }

        deathSound.Play();
        lastDeadCorpse = Instantiate(corpse, transform.position + Vector3.up * 3f, Quaternion.identity);
        HasCorpse = false;
        if (lastSpawnedCorpse != null)
        {
            Destroy(lastSpawnedCorpse);
            lastSpawnedCorpse = null;
        }

        Sc_EventManager.current.GameOverScreen();
    }

    public override void Hurt(int _dmg)
    {
        base.Hurt(_dmg);

        if (_dmg > 0)
        {
            StartCoroutine(ChangeLifeColor(Color.red));
        }
        else
        {
            StartCoroutine(ChangeLifeColor(Color.green));
            healSound.Play();
        }
    }

    void FPS_Move()
    {
        Vector3 horizontalAxis = transform.right * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        Vector3 verticalAxis = transform.forward * Input.GetAxisRaw("Vertical") * Time.deltaTime;
        Vector3 normalized = (horizontalAxis + verticalAxis).normalized;

        Vector3 move = normalized * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void CameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector2 mouseInput = new Vector2(mouseX, mouseY) * camSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - mouseInput.x, transform.rotation.eulerAngles.z);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotY += mouseY * camSensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -camBounds, camBounds);
        viewCam.transform.rotation = Quaternion.Euler(rotY, viewCam.transform.rotation.eulerAngles.y, viewCam.transform.rotation.eulerAngles.z);
    }

    void Jump()
    {
        if (detectedGround && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public override IEnumerator ChangeLifeColor(Color color)
    {
        Color newCol = color;
        hit = true;
        hitScreen.gameObject.SetActive(true);
        newCol.a = 0.4f;
        hitScreen.color = newCol;
        yield return new WaitForSeconds(0.8f);
        hitScreen.gameObject.SetActive(false);
        hit = false;
    }

    private void FixedUpdate()
    {
        detectedGround = Physics.Raycast(transform.position, Vector3.down, groundDist);
    }

    void SetClone()
    {
        if (Input.GetButtonDown("Interact") && HasCorpse && !myGun.detectInteract)
        {
            lastSpawnedCorpse = Instantiate(SpawnCorpse, transform.position + (transform.forward * 3), Quaternion.identity);
            HasCorpse = false;
            dropSound.Play();
            spawnPos = lastSpawnedCorpse.transform.position;
            manager.savedRoomIndex = manager.roomIndex;
        }
    }

    void UseInputs()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("Torch"))
        {
            myLight.gameObject.SetActive(!myLight.gameObject.activeSelf);
        }
    }

    private void Update()
    {
        if (!Health.isDead)
        {
            FPS_Move();
            CameraControl();
            Jump();
            SetClone();
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (HasCorpse)
            corpseIcon.material.EnableKeyword("_EMISSION");
        else
            corpseIcon.material.DisableKeyword("_EMISSION");

        UseInputs();
    }
}
