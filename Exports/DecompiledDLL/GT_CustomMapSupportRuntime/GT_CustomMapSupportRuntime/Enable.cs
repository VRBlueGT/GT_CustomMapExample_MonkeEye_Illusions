using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public class Enable : MonoBehaviour
{
    public GameObject objectEnable;


    public void OnTriggerEnter()
    {
        objectEnable.SetActive(true);
    }
}