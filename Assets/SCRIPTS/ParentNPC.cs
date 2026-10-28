using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Reflection;
using System.Net.Http.Headers;
using UnityEngine.InputSystem;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class ParentNPC : MonoBehaviour
{
    
    public GameObject[] AllNPC;
    public GameObject npc_prefab;
    public TMP_Text score_murders;
    public int npc_counter;
    int counter_npc = 0;
    void Start()
    {
        int max_npc = Random.Range(51,68);
        while(counter_npc <= max_npc-1)
        {
            Instantiate(npc_prefab);
            counter_npc++;
        }
        AllNPC = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in AllNPC)
        {
            npc.transform.SetParent(transform, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        score_murders.text = $"Was has killed {npc_counter}/{counter_npc}";
        if(npc_counter == counter_npc)
        {
            SceneManager.LoadScene(0);
        }
    }
}
