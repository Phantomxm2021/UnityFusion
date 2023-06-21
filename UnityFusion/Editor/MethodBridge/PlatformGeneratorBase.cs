﻿using dnlib.DotNet;
using UnityFusion.Editor.ABI;
using UnityFusion.Editor.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TypeInfo = UnityFusion.Editor.ABI.TypeInfo;

namespace UnityFusion.Editor.MethodBridge
{
    public abstract class PlatformGeneratorBase
    {
        public abstract PlatformABI PlatformABI { get; }

        public abstract void GenerateManaged2NativeMethod(MethodDesc method, List<string> lines);

        public abstract void GenerateNative2ManagedMethod(MethodDesc method, List<string> lines);

        public abstract void GenerateAdjustThunkMethod(MethodDesc method, List<string> outputLines);


        public void GenerateManaged2NativeStub(List<MethodDesc> methods, List<string> lines)
        {
            lines.Add($@"
Managed2NativeMethodInfo unityfusion::interpreter::g_managed2nativeStub[] = 
{{
");

            foreach (var method in methods)
            {
                lines.Add($"\t{{\"{method.CreateInvokeSigName()}\", __M2N_{method.CreateInvokeSigName()}}},");
            }

            lines.Add($"\t{{nullptr, nullptr}},");
            lines.Add("};");
        }

        public void GenerateNative2ManagedStub(List<MethodDesc> methods, List<string> lines)
        {
            lines.Add($@"
Native2ManagedMethodInfo unityfusion::interpreter::g_native2managedStub[] = 
{{
");

            foreach (var method in methods)
            {
                lines.Add($"\t{{\"{method.CreateInvokeSigName()}\", (Il2CppMethodPointer)__N2M_{method.CreateInvokeSigName()}}},");
            }

            lines.Add($"\t{{nullptr, nullptr}},");
            lines.Add("};");
        }

        public void GenerateAdjustThunkStub(List<MethodDesc> methods, List<string> lines)
        {
            lines.Add($@"
NativeAdjustThunkMethodInfo unityfusion::interpreter::g_adjustThunkStub[] = 
{{
");

            foreach (var method in methods)
            {
                lines.Add($"\t{{\"{method.CreateInvokeSigName()}\", (Il2CppMethodPointer)__N2M_AdjustorThunk_{method.CreateCallSigName()}}},");
            }

            lines.Add($"\t{{nullptr, nullptr}},");
            lines.Add("};");
        }
    }
}
