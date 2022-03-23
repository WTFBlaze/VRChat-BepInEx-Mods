using BepInEx.IL2CPP;
using System;
using System.Collections;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace FaveStack.Utils
{
    public static class Coroutine
    {
        private static readonly CoroutineSupport support;

        static Coroutine()
        {
            support = IL2CPPChainloader.AddUnityComponent<CoroutineSupport>();
            ClassInjector.RegisterTypeInIl2Cpp<Wrapper>();
        }

        public static UnityEngine.Coroutine Start(IEnumerator coroutine) => support.StartCoroutine(new Il2CppSystem.Collections.IEnumerator(new Wrapper(coroutine).Pointer));
        public static void Stop(UnityEngine.Coroutine coroutine) => support.StopCoroutine(coroutine);


        private class CoroutineSupport : MonoBehaviour
        {
            public CoroutineSupport() : base(ClassInjector.DerivedConstructorPointer<CoroutineSupport>()) => ClassInjector.DerivedConstructorBody(this);
            public CoroutineSupport(IntPtr ptr) : base(ptr) { }
        }

        [Il2CppImplements(typeof(Il2CppSystem.Collections.IEnumerator))]
        private class Wrapper : Il2CppSystem.Object
        {
            private readonly IEnumerator enumerator;

            public Wrapper(IntPtr ptr) : base(ptr) { }
            public Wrapper(IEnumerator enumerator) : base(ClassInjector.DerivedConstructorPointer<Wrapper>())
            {
                ClassInjector.DerivedConstructorBody(this);
                this.enumerator = enumerator;
            }

            public Il2CppSystem.Object Current
            {
                get => enumerator.Current switch
                {
                    IEnumerator next => new Wrapper(next),
                    Il2CppSystem.Object il2cppObject => il2cppObject,
                    null => null,
                    _ => throw new NotSupportedException($"{enumerator.GetType()}: Unsupported type {enumerator.Current.GetType()}"),
                };
            }

            public bool MoveNext() => enumerator.MoveNext();
            public void Reset() => enumerator.Reset();
        }

    }
}
