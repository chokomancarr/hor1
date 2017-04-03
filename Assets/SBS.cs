using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SBS : MonoBehaviour {

	public Material mat;

	void OnRenderImage (RenderTexture i, RenderTexture o) {
		Graphics.Blit (i, o, mat);
	}
}
