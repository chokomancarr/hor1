using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CDOF : MonoBehaviour {
	public Material mat;
	[Range(0.01f, 1)]
	public float aperture;
	[Range(0, 2)]
	public float amt;
	[Range(0, 0.5f)]
	public float cutoff;
	[Range(0.01f, 0.5f)]
	public float depthNearMul = 0.2f;
	[Range(0.01f, 1)]
	public float depthDiffMul = 0.01f;
	[Range(0, 2)]
	public float depthDiffOff = 1;

	public Transform focalPoint;
	[Range(0, 1)]
	public float focalVal = 0.1f;
	public bool visualize;

	Camera cam;

	void Update () {
		if (!mat) {
			mat = new Material (Shader.Find ("Hidden/DOF"));
		}
		else if (!cam) {
			cam = GetComponent<Camera> ();
		}
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst) {
		if (mat && cam) {
			mat.SetFloat ("_aper", aperture);
			mat.SetFloat ("_cut", cutoff);
			mat.SetFloat ("_depthNearMul", depthNearMul);
			mat.SetFloat ("_depthDiffMul", depthDiffMul);
			mat.SetFloat ("_depthDiffOff", depthDiffOff);
			mat.SetFloat ("pixelW", amt/Screen.width);
			mat.SetFloat ("pixelH", amt/Screen.height);
			if (focalPoint)
				focalVal = cam.WorldToScreenPoint (focalPoint.position).z / cam.farClipPlane;
			mat.SetFloat ("_focal", focalVal);
			RenderTexture tmpR = RenderTexture.GetTemporary (Screen.width, Screen.height);
			Graphics.Blit (src, tmpR, mat, 0);
			mat.SetTexture ("_DepthTex", tmpR);
			if (visualize)
				Graphics.Blit (tmpR, dst, mat, 2);
			else
				Graphics.Blit (src, dst, mat, 1);
			RenderTexture.ReleaseTemporary (tmpR);
		} 
	}
}
