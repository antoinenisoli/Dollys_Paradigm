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
    [SerializeField] GameObject corpse;
    [SerializeField] GameObject SpawnCorpse;
    GameObject lastSpawnedCorpse;
    public GameObject lastDeadCorpse;

    [SerializeField] float moveSpeed = 800;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    float rotY;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDist = 1f;
    bool detectedGround;

    public override void Death()
    {
        if (lastDeadCorpse != null)
        {
            Destroy(lastDeadCorpse);
            lastDeadCorpse = null;
        }

        lastDeadCorpse = Instantiate(corpse, transform.position + Vector3.up * 3f, Quaternion.identity);
        HasCorpse = false;
        if (lastSpawnedCorpse != null)
        {
            Destroy(lastSpawnedCorpse);
            lastSpawnedCorpse = null;
        }

        Sc_EventManager.current.GameOverScreen();
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
        hitScreen.gameObject.SetActive(true);
        newCol.a = 0.4f;
        hitScreen.color = newCol;
        yield return new WaitForSeconds(0.15f);
        hitScreen.gameObject.SetActive(false);
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
            spawnPos = lastSpawnedCorpse.transform.position;
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

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }
}
