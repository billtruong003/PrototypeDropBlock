using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FishFlock
{
    public class CustomTitle : PropertyAttribute
    {
        public string title;
        public CustomTitle(string title)
        {
            this.title = title;
        }
    }
}