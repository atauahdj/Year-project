using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kolonka_boom : MonoBehaviour
{
    public Controller player;
    public ParentNPC ParentNPC;
    public GameObject body_prefab;
    public GameObject[] all_kolonki;
    public bool boom = false;
    float till_boom;

    void Start()
    {
        player = FindObjectOfType<Controller>();
        ParentNPC = FindObjectOfType<ParentNPC>();
        all_kolonki = GameObject.FindGameObjectsWithTag("kolonka");
    }

    GameObject current_npc;
    public GameObject current_kolonka;

    void Update()
    {
        // сбрасываем каждый кадр, чтобы не «залипало
        if (all_kolonki != null && all_kolonki.Length > 0 && player != null && !boom)
        {
            float minDist = Mathf.Infinity;

            for (int ck = 0; ck < all_kolonki.Length; ck++)
            {
                if (all_kolonki[ck] == null) continue; // пропускаем уничтоженные

                float d = Vector3.Distance(player.transform.position, all_kolonki[ck].transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    current_kolonka = all_kolonki[ck].gameObject;
                }
            }
        }

        // защита от null перед логом
        if (current_kolonka != null)
        {
            Debug.Log(current_kolonka.name);
        }
        else
        {
            Debug.Log("Нет доступных колонок");
        }
        #region temp
        if (boom)
        {
            till_boom += Time.deltaTime;
            if(till_boom > 5f)
            {
                //choose npc
                current_npc = null;

                if (ParentNPC.AllNPC != null && ParentNPC.AllNPC.Length > 0)
                {
                    float minDist = Mathf.Infinity;

                    for (int i = 0; i < ParentNPC.AllNPC.Length; i++)
                    {
                        GameObject npc = ParentNPC.AllNPC[i];
                        if (npc == null) continue;

                        float d = Vector3.Distance(current_kolonka.transform.position, npc.transform.position);
                        if (d < minDist)
                        {
                            minDist = d;
                            current_npc = npc;
                        }
                    }
                }
                //kill
                if (Vector3.Distance(current_kolonka.transform.position, player.transform.position) <= 7f
                    || Vector3.Distance(current_kolonka.transform.position, current_npc.transform.position) <= 7f)
                {
                    if (Vector3.Distance(current_kolonka.transform.position, player.transform.position) <= 7f)
                    {
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        Instantiate(body_prefab);
                        body_prefab.transform.position = current_npc.transform.position;
                        Destroy(current_npc);
                    }
                }
                Destroy(current_kolonka);
                boom = false;
            }
        }
        if (Vector3.Distance(current_kolonka.transform.position, player.transform.position) < 5f && !boom)
        {
            player.CanUse = true;
        }
        else
        {
            player.CanUse = false;
        }
        #endregion
    }
}