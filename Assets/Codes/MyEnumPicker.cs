using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnumPicker : MonoBehaviour
{
    public enum EnumType
    {
        A,
        B
    }

    public EnumType selectedEnumType;

    public enum EnumA
    {
        A1,
        A2,
        A3
    }

    public enum EnumB
    {
        B1,
        B2,
        B3
    }

    public EnumA selectedEnumA;
    public EnumB selectedEnumB;
}
