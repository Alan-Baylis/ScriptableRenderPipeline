using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    // RenderRenderPipelineSettings represent settings that are immutable at runtime.
    // There is a dedicated RenderRenderPipelineSettings for each platform

    [Serializable]
    public class GlobalLightLoopSettings
    {
        public int spotCookieSize = 128;
        public int cookieTexArraySize = 16;
        public int pointCookieSize = 512;
        public int cubeCookieTexArraySize = 16;

        public int reflectionProbeCacheSize = 128;
        public int reflectionCubemapSize = 128;
        public bool reflectionCacheCompressed = false;
    }
}



