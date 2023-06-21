using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityFusion.Editor.ABI
{
    public static class ABIUtil
    {
        public static string GetUnityFusionPlatformMacro(PlatformABI abi)
        {
            switch(abi)
            {
                case PlatformABI.Arm64: return "UNITYFUSION_ABI_ARM_64";
                case PlatformABI.Universal64: return "UNITYFUSION_ABI_UNIVERSAL_64";
                case PlatformABI.Universal32: return "UNITYFUSION_ABI_UNIVERSAL_32";
                case PlatformABI.WebGL32: return "UNITYFUSION_ABI_WEBGL32";
                default: throw new NotSupportedException();
            }
        }
    }
}
