﻿Shader "Scream2021/GlitchEffect" {
    Properties {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] _UniversalEnable("Universal enable", Float) = 0
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _Alpha("Alpha", Float) = 1.0
        _XFade("Fade", Range(0, 1)) = 0.0
        
        [Space(25)][MaterialToggle] _CSplitEnabled(" === CMYK Split === ", Float) = 0.0
        _CSplitRate("Rate", Range(0, 2)) = .5
        _CSplitStart("Start depth", Range(0, 100)) = 10
        _CSplitMax("Max split depth", Range(0, 100)) = 40
        _CSplitFix("Fixed split depth", Range(0, .1)) = .01
        
        [Space(25)][MaterialToggle] _DLimitEnabled(" === Depth limit === ", Float) = 0.0
        _DLimitDepthTex("Depth tex", 2D) = "white" {}
        _DLimitMin("Min depth", Range(0, 100)) = 10
        _DLimitMax("Max depth", Range(0, 100)) = 20
        _DLimitFar("Far plane", Range(0, 1000)) = 100
        
        [Space(25)][MaterialToggle] _HDispEnabled(" === Horizontal Displacement === ", Float) = 0.0
        [MaterialToggle] _HDispSloppyPower("Sloppy power", Float) = 0
        _HDispChance("Chance", Range(0, 1)) = 0.5
        _HDispPower("Power", Range(0, 1)) = 0.5
        _HDispPowerVariance("Power variance", Range(0, 1)) = 0.5
        _HDispChunking("Chunk size", Range(0, 1)) = 0.5
        _HDispChunkingVariance("Chunk size variance", Range(0, 1)) = 0.5
        
        
        [Space(25)][MaterialToggle] _HBleedEnabled(" === Horizontal Streaking === ", Float) = 0
        [MaterialToggle] _HBleedAlphaRestrict("Restrict to alpha", Float) = 0
        _HBleedChance("Chance", Range(0, 1)) = 0.5
        _HBleedChunking("Chunk size", Range(0, 1)) = 0.5
        _HBleedChunkingVariance("Chunk size variance", Range(0, 1)) = 0.5
        _HBleedTailing("Tail length", Range(0, 1)) = 0.5
        _HBleedTailingVariance("Tail length variance", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _SFrameEnabled(" === Static Frames === ", Float) = 0
        [MaterialToggle] _SFrameAlphaIncluded("Include alpha regions", Float) = 0
        _SFrameChance("Chance", Range(0, 1)) = 0.5
        _SFrameChunking("Chunk size", Range(0, 1)) = 0.5
        _SFrameChunkingVariance("Chunk size variance", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _PDistEnabled(" === Palette Distortion === ", Float) = 0
        [MaterialToggle] _PDistAlphaIncluded("Include alpha regions", Float) = 0
        [MaterialToggle] _PDistSimultaneousInvert("Synchronized inversion", Float) = 0
        _PDistInvertR("R inversion chance", Range(0, 1)) = 0.0
        _PDistInvertG("G inversion chance", Range(0, 1)) = 0.0
        _PDistInvertB("B inversion chance", Range(0, 1)) = 0.0
        _PDistMaxR("R max chance", Range(0, 1)) = 0.0
        _PDistMaxG("G max chance", Range(0, 1)) = 0.0
        _PDistMaxB("B max chance", Range(0, 1)) = 0.0
        _PDistMonocolorChance("Monocolor chance", Range(0, 1)) = 0.0
        _PDistMonocolor("Monocolor", Color) = (1.0, 1.0, 1.0, 1.0)
        
        [Space(25)][MaterialToggle] _RDispEnabled(" === Rectangular Displacement === ", Float) = 0.0
        [MaterialToggle] _RDispCopyOnly("Keep source region intact", Range(0, 1)) = 0.0
        [MaterialToggle] _RDispInvertSource("Invert source background", Range(0, 1)) = 0.0
        [MaterialToggle] _RDispKeepAlpha("Preserve source alpha", Range(0, 1)) = 1.0
        _RDispTex("Background texture", 2D) = "black" {}
        _RDispChance("Chance", Range(0, 1)) = 0.5
        _RDispChunkXSize("Chunk X Size", Range(0, 1)) = 0.5
        _RDispChunkYSize("Chunk Y size", Range(0, 1)) = 0.5
        _RDispChunkVariance("Chunk size variance", Range(0, 1)) = 0.5
        _RDispMinPowerX("Displacement min dist X", Range(-1, 1)) = -0.5
        _RDispMaxPowerX("Displacement max dist X", Range(-1, 1)) = 0.5
        _RDispMinPowerY("Displacement min dist Y", Range(-1, 1)) = -0.5
        _RDispMaxPowerY("Displacement max dist Y", Range(-1, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _VSyncEnabled(" === VSync === ", Float) = 0.0
        _VSyncPowerMin("Min jitter power", Range(-1, 1)) = -0.5
        _VSyncPowerMax("Max jitter power", Range(-1, 1)) = 0.5
        _VSyncJitterChance("Jitter chance", Range(0, 1)) = 0.5
        _VSyncJitterDuration("Jitter duration", Range(0, 1)) = 0.5
        _VSyncChance("Loop chance", Range(0, 1)) = 0.5
        _VSyncDuration("Loop duration", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _SShiftEnabled(" === Scanline Shift === ", Float) = 0.0
        _SShiftChance("Chance", Range(0, 1)) = .5
        _SShiftPowerMin("Min power", Range(0, 1)) = 0.25
        _SShiftPowerMax("Max power", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _TDistEnabled(" === Traveling Distortion === ", Float) = 0.0
        [MaterialToggle] _TDistTailoff("Linear tailoff", Range(0, 1)) = 1
        [MaterialToggle] _TDistExcludeAlpha("Exclude alpha regions", Range(0, 1)) = 0
        _TDistChance("Chance", Range(0, 1)) = .5
        _TDistDuration("Duration", Range(0, 1)) = .5
        _TDistChunking("Chunk height", Range(0, 1)) = .5
        _TDistStaticBarSize("Static effect height", Range(0, 1)) = .5
        _TDistStaticSize("Static chunk size", Range(0, 1)) = .5
        [MaterialToggle] _TDistHDisp("Horizontal displacement enabled", Range(0, 1)) = 0
        _TDistHDispPower("Displacement power", Range(0, 1)) = .5
        _TDistHDispPowerVariance("Displacement power variance", Range(0, 1)) = .5
        _TDistColorBarSize("Color effect height", Range(0, 1)) = 0
        [MaterialToggle] _TDispPreserveBrightness("Color preserve brightness", Range(0, 1)) = 0
        [MaterialToggle] _TDistInvertR("Color invert R", Range(0, 1)) = 0
        [MaterialToggle] _TDistInvertG("Color invert G", Range(0, 1)) = 0
        [MaterialToggle] _TDistInvertB("Color invert B", Range(0, 1)) = 0
        
        [Space(25)][MaterialToggle] _SColorEnabled(" === Scanline Coloring === ", Float) = 0.0
        [MaterialToggle] _SColorExcludeAlpha("Exclude alpha regions", Range(0, 1)) = 1.0
        _SColorChance("Chance",  Range(0, 1)) = 0.5
        _SColorVelocity("Vertical velocity",  Range(-1, 1)) = 0.0
        _SColorGap("Gap",  Range(0, 1)) = 0.05
        _SColorBrightness("Brightness change",  Range(-1, 1)) = 0.0
        [MaterialToggle] _SColorBleed("Full bleed", Range(0, 1)) = 0.0
        [MaterialToggle] _SColorStatic("Scanline static", Range(0, 1)) = 0.0
        
        [Space(25)][MaterialToggle] _CClampEnabled(" === Color Channel Clamping === ", Float) = 0.0
        _CClampBrightness("Pre-brightness boost",  Range(-1, 1)) = 0.0
        [MaterialToggle] _CClampBlack("Always include true black/white", Float) = 1.0
        _CClampR("R shades allowed",  Range(0, 1)) = 1.0
        _CClampG("G shades allowed",  Range(0, 1)) = 1.0
        _CClampB("B shades allowed",  Range(0, 1)) = 1.0
        [MaterialToggle] _CClampDither("Dithering enabled", Float) = 0.0
        [MaterialToggle] _CClampDitherVary("Varied dithering enabled", Float) = 0.0
        _CClampDitherChunk("Dithering chunk width", Range(0, 1)) = 0.5
        _CClampJitterR("R colors jitter power",  Range(0, 1)) = 0.0
        _CClampJitterG("G colors jitter power",  Range(0, 1)) = 0.0
        _CClampJitterB("B colors jitter power",  Range(0, 1)) = 0.0
        _CClampMaxDitherJump("Max dither jump", Range(0, 1)) = 0.0

        [Space(25)][MaterialToggle] _PEdgeEnabled(" === Pulsing Edge === ", Float) = 0.0
        [MaterialToggle] _PEdgeUseWaveSource("Use wave source", Float) = 0.0
        _PEdgeDuration("Duration",  Range(0, 1)) = 0.5
        _PEdgeDepthMin("Depth Min",  Range(0, 1)) = 0.0
        _PEdgeDepthMax("Depth Max",  Range(0, 1)) = 0.5
        _PEdgePower("Power", Range(0, 1)) = 1.0
        _PEdgeAmplitude("Amplitude",  Range(0, 1)) = 0.5
        _PEdgeDistanceGrain("Distance Granularity", Range(0, 1)) = 1.0
    }
    
    SubShader {
    
        Tags {
            "Queue"="Transparent"
        }

        Pass {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            
            #pragma target 3.0
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Glitch.cginc"
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            
            float _UniversalEnable;
            float _Alpha, _XFade;
            float _DLimitEnabled, _DLimitMin, _DLimitMax, _DLimitFar;
            sampler2D _DLimitDepthTex;

            v2f vert(appdata_t IN) {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
#endif

                return OUT;
            }
            
            fixed4 frag(v2f IN) : SV_Target {
                float2 xy = IN.texcoord;
                float4 pxXY = IN.vertex;
                float depth = tex2D(_DLimitDepthTex, xy) * _DLimitFar;
                fixed4 orig = tex2D(_MainTex, xy);
                fixed4 c;
                if (_UniversalEnable) {
                    c = glitchFragFromCoords(xy, pxXY, depth);
                } else {
                    c = orig;
                }
                
                if (_DLimitEnabled > 0 && _UniversalEnable) {
                    float blend = saturate(smoothstep(_DLimitMin, _DLimitMax, depth));
                    c = (1 - blend) * orig + (blend) * c;
                }
                c -= fixed4(_XFade, _XFade, _XFade, 0);
                
                c *= IN.color;
                return c;
            }

        ENDCG
        }
    }
}
