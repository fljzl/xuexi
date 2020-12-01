using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using System.Text;

namespace Cheng.Comon
{
    public class ReflectHelper
    {
        /// <summary>
        /// 反射获取Type
        /// </summary>
        /// <param name="assemblyNamePath">D://demo.dll</param>
        /// <param name="className">nameplace.classname</param>
        /// <returns></returns>
        public static Type GetType(string assemblyNamePath, string className)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyNamePath.Trim());
            return assembly.GetType(className.Trim(), true, true);
        }
    }
}
