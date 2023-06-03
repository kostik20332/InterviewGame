using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeLogic : MonoBehaviour
{
    public bool animating = false;
    //public GameObject cone;
    public GameObject particalEffect;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Animating());
    }

    // Update is called once per frame
    void Update()
    {
        if (DrawingLogic.play)
        {
            //this.transform.Translate(new Vector3(0f, 0f, -PlatformMoving.movingSpeed) * Time.deltaTime, Space.World);
        }
        if (!animating)
        {
            StartCoroutine(Animating());
        }
    }

    IEnumerator Animating()
    {
        animating = true;
        StartCoroutine(MoveDown());
        yield return new WaitForSeconds(4f);
        StartCoroutine(MoveUp());
        yield return new WaitForSeconds(5f);
        animating = false;
    }

    IEnumerator MoveDown()
    {
        for (int i = 0; i < 200; i++)
        {
            this.transform.Translate(0, -0.01f, 0, Space.World);
            yield return new WaitForSeconds(0.0003f);
        }
    }

    IEnumerator MoveUp()
    {
        for (int i = 0; i < 200; i++)
        {
            this.transform.Translate(0, 0.01f, 0, Space.World);
            yield return new WaitForSeconds(0.0003f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "activeGuy")
        {
            Instantiate(particalEffect, other.transform.position, Quaternion.Euler(45f, 90f, 0f));
            Destroy(other.gameObject);
            DrawingLogic.fallGuysAmount--;
        }
    }
}
