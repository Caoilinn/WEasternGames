using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritesheetAnimation : MonoBehaviour
{
    public int frameCount = 8; // number of cells in the spritesheet
    public float framesPerSecond = 12; // frames per second
    public string textureNameInShader;
    public bool animationActive = true;
    public MeshRenderer meshRenderer;

    public Image imageComponent; // this is for 2d image

    private IEnumerator coroutine;

    void Start() {
        //test
        StartAnimating();
    }

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
        if (meshRenderer) { meshRenderer.sharedMaterial.SetTextureOffset(textureNameInShader, newOffset); }
        if (imageComponent) { imageComponent?.material.SetTextureOffset(textureNameInShader, newOffset); }
    }

    public void SetSpritesheetTexture(Texture2D texture, int frameCount, float framesPerSecond) {
        // set theframe count and speed
        this.frameCount = frameCount;
        this.framesPerSecond = framesPerSecond;
        this.animationActive = true;

        // for 3d objects
        if (meshRenderer) { 
            // set new main texture
            meshRenderer.sharedMaterial.SetTexture(textureNameInShader, texture); 

            // reset to be the first frame and re calculate tiling
            meshRenderer.sharedMaterial.SetTextureOffset(textureNameInShader, new Vector2(0,0));
            meshRenderer.sharedMaterial.SetTextureScale(textureNameInShader, new Vector2(1.0f/frameCount, 1));
        }

        // for 2D UI
        if (imageComponent) {
            // set new main texture
            imageComponent.material.SetTexture(textureNameInShader, texture);

            // reset to be the first frame and re calculate tiling
            imageComponent?.material.SetTextureOffset(textureNameInShader, new Vector2(0,0));
            imageComponent?.material.SetTextureScale(textureNameInShader, new Vector2(1.0f/frameCount, 1));
        }
    }
}
