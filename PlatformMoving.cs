using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    public static float movingSpeed = 8f;

    void Update()
    {
        if (DrawingLogic.play)
        {
            this.transform.Translate(new Vector3(0f, 0f, -movingSpeed) * Time.deltaTime, Space.World);
        }
    }
}
