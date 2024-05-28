using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour
{
    public Select select;
    public AEnum enumA;
    public BEnum enumB;

    public enum Select
    {
        A,
        B
    }

    public enum AEnum
    {
        One,
        Two
    }

    public enum BEnum
    {
        Five,
        Six
    }
}
