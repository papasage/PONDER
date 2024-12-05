using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteReeler : MonoBehaviour
{
    private float stickAngle;

    private void FixedUpdate()
    {
        stickAngle = ControllerInputManager.instance.leftStickAngle;

        transform.rotation = Quaternion.Euler(0, 0, stickAngle);
    }
}
