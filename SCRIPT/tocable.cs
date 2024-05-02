using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tocable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "buggy")
        {
            this.gameObject.SetActive(false);
        }
    }
}
