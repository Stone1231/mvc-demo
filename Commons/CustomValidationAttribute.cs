using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class MustHaveOne : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) {
                return false;
            }

            var typeName = value.GetType().Name;

            switch (typeName)
            {
                case "String[]":
                    var array1 = value as string[];
                    if (array1 != null)
                    {
                        return array1.Length > 0;
                    }
                    break;
                case "Int32[]":
                    var array2 = value as int[];
                    if (array2 != null)
                    {
                        return array2.Length > 0;
                    }
                    break;
                default:
                    var list = value as IList;
                    if (list != null)
                    {
                        return list.Count > 0;
                    }
                    break;
            }

            return false;
        }
    }
}
