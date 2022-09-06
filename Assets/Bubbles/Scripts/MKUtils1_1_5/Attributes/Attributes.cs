using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class NamedArrayAttribute : PropertyAttribute
    {
        public readonly string[] names;
        public NamedArrayAttribute(string[] names) { this.names = names; }
    }

    public class ArrayElementTitleAttribute : PropertyAttribute
    {
        public string Varname;
        public ArrayElementTitleAttribute(string ElementTitleVar)
        {
            Varname = ElementTitleVar;
        }
    }

    public class ShowOnlyAttribute : PropertyAttribute
    {
        // https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    }

    public class ReadOnlyAttribute : PropertyAttribute
    {
        // https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    }

}
