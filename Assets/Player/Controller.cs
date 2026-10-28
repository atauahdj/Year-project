using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public Animator_player AP;
    public float speed = 5f;
    ParentNPC pnpc;
    CharacterController cc;
    public GameObject prefab_body;
    public GameObject Wanted_Image;
    public bool CanUse;
    public GameObject Use_button;
    Kolonka_boom kb;
    void Start()
    {
        AP = GetComponent<Animator_player>();
        cc = GetComponent<CharacterController>();
        pnpc = FindObjectOfType<ParentNPC>();
        transform.position = new Vector3(transform.position.x,transform.position.y,-2);
        Wanted_Image.SetActive(false);
        kb = FindObjectOfType<Kolonka_boom>();
    }

    Vector2 movement;
    void Update()
    {
        Vector2 moveX = Gamepad.current.leftStick.ReadValue();
        Vector2 moveY = Gamepad.current.leftStick.ReadValue();
        movement = new Vector3(0,0, -2);
        movement += new Vector2(moveX.x * speed * Time.deltaTime, moveY.y * speed * Time.deltaTime);
        if (Keyboard.current.shiftKey.IsPressed())
        {
            speed = 7;
        }
        else
        {
            speed = 5;
        }
        if(moveX.x < 0 && moveX.x != 0)
        {
            AP.player_renderer.flipX = true;
        }
        else if (moveX.x != 0)
        {
            AP.player_renderer.flipX =false;
        }
        if (cc.velocity != Vector3.zero)
        {
            AP.walking = true;
        }
        else
        {
            AP.walking = false;
        }
        cc.Move(movement);
        if (CanUse)
        {
            Use_button.SetActive(true);
        }
        else
        {
            Use_button.SetActive(false);
        }
        Debug.Log(kb.all_kolonki.Length);
        // ==== Активация колонки по клавише R ====
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            // 1. Ищем ближайшую ЖИВУЮ колонку
            float minDist = Mathf.Infinity;
            GameObject nearest = null;

            for (int ck = 0; ck < kb.all_kolonki.Length; ck++)
            {
                if (kb.all_kolonki[ck] == null) continue; // пропускаем уничтоженные

                float d = Vector3.Distance(transform.position, kb.all_kolonki[ck].transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = kb.all_kolonki[ck].gameObject;
                }
            }

            // 2. Проверяем, что ближайшая колонка в радиусе активации
            if (nearest != null && minDist <= 5f)
            {
                kb.current_kolonka = nearest;
                kb.boom = true;
            }
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameObject[] npc_c = GameObject.FindGameObjectsWithTag("NPC");

            // Ищем ближайшего NPC в радиусе
            GameObject nearestNpc = null;
            float minDist = Mathf.Infinity;

            foreach (GameObject npc in npc_c)
            {
                float d = Vector2.Distance(npc.transform.position, transform.position);
                if (d < 2f && d < minDist)
                {
                    minDist = d;
                    nearestNpc = npc;
                }
            }
            Debug.Log(nearestNpc);
            // Убиваем только одного
            if (nearestNpc != null)
            {
                pnpc.npc_counter++;
                GameObject body_dead = Instantiate(prefab_body);
                body_dead.transform.position = nearestNpc.transform.position;
                Destroy(nearestNpc);
            }
        }
    }
}
