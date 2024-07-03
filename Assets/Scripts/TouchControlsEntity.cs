using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsEntity : MonoBehaviour
{
    public static TouchControls instance;

    private void Awake()
    {
        instance = new TouchControls();
    }
}
