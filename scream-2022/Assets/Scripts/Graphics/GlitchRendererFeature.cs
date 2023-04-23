using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class GlitchRendererFeature : ScriptableRendererFeature {
    public float m_Intensity;

    public Material m_Material;

    ColorBlitPass m_RenderPass = null;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (renderingData.cameraData.cameraType == CameraType.Game) {
            //Calling ConfigureInput with the ScriptableRenderPassInput.Color argument ensures that the opaque texture is available to the Render Pass
            m_RenderPass.ConfigureInput(ScriptableRenderPassInput.Color);
            m_RenderPass.SetTarget(renderer.cameraColorTarget, m_Intensity);
            renderer.EnqueuePass(m_RenderPass);
        }
    }

    public override void Create() {

        m_RenderPass = new ColorBlitPass(m_Material);
    }

    protected override void Dispose(bool disposing) {

    }

    internal class ColorBlitPass : ScriptableRenderPass {
        ProfilingSampler m_ProfilingSampler = new ProfilingSampler("ColorBlit");
        Material m_Material;
        RenderTargetIdentifier m_CameraColorTarget;
        float m_Intensity;

        public ColorBlitPass(Material material) {
            m_Material = material;
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        }

        public void SetTarget(RenderTargetIdentifier colorHandle, float intensity) {
            m_CameraColorTarget = colorHandle;
            m_Intensity = intensity;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) {
            ConfigureTarget(new RenderTargetIdentifier(m_CameraColorTarget, 0, CubemapFace.Unknown, -1));
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            var camera = renderingData.cameraData.camera;
            if (camera.cameraType != CameraType.Game)
                return;

            if (m_Material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, m_ProfilingSampler)) {
                m_Material.SetFloat("_Intensity", m_Intensity);
                cmd.SetRenderTarget(new RenderTargetIdentifier(m_CameraColorTarget, 0, CubemapFace.Unknown, -1));
                //The RenderingUtils.fullscreenMesh argument specifies that the mesh to draw is a quad.
                cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m_Material);


            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }
    }
}