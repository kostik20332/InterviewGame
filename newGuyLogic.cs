using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newGuyLogic : MonoBehaviour
{
    private bool isGuyActive = false;
    public GameObject fallGuysParent;
    public GameObject animatorParent;
    private Animator animator;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "activeGuy" && !isGuyActive)
        {
            isGuyActive = true;
            DrawingLogic.AddGuy(this.gameObject);
            this.transform.SetParent(fallGuysParent.transform, true);
            Rigidbody rb = GetComponent<Rigidbody>();
            Destroy(rb);
            BoxCollider col = GetComponent<BoxCollider>();
            col.isTrigger = true;
            animator = animatorParent.GetComponent<Animator>();
            animator.SetBool("run", true);
        }
    }
}
