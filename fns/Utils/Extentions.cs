using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Utils;

namespace fns.Utils
{
    public static class Extentions
    {
        public static string ToDateTime(this DateTime? date)
        {
            return date == null ? "" : date?.ToString("yyyy-MM-dd HH:mm");
        }
        public static string ToDate(this DateTime? date)
        {
            return date == null ? "" : date?.ToString("yyyy-MM-dd");
        }
        public static string ToEnumValue(this ResponseCodeEnum responseCode)
        {
            switch (responseCode)
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
        public static string ToEnumText(this UserStatusEnum status)
        {
            switch (status)
            {
                case UserStatusEnum.Blocked:
                    return "黑名单";
                case UserStatusEnum.Normal:
                    return "正常";
                default:
                    return "正常";
            }
        }
        public static string ToEnumText(this UserGenderEnum gender)
        {

            switch (gender)
            {
                case UserGenderEnum.Female:
                    return "女";
                case UserGenderEnum.Male:
                    return "男";
                case UserGenderEnum.Unknown:
                    return "未知";
                default:
                    return "未知";
            }

        }
    }
}