using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;

namespace DefaultNamespace
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(Camera))]
	public class RenderTo2D : MonoBehaviour
	{
		private MeshFilter m_MeshFilter;
		private MeshRenderer m_MeshRenderer;
		
		private int2 m_LastScreenDimension;

		private RenderTexture  m_RenderTexture;
		private Camera         m_Camera;

		public RenderTextureDescriptor RenderDescriptor;
		public Material Material;
		
		public LayerMask  RenderMask;
		public GameObject Target;

		private List<Renderer> m_TargetRenderer = new List<Renderer>();
		private Mesh m_PlaneMesh;

		private Material m_InstancedMaterial;

		private void OnEnable()
		{
			if (RenderDescriptor.width == 0)
			{
				RenderDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.DefaultHDR);
			}

			m_MeshFilter = GetComponent<MeshFilter>();
			m_MeshRenderer = GetComponent<MeshRenderer>();
			m_Camera                        = GetComponent<Camera>();
			m_Camera.useOcclusionCulling = false;
			m_Camera.forceIntoRenderTexture = true;
			m_Camera.cullingMask            = RenderMask;

			m_InstancedMaterial = Instantiate(Material);
			m_MeshRenderer.sharedMaterial = m_InstancedMaterial;

			m_PlaneMesh = new Mesh
			{
				vertices = new[]
				{
					new Vector3(-0.5f, -0.5f, 0),
					new Vector3(0.5f, -0.5f, 0),
					new Vector3(0.5f, 0.5f, 0),
					new Vector3(-0.5f, 0.5f, 0),
				},
				uv = new[]
				{
					new Vector2(0, 0),
					new Vector2(1, 0),
					new Vector2(1, 1),
					new Vector2(0, 1)
				},
				triangles = new[] {0, 1, 2, 0, 2, 3}
			};
			m_PlaneMesh.RecalculateNormals();
			m_MeshFilter.mesh = m_PlaneMesh;

			FinalizeRenderer();
			UpdateRenderTexture();

			RenderPipelineManager.beginFrameRendering  += BeginFrameRendering;
			RenderPipelineManager.beginCameraRendering += BeginCameraRendering;
			RenderPipelineManager.endCameraRendering   += EndCameraRending;
			RenderPipelineManager.endFrameRendering    += EndFrameRending;
		}

		private void BeginFrameRendering(ScriptableRenderContext arg1, Camera[] arg2)
		{
			foreach (var renderer in m_TargetRenderer) renderer.enabled = false;
		}

		private void BeginCameraRendering(ScriptableRenderContext arg1, Camera cam)
		{
			if (cam != m_Camera)
				return;

			foreach (var renderer in m_TargetRenderer) renderer.enabled = true;
		}

		private void EndCameraRending(ScriptableRenderContext arg1, Camera cam)
		{
			if (cam != m_Camera)
				return;
			
			foreach (var renderer in m_TargetRenderer) renderer.enabled = false;
		}

		private void EndFrameRending(ScriptableRenderContext arg1, Camera[] arg2)
		{
			if (!Application.isPlaying)
				return;

			//foreach (var renderer in m_TargetRenderer) renderer.enabled = true;
		}

		private void UpdateRenderTexture()
		{
			if (m_RenderTexture == null)
				m_RenderTexture = new RenderTexture(RenderDescriptor);
			
			m_RenderTexture.Release();


			RenderDescriptor.width = Screen.width;
			RenderDescriptor.height = Screen.height;
			RenderDescriptor.msaaSamples = 8;
			
			m_RenderTexture.descriptor = RenderDescriptor;
			m_RenderTexture.Create();

			m_InstancedMaterial.SetTexture("_MainTex", m_RenderTexture);
			m_Camera.targetTexture = m_RenderTexture;

			if (!Target) return;

			RecursiveTransformLayer(Target.transform);
		}

		private void RecursiveTransformLayer(Transform tr)
		{
			foreach (Transform child in tr)
			{
				var renderer = child.GetComponent<Renderer>();
				if (renderer)
				{
					renderer.renderingLayerMask = (uint) RenderMask.value;
				}

				RecursiveTransformLayer(child);
			}
		}

		private void FinalizeRenderer()
		{
			void Recursive(Transform tr)
			{
				var childCount = tr.childCount;
				for (var i = 0; i != childCount; i++)
				{
					var child    = tr.GetChild(i);
					var renderer = child.GetComponent<Renderer>();

					if (renderer) m_TargetRenderer.Add(renderer);

					Recursive(child);
				}
			}

			m_TargetRenderer.Clear();
			Recursive(Target.transform);
		}

		private void LateUpdate()
		{
			var worldScreenHeight = m_Camera.orthographicSize * 2;
			var worldScreenWidth  = worldScreenHeight / Screen.height * Screen.width;

			if (m_LastScreenDimension.x != Screen.width && m_LastScreenDimension.y != Screen.height)
			{
				m_LastScreenDimension.x = Screen.width;
				m_LastScreenDimension.y = Screen.height;

				UpdateRenderTexture();
			}

			var pos = transform.position;

			transform.position = pos;

			//var size = m_SpriteRenderer.sprite.bounds.size;
			//m_SpriteRenderer.size = new Vector2(worldScreenWidth, worldScreenHeight);
			/*transform.localScale = new Vector3(
				worldScreenWidth / size.x,
				worldScreenHeight / size.y, 1);*/
		}

		private void OnDisable()
		{
			DestroyImmediate(m_PlaneMesh);
			DestroyImmediate(m_InstancedMaterial);
			
			m_PlaneMesh = null;

			RenderPipelineManager.beginFrameRendering  -= BeginFrameRendering;
			RenderPipelineManager.beginCameraRendering -= BeginCameraRendering;
			RenderPipelineManager.endCameraRendering   -= EndCameraRending;
			RenderPipelineManager.endFrameRendering    -= EndFrameRending;
		}
	}
}