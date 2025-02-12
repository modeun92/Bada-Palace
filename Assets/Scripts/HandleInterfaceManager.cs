using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Assets.Scripts
{
    class HandleInterfaceManager : MonoBehaviour, IPointerUpHandler
    {
        public Scrollbar scrollbar;
        float prevVal = 0f;
        private AudioSource source;
        private bool isRighthanded;
        private void Start()
        {
            source = GetComponent<AudioSource>();
            bool isRighthanded;
            //GameMainSettingManager.GetUserHandStatic(out isRighthanded);
            ConfigManager.GetValue(ConfigParameter.IS_RIGHTHANDED, out isRighthanded);
            if (isRighthanded)
            {
                scrollbar.value = 1f;
            }
            else
            {
                scrollbar.value = 0f;
            }
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            source.Play();
            var val = scrollbar.value;
            if (prevVal == val)
            {
                return;
            }
            if (val == 0f)
            {
                Debug.Log("Left");
                isRighthanded = false;
            }
            else if (val == 1f)
            {
                Debug.Log("Right");
                isRighthanded = true;
            }
            else
            {
                Debug.Log("None Value");
            }
            prevVal = val;
        }
        public void Apply()
        {
            //GameMainSettingManager.SetUserHandStatic(isRighthanded);
            ConfigManager.SetValue(ConfigParameter.IS_RIGHTHANDED, isRighthanded);
        }
    }
}
