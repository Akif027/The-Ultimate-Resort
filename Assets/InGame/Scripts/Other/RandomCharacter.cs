using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacter : MonoBehaviour
{
    [SerializeField] private Animator animator;  // Reference to the Animator

    private void OnEnable()
    {
        EnableRandomChild();
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    private void EnableRandomChild()
    {
        int childCount = transform.childCount;

        if (childCount == 0) return;

        // Disable all children first
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        // Pick a random index
        int randomIndex = Random.Range(0, childCount);

        // Enable the randomly selected child
        GameObject selectedChild = transform.GetChild(randomIndex).gameObject;
        selectedChild.SetActive(true);

        // Try to get the Animator component from the active child
        Animator childAnimator = selectedChild.GetComponent<Animator>();
        if (childAnimator != null)
        {
            animator = childAnimator;
        }
        else
        {
            Debug.LogWarning("No Animator found on the selected child model.");
        }
    }
}
