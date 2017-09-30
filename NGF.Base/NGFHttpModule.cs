using ITS.WebFramework.SSO.SSOModule;
using System;
using System.Web;

namespace NGF.Base
{
    public class NGFHttpModule : IHttpModule
    {
        public void Dispose() { }
        public void Init(HttpApplication context)
        {
            context.BeginRequest += ngfProcess;
        }

        private static void ngfProcess(Object source, EventArgs args)
        {            
            SSOModule wfkSSOModule = new SSOModule();
            wfkSSOModule.BeginRequest(source, args);
        }
    }
}