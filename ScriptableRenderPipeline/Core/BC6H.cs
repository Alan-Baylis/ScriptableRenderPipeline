﻿using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
    public class BC6H
    {
        public static BC6H DefaultInstance;

        static readonly int _Source = Shader.PropertyToID("_Source");
        static readonly int _Target = Shader.PropertyToID("_Target");

        readonly ComputeShader m_Shader;
        readonly int m_KernelEncodeFast;
        readonly int[] m_KernelEncodeFastGroupSize;

        public BC6H(ComputeShader shader)
        {
            Assert.IsNotNull(shader);

            m_Shader = shader;
            m_KernelEncodeFast = m_Shader.FindKernel("KEncodeFast");

            uint x, y, z;
            m_Shader.GetKernelThreadGroupSizes(m_KernelEncodeFast, out x, out y, out z);
            m_KernelEncodeFastGroupSize = new[] { (int)x, (int)y, (int)z };
        }

        // Only use mode11 of BC6H encoding
        public void EncodeFast(CommandBuffer cmb, RenderTargetIdentifier source, int sourceWidth, int sourceHeight, RenderTargetIdentifier target)
        {
            int targetWidth, targetHeight;
            CalculateOutputSize(sourceWidth, sourceHeight, out targetWidth, out targetHeight);

            cmb.SetComputeTextureParam(m_Shader, m_KernelEncodeFast, _Source, source);
            cmb.SetComputeTextureParam(m_Shader, m_KernelEncodeFast, _Target, target);
            cmb.DispatchCompute(m_Shader, m_KernelEncodeFast, targetWidth, targetHeight, 1);
        }

        static void CalculateOutputSize(int swidth, int sheight, out int twidth, out int theight)
        {
            // BC6H encode 4x4 blocks of 32bit in 128bit
            twidth = swidth >> 2;
            theight = sheight >> 2;
        }
    }

    public static class BC6HExtensions
    {
        public static void BC6HEncodeFast(this CommandBuffer cmb, RenderTargetIdentifier source, int sourceWidth, int sourceHeight, RenderTargetIdentifier target)
        {
            BC6H.DefaultInstance.EncodeFast(cmb, source, sourceWidth, sourceHeight, target);
        }
    }
}