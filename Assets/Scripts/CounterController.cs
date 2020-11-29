using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    [SerializeField] CounterObject counter;

    [SerializeField] Image image;
    [SerializeField] Text text;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        image.sprite = counter.sprite;
    }

    void Update()
    {
        text.text = counter.count.ToString("00");
        animator.SetBool("Selected", counter.selected);
    }
}
