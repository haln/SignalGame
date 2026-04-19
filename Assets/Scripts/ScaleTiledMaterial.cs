using UnityEngine;

[ExecuteAlways]
public class ScaleTiledMaterial : MonoBehaviour
{
	[SerializeField] private Vector2 textureWorldSize = Vector2.one;
	private MaterialPropertyBlock _block;
	private Renderer _renderer;

	void OnEnable()
	{
		_renderer = GetComponent<Renderer>();
		_block = new MaterialPropertyBlock();
		UpdateTiling();
	}

	void Update()
	{
#if UNITY_EDITOR
		UpdateTiling();
#endif
	}

	void UpdateTiling()
	{
		Vector2 tiling = new Vector2(
			transform.lossyScale.x / textureWorldSize.x,
			transform.lossyScale.y / textureWorldSize.y
		);
		_block.SetVector("_BaseMap_ST", new Vector4(tiling.x, tiling.y, 0, 0));
		_renderer.SetPropertyBlock(_block);
	}
}