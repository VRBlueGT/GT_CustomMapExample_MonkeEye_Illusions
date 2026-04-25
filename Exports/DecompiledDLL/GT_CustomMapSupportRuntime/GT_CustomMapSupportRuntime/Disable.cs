using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GT_CustomMapSupportRuntime;

public class Disable : MonoBehaviour
{

    public GameObject objectDisable;


    void OnTriggerEnter()
    {
        objectDisable.SetActive(false);
    }
}