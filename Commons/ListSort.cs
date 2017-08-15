using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Commons
{
    public class ListSort<T>
    {
        public static IList<T> GetList(IList<T> ilist,string sort, bool isDesc) 
        {
            List<T> list = (List<T>)ilist;
            ListSort<T> listSort = new ListSort<T>();
            list.Sort(
                delegate(T info1, T info2)
                {
                    object obj = info2.GetType().GetProperty(sort).GetValue(info2, null);
                    object obj2 = info1.GetType().GetProperty(sort).GetValue(info1, null);
                    string type = info1.GetType().GetProperty(sort).PropertyType.ToString();

                    if (isDesc == false)
                        return listSort.Compare(ref obj,ref obj2,ref type);
                    else
                        return listSort.Compare(ref obj2,ref obj,ref type);
                });            
            return ilist;
        }

        private int Compare(ref object obj1, ref object obj2,ref string type)
        {
            switch (type)
            {
                case "System.Decimal":
                    return decimal.Parse(obj1.ToString()).CompareTo(decimal.Parse(obj2.ToString()));                    
                    break;
                case "System.Single":
                    return Single.Parse(obj1.ToString()).CompareTo(Single.Parse(obj2.ToString()));
                    break;
                case "System.String":
                    return obj1.ToString().CompareTo(obj2.ToString());
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    return int.Parse(obj1.ToString()).CompareTo(int.Parse(obj2.ToString()));
                    break;
                case "System.DateTime":
                    return DateTime.Parse(obj1.ToString()).CompareTo(DateTime.Parse(obj2.ToString()));
                    break;
                case "System.Boolean":
                    return Boolean.Parse(obj1.ToString()).CompareTo(Boolean.Parse(obj2.ToString()));
                    break;
                default:
                    return 0;
                    break;
            }
        }
    }
}
