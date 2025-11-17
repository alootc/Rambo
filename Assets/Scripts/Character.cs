using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int hearts = 3;

    public virtual void TakeDamage()
    {
        // Este método será sobrescrito por Player y Enemy
        hearts = Mathf.Clamp(hearts - 1, 0, hearts);
    }
}