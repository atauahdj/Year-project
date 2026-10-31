using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class poison_script : MonoBehaviour
{
    public ParentNPC pnpc;
    void Start()
    {
        pnpc = FindObjectOfType<ParentNPC>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject npc in pnpc.AllNPC)
        {
            
        }
    }
}
