using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Tools.WIN32
{
    public interface IWmiAccess
    {
        int InvokeInstanceMethod(string machineName, string className, string name, string methodName);

        /// <summary>
        /// Llamada a una instacia de WMI en la maquina remota, invocando un metodo de la clase WMI
        /// </summary>
        /// <param name="machineName">Nombre de la maquina donde ejecutar el metodo</param>
        /// <param name="className">La clase WMI a invocar</param>
        /// <param name="name">Nombre de la instancia WMI</param>
        /// <param name="methodName">Nombre del metodo WMI</param>
        /// <param name="parameters">Parametros del metodo</param>
        /// <returns>un codigo devuelto por el metodo invocado en la clase WMI</returns>
        int InvokeInstanceMethod(string machineName, string className, string name, string methodName, object[] parameters);

        int InvokeStaticMethod(string machineName, string className, string methodName);

        /// <summary>
        /// Llamada a un metodo de una clase WMI
        /// </summary>
        /// <param name="machineName">Nombre de la maquina donde ejecutar el metodo</param>
        /// <param name="className">La clase WMI a invocar</param>
        /// <param name="methodName">Nombre del metodo WMI</param>
        /// <param name="parameters">Parametros del metodo</param>
        /// <returns>un codigo devuelto por el metodo invocado en la clase WMI</returns>
        int InvokeStaticMethod(string machineName, string className, string methodName, object[] parameters);
    }

}
