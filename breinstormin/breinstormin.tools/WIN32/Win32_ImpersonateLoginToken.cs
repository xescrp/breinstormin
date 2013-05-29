using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_ImpersonateLoginToken
    {
        string _domain_name;
        string _user_name;
        string _password;
        string _error;
        bool _ok;

        System.Security.Principal.WindowsIdentity _win_identity;
        System.Security.Principal.WindowsImpersonationContext _win_identity_context;

        IntPtr _main_token;

        public bool LogonSuccess { get { return _ok; } }
        public string LogonError { get { return _error; } }

        public string DomainName { get { return _domain_name; } }
        public string UserName { get { return _user_name; } }

        public IntPtr TokenPointer { get { return _main_token; } }

        public Win32_ImpersonateLoginToken(string user, string password, string domain) 
        {
            _domain_name = domain;
            _user_name = user;
            _password = password;
            _get_access();
        }

        public void Undo() 
        {
            try
            {
                Win32API.CloseHandle(_main_token);
                _win_identity_context.Undo();
            }
            catch (Exception ex) { }
        }

        private void _get_access() 
        {
            IntPtr _token = new IntPtr();
            if (Win32API.LogonUser(_user_name, _domain_name, _password,
                Win32API.LogonTypes.NewCredentials,
                Win32API.LogonProviders.Default,
                out _token))
            {
                _ok = true;
                Win32API.DuplicateToken(_token, 2, ref _main_token);
                _win_identity = new System.Security.Principal.WindowsIdentity(_main_token);
                _win_identity_context = _win_identity.Impersonate();
            }
            else
            {
                _ok = false; 
                try
                {
                    _error =
                        System.Runtime.InteropServices.Marshal.GetLastWin32Error().ToString();
                }
                catch (Exception ex) { }
            }
        }

    }
}
