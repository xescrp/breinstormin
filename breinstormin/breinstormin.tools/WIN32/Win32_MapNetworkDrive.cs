using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public class Win32_MapNetworkDrive
    {
        //Windows API
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2A(ref structNetResource pstNetRes, 
            string psPassword, string psUsername, int piFlags);
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2A(string psName, int piFlags, int pfForce);
        [DllImport("mpr.dll")]
        private static extern int WNetConnectionDialog(int phWnd, int piType);
        [DllImport("mpr.dll")]
        private static extern int WNetDisconnectDialog(int phWnd, int piType);
        [DllImport("mpr.dll")]
        private static extern int WNetRestoreConnectionW(int phWnd, string psLocalDrive);

        [StructLayout(LayoutKind.Sequential)]
        private struct structNetResource
        {
            public int iScope;
            public int iType;
            public int iDisplayType;
            public int iUsage;
            public string sLocalName;
            public string sRemoteName;
            public string sComment;
            public string sProvider;
        }

        private const int RESOURCETYPE_DISK = 0x1;

        //Standard	
        private const int CONNECT_INTERACTIVE = 0x00000008;
        private const int CONNECT_PROMPT = 0x00000010;
        private const int CONNECT_UPDATE_PROFILE = 0x00000001;
        //IE4+
        private const int CONNECT_REDIRECT = 0x00000080;
        //NT5 (solo para NT5)
        private const int CONNECT_COMMANDLINE = 0x00000800;
        private const int CONNECT_CMD_SAVECRED = 0x00001000;
        

        //Implementacion
        private bool lf_SaveCredentials = false;
		
		// Salvar credenciales
		public bool SaveCredentials
        {
			get{ return(lf_SaveCredentials); }
			set{ lf_SaveCredentials=value; }
		}
		private bool lf_Persistent = false;
		
		// opcion de reconectar la unidad despues de un reboot o desconexion ...
		public bool Persistent{
			get{ return(lf_Persistent); }
			set{ lf_Persistent = value; }
		}
		private bool lf_Force = false;

		// opcion para forzar la conexion si ya existe la unidad mapeada,
		// o fuerza la desconexion si no responde la ruta de red
		public bool Force
        {
			get{return(lf_Force);}
			set{ lf_Force = value; }
		}
		private bool ls_PromptForCredentials = false;

		// opcion para solicitar credenciales al mapear
		public bool PromptForCredentials
        {
			get{ return(ls_PromptForCredentials); }
			set{ ls_PromptForCredentials = value; }
		}

        
		private string ls_Drive = "z:";
        // unidad a ser mapeada
		public string LocalDrive
        {
			get{return(ls_Drive);}
			set{
				if(value.Length>=1)
                {
					ls_Drive = value.Substring(0,1) + ":";
				}else
                {
					ls_Drive = "";
				}
			}
		}
		private string ls_ShareName = "\\\\Computer\\C$";
        //ruta a mapear
		public string ShareName
        {
			get{ return(ls_ShareName); }
			set{ ls_ShareName = value; }
		}
		

		
		//Mapear unidad
		public void MapDrive()
        {
            zMapDrive(null, null);
        }
		//Mapear unidad sustituyendo password
		public void MapDrive(string Password)
        {
            zMapDrive(null, Password);
        }
		//Mapear unidad con usuario y password
		public void MapDrive(string Username, string Password)
        {
            zMapDrive(Username, Password);
        }
		//Deshacer mapeo de unidad
		public void UnMapDrive()
        {
            zUnMapDrive(this.lf_Force);
        }
		//Check / restore una unidad mapeada permanente
		public void RestoreDrives()
        {
            zRestoreDrive();
        }
			

		// Mapear unidad
		private void zMapDrive(string psUsername, string psPassword)
        {
			//generamos la nueva estructura de datos
			structNetResource stNetRes = new structNetResource();			
			stNetRes.iScope = 2;
			stNetRes.iType = RESOURCETYPE_DISK;
			stNetRes.iDisplayType = 3;
			stNetRes.iUsage = 1;
			stNetRes.sRemoteName = ls_ShareName;
			stNetRes.sLocalName = ls_Drive;			
			//preparar parametros
			int iFlags = 0;
			if(lf_SaveCredentials){ iFlags += CONNECT_CMD_SAVECRED; }
			if(lf_Persistent){ iFlags += CONNECT_UPDATE_PROFILE; }
			if(ls_PromptForCredentials){ iFlags += CONNECT_INTERACTIVE + CONNECT_PROMPT; }
			if(psUsername == ""){ psUsername = null; }
			if(psPassword == ""){ psPassword = null; }
			//si forzamos, unmap preparada para nueva conexion
			if(lf_Force){ try{ zUnMapDrive(true); } catch{} }
			//llamada y retorno
			int i = WNetAddConnection2A(ref stNetRes, psPassword, psUsername, iFlags);			
			if(i>0){ throw new System.ComponentModel.Win32Exception(i); }						
		}

		// Deshace el mapeo de la unidad
		private void zUnMapDrive(bool pfForce)
        {
			//llamada a unmap y retorno
			int iFlags=0;
			if(lf_Persistent){iFlags+=CONNECT_UPDATE_PROFILE;}
			int i = WNetCancelConnection2A(ls_Drive, iFlags, Convert.ToInt32(pfForce));
			if(i>0){throw new System.ComponentModel.Win32Exception(i);}
		}

		// Check / Restore una unidad mapeada
		private void zRestoreDrive()
        {
			//Llamada a restore y retorno
			int i = WNetRestoreConnectionW(0, null);
			if(i>0){ throw new System.ComponentModel.Win32Exception(i); }
		}
		
        
        


    }
}




