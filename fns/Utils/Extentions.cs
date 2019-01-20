using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Utils;

namespace fns.Utils
{
    public static class Extentions
    {
        public static string ToEnumValue(this ResponseCodeEnum responseCode)
        {
            switch(responseCode)
            {
                case ResponseCodeEnum.Success:
                    return "0000";
                case ResponseCodeEnum.Failed:
                    return "0001";
                default:
                    return "0001";
            }
        }
        public static string ToEnumText(this ResponseCodeEnum responseCode)
        {
            switch (responseCode)
            {
                case ResponseCodeEnum.Success:
                    return "成功！";
                case ResponseCodeEnum.Failed:
                    return "失败！";
                default:
                    return "失败！";
            }
        }
    }
}
