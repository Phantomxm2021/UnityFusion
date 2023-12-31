﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityFusion.Editor.ABI
{
    public class TypeCreatorUniversal64 : TypeCreatorBase
    {
        public override bool IsArch32 => false;

        protected override TypeInfo OptimizeSigType(TypeInfo type, bool returnType)
        {
            if (type.PorType > ParamOrReturnType.STRUCTURE_ALIGN1 && type.PorType <= ParamOrReturnType.STRUCTURE_ALIGN8)
            {
                return new TypeInfo(ParamOrReturnType.STRUCTURE_ALIGN1, type.Size);
            }
            return type;
        }
    }
}
