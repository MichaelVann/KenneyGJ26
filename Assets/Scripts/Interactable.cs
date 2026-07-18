using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject _prompt;

    internal void SetPromptActive(bool a_active) { _prompt.SetActive(a_active); }

    internal virtual void Awake()
    {
        SetPromptActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    internal virtual void Start()
    {

    }

    // Update is called once per frame
    internal virtual void Update()
    {

    }

    internal virtual void Interact()
    {

    }
}


