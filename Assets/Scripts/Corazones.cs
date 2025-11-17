using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corazones : MonoBehaviour
{


    [SerializeField] private Sprite llenos;
    [SerializeField] private Sprite vacios;

    private Image heart;

    void Start()
    {
        heart = GetComponent<Image>();
    }

    public void SetHeartFull()
    {
        heart.sprite = llenos;
    }

    public void SetHeartEmpty()
    {
        heart.sprite = vacios;
    }
   
}
