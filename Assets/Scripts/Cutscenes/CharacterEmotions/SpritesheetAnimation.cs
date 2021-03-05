using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesheetAnimation : MonoBehaviour
{
    public int frameCount = 8; // number of cells in the spritesheet
    public float framesPerSecond = 12; // frames per second
    public string textureNameInShader;
    public bool animationActive = true;
    public MeshRenderer meshRenderer;

    private IEnumerator coroutine;

    public void StartAnimating() {
        if (coroutine == null) { coroutine = Animate();}
        StopCoroutine(coroutine);
        coroutine = Animate();
        StartCoroutine(coroutine);
    }

    public void StopAnimating() {
        this.animationActive = false;
    }

    private IEnumerator Animate()
    {
        while (animationActive)
        {
            //count up
            for(int i = 0; i <frameCount; i++)
            {
                //Debug.Log("Frame: " + i);
                SetNewFrame(i);
                yield return new WaitForSeconds(1/framesPerSecond);
            }
        }
    }

    private void SetNewFrame(int frameIndex)
    {
        //Create new Offset, move texture
        Vector2 newOffset = new Vector2(frameIndex * (1.0f / frameCount), 0);
        meshRenderer.sharedMaterial.SetTextureOffset(textureNameInShader, newOffset);
    }

    public void SetSpritesheetTexture(Texture2D texture, int frameCount, float framesPerSecond) {
        // set the main texture in the material
        meshRenderer.sharedMaterial.SetTexture(textureNameInShader, texture);

        // set theframe count and speed
        this.frameCount = frameCount;
        this.framesPerSecond = framesPerSecond;
        this.animationActive = true;

        // reset offset and re calculate tiling on the X (based on number of frames)
        meshRenderer.sharedMaterial.SetTextureOffset(textureNameInShader, new Vector2(0,0));
        meshRenderer.sharedMaterial.SetTextureScale(textureNameInShader, new Vector2(1.0f/frameCount, 1));
    }

}
