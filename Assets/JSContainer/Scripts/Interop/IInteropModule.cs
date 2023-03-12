using System;
using UnityEngine.Assertions;

namespace JSContainer.Interop
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InteropModule : Attribute
    {
        public readonly bool AlwaysOn;
        public readonly string ItemName;

        public InteropModule(string itemName, bool alwaysOn = false)
        {
            Assert.IsTrue(!string.IsNullOrEmpty(itemName), "Item name is mandatory");
            ItemName = itemName;
            AlwaysOn = alwaysOn;
        }
    }
}