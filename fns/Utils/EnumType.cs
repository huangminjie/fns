﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Utils
{
    public enum AdminStatusEnum
    {
        Normal = 0,
        Blocked = 1
    }
    public enum UserStatusEnum
    {
        Normal = 0,
        Blocked = 1
    }
    public enum ResponseCodeEnum
    {
        Success = 0,
        Failed = 1
    }
    public enum UserGenderEnum
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }
    public enum BannerRedirectTypeEnum
    {
        In = 1, //应用内
        Out = 2 //应用外
    }
    public enum TagEnum
    {
        Top = 1,//置顶 
        Hot = 2,//热点 
        Recommend = 3, //推荐 
        Special = 4//专题
    }

    public enum NewsStatusEnum
    {
        Normal = 0 //正常
    }
    public enum CommentStatusEnum
    {
        Normal = 0, //正常
        Illegal = 1, //违规
    }


    public enum PostStatusEnum
    {
        Normal = 0, //正常
        Deleted = 1, //被自己删除
        DeletedByAdmin = 2//被管理员删除
    }
    public enum PostCommentStatusEnum
    {
        Normal = 0, //正常
        Deleted = 1, //被自己删除
        DeletedByAdmin = 2//被管理员删除
    }
    public enum PostCommentReplyStatusEnum
    {
        Normal = 0, //正常
        Deleted = 1, //被自己删除
        DeletedByAdmin = 2//被管理员删除
    }
}
