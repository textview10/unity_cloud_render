namespace SRDebugger.UI
{
    using Scripts;
    using Services;
    using SRF;
    using SRF.Service;
    using UnityEngine;

    public class DebugPanelRoot : SRMonoBehaviourEx
    {
        [RequiredField] public Canvas Canvas;

        [RequiredField] public CanvasGroup CanvasGroup;

        [RequiredField] public DebuggerTabController TabController;

        public void Close()
        {
            #if STUDIO || STUDIO_CLIENT || STUDIO_SERVER
                gameObject.SetActive(false);
                SRServiceManager.GetService<IDebugService>().HideDebugPanel();
                SRServiceManager.GetService<IDebugPanelService>().IsVisible = false;
                SRServiceManager.GetService<IDebugPanelService>().Unload();
#else
                 if (Settings.Instance.UnloadOnClose)
            {
                SRServiceManager.GetService<IDebugService>().DestroyDebugPanel();
            }
            else
            {
                SRServiceManager.GetService<IDebugService>().HideDebugPanel();
            }
#endif

        }

        public void CloseAndDestroy()
        {
            SRServiceManager.GetService<IDebugService>().DestroyDebugPanel();
        }
    }
}
