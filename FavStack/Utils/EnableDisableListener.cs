using System;
using System.Runtime.CompilerServices;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace FavStack.Utils
{
    [NullableContext(2)]
    [Nullable(0)]
    public class EnableDisableListener : MonoBehaviour
    {
        public EnableDisableListener(IntPtr id) : base(id) { }

        [method: HideFromIl2Cpp]
        public event Action OnEnabled;

        [method: HideFromIl2Cpp]
        public event Action OnDisabled;

        private void OnEnable()
        {
            var onEnabled = OnEnabled;
            if (onEnabled == null)
            {
                return;
            }
            onEnabled();
        }

        private void OnDisable()
        {
            var onDisabled = OnDisabled;
            if (onDisabled == null)
            {
                return;
            }
            onDisabled();
        }
    }

    [CompilerGenerated]
    [Embedded]
    public sealed class EmbeddedAttribute : Attribute
    {
    }

    [CompilerGenerated]
    [Embedded]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
    public sealed class NullableAttribute : Attribute
    {
        public readonly byte[] NullableFlags;

        public NullableAttribute(byte[] A_1)
        {
            NullableFlags = A_1;
        }

        public NullableAttribute(byte A_1)
        {
            NullableFlags = new[]
            {
                A_1
            };
        }
    }

    [CompilerGenerated]
    [Embedded]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    class NullableContextAttribute : Attribute
    {
        public readonly byte Flag;
        public NullableContextAttribute(byte A_1)
        {
            Flag = A_1;
        }
    }
}
