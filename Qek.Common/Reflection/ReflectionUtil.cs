using System;
using System.Reflection;

namespace Qek.Common.Reflection
{
    public class ReflectionUtil
    {
        public static void SetPropertyValue(object obj, string propertyName, object value)
        {
            PropertyInfo info = obj.GetType().GetProperty(propertyName);
            if (info != null)
            {
                info.SetValue(obj, Convert.ChangeType(value, info.PropertyType), null);
            }
        }

        public static void SetFieldValue(object obj, string fieldName, object value)
        {
            FieldInfo info = obj.GetType().GetField(fieldName);
            if (info != null)
            {
                info.SetValue(obj, Convert.ChangeType(value, info.FieldType));
            }
        }

        public static Type GetType(string TypeName, bool throwIfNotfound)
        {
            //用"類別名稱"搜尋是否有此型別
            Type type = Type.GetType(TypeName, false);
            if ((type != null))
            {
                return type;
            }

            //找不到此類別名稱,就從應用系統內開始比對所有Assembly
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Console.WriteLine(assembly.FullName);
                //每個Assembly都搜尋是否有此"類別名稱"
                type = assembly.GetType(TypeName, false);
                if ((type != null))
                {
                    break;
                }
            }

            if (throwIfNotfound && type == null)
            {
                throw new Exception(string.Format("TypeName : {0} not found", TypeName));
            }

            return type;
        }
    }
}
