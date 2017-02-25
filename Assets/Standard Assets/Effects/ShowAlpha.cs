using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ShowAlpha : MonoBehaviour {
	public Material mat;

	void Update () {
		if (!mat) {
			mat = new Material (Shader.Find ("Hidden/ShowAlpha"));
		}
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst) {
		if (mat) {
			Graphics.Blit (src, dst, mat);
		}
	}
}
