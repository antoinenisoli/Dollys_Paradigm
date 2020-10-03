using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Gun : MonoBehaviour
{
    Sc_PlayerController player => FindObjectOfType<Sc_PlayerController>();
    [SerializeField] LayerMask enemyLayer = 1 >> 8;
    [SerializeField] int damage = 5;
    [SerializeField] float shootRange = 50;
    [SerializeField] GameObject shootFX;

    [SerializeField] private int currentAmmo;
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = player.viewCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            bool find = Physics.Raycast(ray, out RaycastHit hit, shootRange);
            if (find)
            {
                Instantiate(shootFX, hit.point, Quaternion.identity);

                Sc_Health chara = hit.collider.GetComponentInParent<Sc_Health>();
                if (chara)
                {
                    chara.TakeDamages(damage);
                }
            }
        }
    }

    void Update()
    {
        Shooting();
    }
}
