using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RombLogic : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(0, 0.3f, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "activeGuy")
        {
            DrawingLogic.score += 10;
            Destroy(this.gameObject);
        }
    }
}
