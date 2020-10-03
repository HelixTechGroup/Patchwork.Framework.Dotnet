#region Usings
using Microsoft.Win32;
#endregion

namespace Patchwork.Framework
{
    public class WindowsRegistry
    {
        #region Methods
        public static string GetValue(RegistryKey root, string path, string name)

        {
            try

            {
                var key = root.OpenSubKey(path);

                var result = (string)key.GetValue(name);

                key.Close();


                return result;
            }

            catch

            {
                return null;
            }
        }


        public static void SetValue(RegistryKey root, string path, string name, object value)

        {
            try

            {
                var key = root.OpenSubKey(path, true);

                key.SetValue(name, value);
            }

            catch { }
        }


        public static string GetValue(string path, string name)

        {
            return GetValue(Registry.CurrentUser, path, name);
        }


        public static void SetValue(string path, string name, object value)

        {
            SetValue(Registry.CurrentUser, path, name, value);
        }


        public static string GetValueLocalMachine(string path, string name)

        {
            return GetValue(Registry.LocalMachine, path, name);
        }


        public static void Delete(string path, string name)

        {
            try

            {
                var parentKey = Registry.CurrentUser.OpenSubKey(path, true);


                var key = parentKey.OpenSubKey(name);


                if (key != null) parentKey.DeleteSubKeyTree(name);
            }

            catch { }
        }

        public static void RemoveKey(string key)

        {
            try

            {
                Registry.CurrentUser.DeleteSubKeyTree(key);
            }

            catch { }
        }


        public static void RemoveKeyLocalMachine(string key)

        {
            try

            {
                Registry.LocalMachine.DeleteSubKeyTree(key);
            }

            catch { }
        }
        #endregion
    }
}