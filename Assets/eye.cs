using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eye : MonoBehaviour
{
    public MeshRenderer mEyeRenderer;

    private MeshRenderer mLidRenderer = null;
    private int mFrameCount = 8;

    void Awake()
    {
        mLidRenderer = GetComponent<MeshRenderer>();

    }

    void Start()
    {
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        float speed = 0.05f;

        while (gameObject.activeSelf)
        {
            //Time Between blinks
            yield return new WaitForSeconds(Random.Range(3, 8));

            //count up
            for(int i = 0; i <mFrameCount; i++)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetNewFrame(i);
            }

            //shift eye
            Vector2 eyeShift = new Vector2(Random.Range(-0.04f, 0.04f), Random.Range(-0.04f, 0.04f));
            mEyeRenderer.material.SetTextureOffset("_MainTex", eyeShift);

            //Wait
            yield return new WaitForSeconds(0.25f);

            //Count down
            for (int j = mFrameCount - 1; j >= 0; j--)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetNewFrame(j);
            }
        }
    }

    private void SetNewFrame(int frameIndex)
    {
        //Create new Offset, move texture
        Vector2 newOffset = new Vector2(frameIndex * (1.0f / mFrameCount), 0);
        mLidRenderer.material.SetTextureOffset("_MainTex", newOffset);
    }

}
