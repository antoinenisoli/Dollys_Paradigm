using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Gun : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    [SerializeField] int damage = 5;
    [SerializeField] float shootRange = 50;
    [SerializeField] GameObject[] shootFX;
    [SerializeField] AnimationClip clip;
    [SerializeField] LayerMask shootLayer;
    [SerializeField] float shootDelay = 0.1f;
    float timer;

    [SerializeField] bool Auto;
    Animator anim => GetComponent<Animator>();

    private int currentAmmo;
    public int CurrentAmmo 
    { 
        get => currentAmmo;

        set
        {
            if (value < 0)
                value = 0;

            currentAmmo = value;
        }
    }

    void Shooting()
    {
        timer += Time.deltaTime;
        if (Auto)
        {
            bool holdingFire = Input.GetMouseButton(0);
            anim.SetBool("HoldingFire", holdingFire);

            if (holdingFire && timer > shootDelay)
            {
                Shoot();
                timer = 0;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && timer > shootDelay)
            {
                anim.SetTrigger("Shoot");
                timer = 0;
            }
        }
    }

    public void Shoot()
    {
        Ray ray = player.viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        bool find = Physics.Raycast(ray, out RaycastHit hit, shootRange, shootLayer);
        CurrentAmmo--;
        if (find)
        {
            Vector3 impactPos = hit.point + hit.normal;
            Sc_Character chara = hit.collider.GetComponentInParent<Sc_Character>();
            if (chara)
            {
                Instantiate(shootFX[1], impactPos, Quaternion.LookRotation(hit.normal));

                if (!chara.Health.isDead)
                {
                    if (Auto)
                    {
                        chara.Hurt(damage);
                    }
                    else
                    {
                        chara.Hurt(damage * 3);
                    }
                }
            }
            else
            {
                Instantiate(shootFX[0], impactPos, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    void Update()
    {
        Vector2 vel = new Vector2(player.rb.velocity.x, player.rb.velocity.z).normalized;
        anim.SetFloat("Velocity", vel.sqrMagnitude);

        if (player.Health.isDead)
            return;

        Shooting();
    }
}
