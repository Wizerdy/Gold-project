using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void TimeScaleToOne()
    {
        Time.timeScale = 1f;
    }

    public void TimeScaleToZero()
    {
        Time.timeScale = 0f;
    }
}
