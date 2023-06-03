using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChanging : MonoBehaviour
{
    private Animator animator;
    private bool run = false;
    public static bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DrawingLogic.play && !run)
        {
            run = true;
            animator.SetBool("run", true);
        }
        if (DrawingLogic.finish)
        {
            animator.SetBool("finish", true);
        }
    }
}
