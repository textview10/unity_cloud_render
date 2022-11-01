

namespace SRDebugger.Scripts
{
    using System;
    using System.Linq;
    using SRF;
    using UI.Other;
    using UnityEngine;

    public class StudioDebuggerTabController : SRMonoBehaviourEx
    {
        private SRTab _aboutTabInstance;
        private DefaultTabs? _activeTab;
        private bool _hasStarted;
        public SRTab AboutTab;

        
        [RequiredField] public SRTabController TabController;

        public DefaultTabs? ActiveTab
        {
            get
            {
                var key = TabController.ActiveTab.Key;

                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                var t = Enum.Parse(typeof (DefaultTabs), key);

                if (!Enum.IsDefined(typeof (DefaultTabs), t))
                {
                    return null;
                }

                return (DefaultTabs) t;
            }
        }

        protected override void Start()
        {
            base.Start();

            _hasStarted = true;

            // Loads all available tabs from resources
            var tab = Resources.Load<SRTab>("SRDebugger/UI/Prefabs/Tabs/Console");
            var defaultTabs = Enum.GetNames(typeof (DefaultTabs));

            
                var enabler = tab.GetComponent(typeof (IEnableTab)) as IEnableTab;

                if (enabler != null && !enabler.IsEnabled)
                {
                    return;
                }

                if (defaultTabs.Contains(tab.Key))
                {
                    var tabValue = Enum.Parse(typeof (DefaultTabs), tab.Key);

                    if (Enum.IsDefined(typeof (DefaultTabs), tabValue) &&
                        Settings.Instance.DisabledTabs.Contains((DefaultTabs) tabValue))
                    {
                        return;
                    }
                }

                var t = SRInstantiate.Instantiate(tab);
                TabController.AddTab(t);

            // Add about tab (has no button, accessed via "Stompy" logo
            if (AboutTab != null)
            {
                _aboutTabInstance = SRInstantiate.Instantiate(AboutTab);
                TabController.AddTab(_aboutTabInstance, false);
            }

            // Open active tab (set before panel loaded), or default tab from settings
            var defaultTab = _activeTab ?? Settings.Instance.DefaultTab;

            if (!OpenTab(defaultTab))
            {
                TabController.ActiveTab = TabController.Tabs.FirstOrDefault();
            }
        }


        public bool OpenTab(DefaultTabs tab)
        {
            if (!_hasStarted)
            {
                _activeTab = tab;
                return true;
            }

            var tabName = tab.ToString();

            foreach (var t in TabController.Tabs)
            {
                if (t.Key == tabName)
                {
                    TabController.ActiveTab = t;
                    return true;
                }
            }

            return false;
        }

        public void ShowAboutTab()
        {
            if (_aboutTabInstance != null)
            {
                TabController.ActiveTab = _aboutTabInstance;
            }
        }
    }
}
