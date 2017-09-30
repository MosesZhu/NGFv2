using System;
using System.Collections.Generic;
using System.Text;
/*****************************************************************************************************
Author        : Anson.Lin
Date	      : 27th Jan, 2006
Description   : 
/*****************************************************************************************************/
namespace WSC.Common
{
    /// <summary>
    ///
    /// </summary>
    public class CommonEnum
    {
        //Created by Anson Lin on 27th Jan, 2006


        public enum MailType
        {
            ToUser = 1,
            ToAdmin = 2,
            Error = 3,
            Information = 4,
            GetPassword = 5
        };


        /// <summary>
        /// The action type of Log.
        /// </summary>
        public enum LogActionType
        {
            /// <summary>
            /// 
            /// </summary>
            Add = 1,
            /// <summary>
            /// 
            /// </summary>
            Update = 2,
            /// <summary>
            /// 
            /// </summary>
            Delete = 3,
            /// <summary>
            /// 
            /// </summary>
            Get = 4,
            /// <summary>
            /// 
            /// </summary>
            Error = 5,
            /// <summary>
            /// Information
            /// </summary>
            Info = 6,
            /// <summary>
            /// 
            /// </summary>
            Success = 7,
            /// <summary>
            /// 
            /// </summary>
            Report = 8,
            /// <summary>
            /// 
            /// </summary>
            Query = 9,
            /// <summary>
            /// Log in to system
            /// </summary>
            Login = 10
        };



        public enum EditActionType
        {
            New = 1,
            Update = 2,
            Delete = 3

        };

        public enum Culture
        {
            EN,
            SC,
            TC
        };



        public enum WindowTarget
        {
            _top,
            _blank,
            _self,
            _parent,
            _media,
            _search
        }

        /// <summary>
        /// DIV message type
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// 
            /// </summary>
            Loading,
            /// <summary>
            /// 
            /// </summary>
            Working,
            /// <summary>
            /// 
            /// </summary>
            Processing,
            /// <summary>
            /// 
            /// </summary>
            Saving,
            /// <summary>
            /// 
            /// </summary>
            Cancelling,
            /// <summary>
            /// 
            /// </summary>
            Deleting
        };

        /// <summary>
        /// 含异常的公共基类
        /// by Hedda
        /// </summary>
        public class Error
        {
            #region
            private string _lastError;

            public virtual string LastError
            {
                get
                {
                    return _lastError;
                }
                set
                {
                    _lastError = value;
                }
            }

            #endregion
        }

    }
}
