//------------------------------------------------------------------------------
// File Name   : Mc_Token.cs
// Creator     : Moses.Zhu
// Create Date : 2017-03-31
// Description : 此代码由工具生成，请不要人为更改代码，如果重新生成代码后，这些更改将会丢失。
// Copyright (C) 2017 Qisda Corporation. All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ITS.Data;
using ITS.Data.Common;

namespace NGF.Model.Entity
{

    /// <summary>
    /// 实体类Mc_Token
    /// </summary>
    [Serializable]
    public class Mc_Token : ITS.Data.EntityBase
    {
        public Mc_Token() : base("mc_token") { }

        #region Model
        private Guid _User_Id;
        private DateTime _Login_Time;
        private string _Secret_Key;
        /// <summary>
        /// 
        /// </summary>
        public Guid User_Id
        {
            get { return _User_Id; }
            set
            {
                this.OnPropertyValueChange(_.User_Id, _User_Id, value);
                this._User_Id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Login_Time
        {
            get { return _Login_Time; }
            set
            {
                this.OnPropertyValueChange(_.Login_Time, _Login_Time, value);
                this._Login_Time = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Secret_Key
        {
            get { return _Secret_Key; }
            set
            {
                this.OnPropertyValueChange(_.Secret_Key, _Secret_Key, value);
                this._Secret_Key = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
				_.User_Id};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.User_Id,
				_.Login_Time,
				_.Secret_Key};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._User_Id,
				this._Login_Time,
				this._Secret_Key};
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(IDataReader reader)
        {
            this._User_Id = DataUtils.ConvertValue<Guid>(reader["user_id"]);
            this._Login_Time = DataUtils.ConvertValue<DateTime>(reader["login_time"]);
            this._Secret_Key = DataUtils.ConvertValue<string>(reader["secret_key"]);
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(DataRow row)
        {
            this._User_Id = DataUtils.ConvertValue<Guid>(row["user_id"]);
            this._Login_Time = DataUtils.ConvertValue<DateTime>(row["login_time"]);
            this._Secret_Key = DataUtils.ConvertValue<string>(row["secret_key"]);
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// * 
            /// </summary>
            public readonly static Field All = new Field("*", "mc_token");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field User_Id = new Field("user_id", "mc_token", DbType.Guid, 16, "user_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Login_Time = new Field("login_time", "mc_token", DbType.DateTime, 8, "login_time");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Secret_Key = new Field("secret_key", "mc_token", DbType.String, 1000, "secret_key");
        }
        #endregion


    }
}

