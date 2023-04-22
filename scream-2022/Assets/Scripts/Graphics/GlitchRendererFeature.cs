using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlitchRendererFeature : ScriptableRendererFeature {

    [System.Serializable]
    public class GlitchSettings {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material MaterialToBlit;
    }
    
    public GlitchSettings settings = new GlitchSettings();

    RenderTargetHandle renderTextureHandle;
    GlitchPass glitchPass;

    public override void Create() {
        glitchPass = new GlitchPass(
          "Glitch pass",
          settings.WhenToInsert,
          settings.MaterialToBlit
        );
    }

    // called every frame once per camera
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (!settings.IsEnabled) {
            return;
        }
        
        var cameraColorTargetIdent = renderer.cameraColorTarget;
        glitchPass.Setup(cameraColorTargetIdent);
        
        renderer.EnqueuePass(glitchPass);
    }

    class GlitchPass : ScriptableRenderPass {
        
        string profilerTag;

        Material materialToBlit;
        RenderTargetIdentifier cameraColorTargetIdent;
        RenderTargetHandle tempTexture;

        public GlitchPass(string profilerTag,
          RenderPassEvent renderPassEvent, Material materialToBlit) {
            this.profilerTag = profilerTag;
            this.renderPassEvent = renderPassEvent;
            this.materialToBlit = materialToBlit;
        }
        
        public void Setup(RenderTargetIdentifier cameraColorTargetIdent) {
            this.cameraColorTargetIdent = cameraColorTargetIdent;
        }
        
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor) {
            cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData) {
            
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            cmd.Clear();
            
            // we apply our material while blitting to a temporary texture
            cmd.Blit(cameraColorTargetIdent, tempTexture.Identifier(), materialToBlit, 0);
            // ...then blit it back again 
            cmd.Blit(tempTexture.Identifier(), cameraColorTargetIdent);
            
            context.ExecuteCommandBuffer(cmd);
            
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
        
        public override void FrameCleanup(CommandBuffer cmd) {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }
}
