using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script goes on the character model that is nested inside of the prefab. A parent object will control this along with equipping clothes
public class FacialAnimator : MonoBehaviour
{
    Renderer matRenderer;
    Material eyeMat;
    Material mouthMat;
    public Eyes currentEyes;
    public Mouth currentMouth;

    public float blinkInterval = 5f; // Time between blinks in seconds
    public float blinkDuration = 0.1f; // How long the blink lasts
    private float blinkTimer;
    private bool isBlinking = false;

    public enum Eyes 
    {
        Content,  //1,1.6758
        Closed,   //1,1.337 
        Squint,   //1.5156,1.337 
        Open,     //1.5156,1
        Furrowed  //1.5156,1.6758
    }
    public enum Mouth 
    {
        Closed,  //1,1.6758
        Cracked, //1,1.337
        Wide, //1.5156,1.337
        Open, //1.5156,1
        Ajar  //1.5156,1.6758
    }

    Eyes lastEyes;
    Mouth lastMouth;

    // Start is called before the first frame update
    void Start()
    {
        matRenderer = GetComponent<Renderer>();
        eyeMat = matRenderer.materials[1];
        mouthMat = matRenderer.materials[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEyes != lastEyes)
        {
            SetEyes(currentEyes);
            lastEyes = currentEyes;
        }

        if (currentMouth != lastMouth)
        {
            SetMouth(currentMouth);
            lastMouth = currentMouth;
        }

        Blinking();
    }

    void SetEyes(Eyes mode)
    {
        float xOffset = 1;
        float yOffset = 1;

        if (mode == Eyes.Content)
        {
            xOffset = 1f;
            yOffset = 1.6758f;
        }
        else if (mode == Eyes.Closed)
        {
            xOffset = 1f;
            yOffset = 1.337f;
        }
        else if (mode == Eyes.Squint)
        {
            xOffset = 1.5156f;
            yOffset = 1.337f;
        }
        else if (mode == Eyes.Open)
        {
            xOffset = 1.5156f;
            yOffset = 1f;
        }
        else if (mode == Eyes.Furrowed)
        {
            xOffset = 1.5156f;
            yOffset = 1.6758f;
        }

        eyeMat.mainTextureOffset = new Vector2(xOffset, yOffset);
    }
    void SetMouth(Mouth mode)
    {
        float xOffset = 1;
        float yOffset = 1;

        if (mode == Mouth.Closed)
        {
            xOffset = 1f;
            yOffset = 1.6758f;
        }
        else if (mode == Mouth.Cracked)
        {
            xOffset = 1f;
            yOffset = 1.337f;
        }
        else if (mode == Mouth.Wide)
        {
            xOffset = 1.5156f;
            yOffset = 1.337f;
        }
        else if (mode == Mouth.Open)
        {
            xOffset = 1.5156f;
            yOffset = 1f;
        }
        else if (mode == Mouth.Ajar)
        {
            xOffset = 1.5156f;
            yOffset = 1.6758f;
        }

        mouthMat.mainTextureOffset = new Vector2(xOffset, yOffset);
    }

    //this method can be called from other scripts to influence the NPC's emotion. idk if this is the best way to do this though
    //we need to establish how this system will actually be used. What causes the NPC to react? Time for a dialogue system...
    public void SetEmote(Eyes eyeMode, Mouth mouthMode)
    {
        currentEyes = eyeMode;
        currentMouth = mouthMode;
    }

    void Blinking()
    {
        blinkTimer += Time.deltaTime;

        if (!isBlinking && blinkTimer >= blinkInterval)
        {
            StartCoroutine(Blink());
            blinkTimer = 0f;
        }

        if (!isBlinking)
        {
            SetEyes(currentEyes);
            SetMouth(currentMouth);
        }
    }

    private IEnumerator Blink()
    {
        isBlinking = true;
        lastEyes = currentEyes;

        SetEyes(Eyes.Closed);
        yield return new WaitForSeconds(blinkDuration);

        SetEyes(lastEyes);
        isBlinking = false;
    }    
}
