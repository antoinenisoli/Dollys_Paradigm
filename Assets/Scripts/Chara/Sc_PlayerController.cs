using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Sc_PlayerController : Sc_Character
{
    public Camera viewCam => Camera.main;
    public Rigidbody rb => GetComponent<Rigidbody>();
    Sc_Gun myGun => FindObjectOfType<Sc_Gun>();

    [Header("Clone corpse")]
    public bool HasCorpse;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] Image hitScreen;
    [SerializeField] Image corpseIcon;
    [SerializeField] GameObject corpse;
    [SerializeField] GameObject SpawnCorpse;
    [HideInInspector] public GameObject lastSpawnedCorpse;
    [SerializeField] GameObject lastDeadCorpse;
    [SerializeField] bool onLava;

    [Header("Movements")]
    [SerializeField] float moveSpeed = 800;
    [SerializeField] float camSensitivity = 150;
    [SerializeField] float camBounds = 40;
    float rotY;

    [Header("Jump")]
    [SerializeField] float jumpForce = 5;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDist = 1f;
    [SerializeField] bool detectedGround;
    RaycastHit rayHit;
    Light myLight;

    [Header("Audio")]
    [SerializeField] AudioSource walkSound;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource healSound;
    [SerializeField] AudioSource dropSound;
    public AudioSource lootSound;
    [SerializeField] AudioSource footstepSound;
    float walkDelay;

    public override void Start()
    {
        base.Start();
        Respawn();
        myLight = GetComponentInChildren<Light>();
    }

    public void WalkSound()
    {
        walkSound.pitch = Random.Range(0.8f, 1.2f);
        walkSound.Play();
        walkDelay = 0;
    }

    void FindSafePlace(Vector3 offSet, List<RaycastHit> hits)
    {
        _ = Physics.Raycast(transform.position + offSet, -transform.up, out RaycastHit hit, groundDist*2, groundLayer);
        hits.Add(hit);
    }

    public override void Death()
    {
        manager.roomIndex = manager.savedRoomIndex;
        if (lastDeadCorpse != null)
        {
            Destroy(lastDeadCorpse);
            lastDeadCorpse = null;
        }

        if (onLava)
        {
            bool findPlace = false;
            RaycastHit newHit = new RaycastHit();
            List<RaycastHit> allHits = new List<RaycastHit>();
            float set = 5;
            FindSafePlace(new Vector3(1 * set, 2), allHits);
            FindSafePlace(new Vector3(-1 * set, 2), allHits);
            FindSafePlace(new Vector3(0, 2, 1 * set), allHits);
            FindSafePlace(new Vector3(0, 2, -1 * set), allHits);

            foreach (RaycastHit safePos in allHits)
            {
                if (safePos.collider != null)
                {
                    findPlace = true;
                    newHit = safePos;
                    print(safePos.collider.gameObject + "/" + safePos.point);
                    break;
                }
            }

            if (findPlace)
                lastDeadCorpse = Instantiate(corpse, newHit.point + Vector3.up * 2, Quaternion.identity);
            else
                lastDeadCorpse = Instantiate(corpse, rayHit.point + Vector3.up * 2, Quaternion.identity);
        }
        else
        {
            lastDeadCorpse = Instantiate(corpse, rayHit.point + Vector3.up * 2, Quaternion.identity);
        }

        deathSound.Play();
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
            hitSound.Play();
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
        move.y = 0;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);

        walkDelay += Time.deltaTime;
        if (move.sqrMagnitude > 0 && detectedGround && walkDelay > 0.5f)
        {
            WalkSound();
        }
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
        if (detectedGround)
        {
            onLava = rayHit.collider.GetComponent<Sc_Lava>();

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            onLava = false;
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
        detectedGround = Physics.Raycast(transform.position, Vector3.down, out rayHit, groundDist);
    }

    void SetClone()
    {
        if (Input.GetButtonDown("Interact") && HasCorpse && !myGun.detectInteract && detectedGround)
        {
            lastSpawnedCorpse = Instantiate(SpawnCorpse, rayHit.point + rayHit.normal + (transform.forward * 3.5f), Quaternion.identity);
            HasCorpse = false;
            dropSound.Play();
            spawnPos = lastSpawnedCorpse.transform.position + Vector3.up * 2;
            manager.savedRoomIndex = manager.roomIndex;
        }
    }

    void UseInputs()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            pauseScreen.SetActive(Time.timeScale == 0);
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
