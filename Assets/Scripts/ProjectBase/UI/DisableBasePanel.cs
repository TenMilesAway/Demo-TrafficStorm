using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cyber
{
    /// <summary>
    /// ��������Ļ������
    /// </summary>
    public class DisableBasePanel : BasePanel
    {

        public override void ShowMe()
        {
            base.ShowMe();

            //GameDataMgr.GetInstance().GetPlayer().Input.OnDisable();
        }

        public override void HideMe()
        {
            base.HideMe();

            //GameDataMgr.GetInstance().GetPlayer().Input.OnEnable();
        }
    }
}
