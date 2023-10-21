using System;
using UnityEngine;

[ExecuteAlways]
public class BlurScript : MonoBehaviour
{
    [SerializeField] private RenderTexture _renderTexture;
    
    private void Update()
    {
        _renderTexture = Blur(_renderTexture, 10);  
    }

    RenderTexture Blur(RenderTexture source, int iterations) 
    {
        RenderTexture result = source; //result will store partial results (blur iterations)
        Material mat = new Material(Shader.Find("Blur")); //create blur material
        RenderTexture blit = RenderTexture.GetTemporary(Screen.width,Screen.height); //get temp RT
        for (int i = 0; i < iterations; i++) {
            Graphics.SetRenderTarget(blit);
            GL.Clear(true, true, Color.black); //avoid artifacts in temp RT by clearing it
            Graphics.Blit(result, blit, mat); //PERFORM A BLUR ITERATION
            result= blit; //overwrite partial result
        }
        RenderTexture.ReleaseTemporary(blit);
        return result; //return the last partial result
    }
}
