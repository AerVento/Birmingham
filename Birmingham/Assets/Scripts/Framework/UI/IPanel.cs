using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public interface IPanel 
    {
        public bool IsShowing {get;}

        public void Show();

        public void Hide();
    }
}

