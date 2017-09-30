//------------------------------------------------------------------------------
// File Name   : Mc_Function.cs
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
    /// 实体类Mc_Function
    /// </summary>
    [Serializable]
    public class Mc_Function : ITS.Data.EntityBase
    {
        public Mc_Function() : base("mc_function") { }

        #region Model
        private Guid _Id;
        private string _Code;
        private string _System_Id;
        private string _Parent_Function_Id;
        private string _Language_Key;
        private string _Url;
        private DateTime? _Created_At;
        private string _Created_By;
        private DateTime? _Modified_At;
        private string _Modified_By;
        /// <summary>
        /// 
        /// </summary>
        public Guid Id
        {
            get { return _Id; }
            set
            {
                this.OnPropertyValueChange(_.Id, _Id, value);
                this._Id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Code
        {
            get { return _Code; }
            set
            {
                this.OnPropertyValueChange(_.Code, _Code, value);
                this._Code = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string System_Id
        {
            get { return _System_Id; }
            set
            {
                this.OnPropertyValueChange(_.System_Id, _System_Id, value);
                this._System_Id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Parent_Function_Id
        {
            get { return _Parent_Function_Id; }
            set
            {
                this.OnPropertyValueChange(_.Parent_Function_Id, _Parent_Function_Id, value);
                this._Parent_Function_Id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Language_Key
        {
            get { return _Language_Key; }
            set
            {
                this.OnPropertyValueChange(_.Language_Key, _Language_Key, value);
                this._Language_Key = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set
            {
                this.OnPropertyValueChange(_.Url, _Url, value);
                this._Url = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Created_At
        {
            get { return _Created_At; }
            set
            {
                this.OnPropertyValueChange(_.Created_At, _Created_At, value);
                this._Created_At = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Created_By
        {
            get { return _Created_By; }
            set
            {
                this.OnPropertyValueChange(_.Created_By, _Created_By, value);
                this._Created_By = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified_At
        {
            get { return _Modified_At; }
            set
            {
                this.OnPropertyValueChange(_.Modified_At, _Modified_At, value);
                this._Modified_At = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Modified_By
        {
            get { return _Modified_By; }
            set
            {
                this.OnPropertyValueChange(_.Modified_By, _Modified_By, value);
                this._Modified_By = value;
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
				_.Id};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.Id,
				_.Code,
				_.System_Id,
				_.Parent_Function_Id,
				_.Language_Key,
				_.Url,
				_.Created_At,
				_.Created_By,
				_.Modified_At,
				_.Modified_By};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._Id,
				this._Code,
				this._System_Id,
				this._Parent_Function_Id,
				this._Language_Key,
				this._Url,
				this._Created_At,
				this._Created_By,
				this._Modified_At,
				this._Modified_By};
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(IDataReader reader)
        {
            this._Id = DataUtils.ConvertValue<Guid>(reader["id"]);
            this._Code = DataUtils.ConvertValue<string>(reader["code"]);
            this._System_Id = DataUtils.ConvertValue<string>(reader["system_id"]);
            this._Parent_Function_Id = DataUtils.ConvertValue<string>(reader["parent_function_id"]);
            this._Language_Key = DataUtils.ConvertValue<string>(reader["language_key"]);
            this._Url = DataUtils.ConvertValue<string>(reader["url"]);
            this._Created_At = DataUtils.ConvertValue<DateTime?>(reader["created_at"]);
            this._Created_By = DataUtils.ConvertValue<string>(reader["created_by"]);
            this._Modified_At = DataUtils.ConvertValue<DateTime?>(reader["modified_at"]);
            this._Modified_By = DataUtils.ConvertValue<string>(reader["modified_by"]);
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(DataRow row)
        {
            this._Id = DataUtils.ConvertValue<Guid>(row["id"]);
            this._Code = DataUtils.ConvertValue<string>(row["code"]);
            this._System_Id = DataUtils.ConvertValue<string>(row["system_id"]);
            this._Parent_Function_Id = DataUtils.ConvertValue<string>(row["parent_function_id"]);
            this._Language_Key = DataUtils.ConvertValue<string>(row["language_key"]);
            this._Url = DataUtils.ConvertValue<string>(row["url"]);
            this._Created_At = DataUtils.ConvertValue<DateTime?>(row["created_at"]);
            this._Created_By = DataUtils.ConvertValue<string>(row["created_by"]);
            this._Modified_At = DataUtils.ConvertValue<DateTime?>(row["modified_at"]);
            this._Modified_By = DataUtils.ConvertValue<string>(row["modified_by"]);
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
            public readonly static Field All = new Field("*", "mc_function");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Id = new Field("id", "mc_function", DbType.Guid, 16, "id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Code = new Field("code", "mc_function", DbType.String, 100, "code");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field System_Id = new Field("system_id", "mc_function", DbType.String, 400, "system_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Parent_Function_Id = new Field("parent_function_id", "mc_function", DbType.String, 400, "parent_function_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Language_Key = new Field("language_key", "mc_function", DbType.String, 500, "language_key");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Url = new Field("url", "mc_function", DbType.String, -1, "url");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_At = new Field("created_at", "mc_function", DbType.DateTime, 8, "created_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_By = new Field("created_by", "mc_function", DbType.String, 100, "created_by");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_At = new Field("modified_at", "mc_function", DbType.DateTime, 8, "modified_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_By = new Field("modified_by", "mc_function", DbType.String, 100, "modified_by");
        }
        #endregion


    }
}

