using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public bool rotateEnabled = false;  // Public boolean variable to enable/disable rotation
    public float rotationSpeed = 5f;    // Rotation speed
    public float growshrinkRate = .05f;

    // Update is called once per frame
    void Update()
    {
        if (rotateEnabled)
        {
            RotateGameObject();
        }
    }

    void RotateGameObject()
    {
        // Update the rotation based on the rotation speed
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotate the object locally around its Y axis
        transform.Rotate(Vector3.up, rotationAmount);
    }

    public IEnumerator RevealFishModelCoroutine()
    {
        //gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        while (gameObject.transform.localScale.x < 1f)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + growshrinkRate, gameObject.transform.localScale.y + growshrinkRate, gameObject.transform.localScale.z + growshrinkRate);
     
            yield return null;
        }
        rotateEnabled = true;
    }
    public IEnumerator HideFishModelCoroutine()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        while (gameObject.transform.localScale.x > 0f)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - growshrinkRate, gameObject.transform.localScale.y - growshrinkRate, gameObject.transform.localScale.z - growshrinkRate);
     
            yield return null;
        }
        rotateEnabled = false;
    }
}
