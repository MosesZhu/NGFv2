//------------------------------------------------------------------------------
// File Name   : Ngf_Preference.cs
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
    /// 实体类Ngf_Preference
    /// </summary>
    [Serializable]
    public class Ngf_Preference : ITS.Data.EntityBase
    {
        public Ngf_Preference() : base("ngf_preference") { }

        #region Model
        private Guid _User_Id;
        private string _Language_Key;
        private string _Skin;
        private DateTime? _Created_At;
        private string _Created_By;
        private DateTime? _Modified_At;
        private string _Modified_By;
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
        public string Skin
        {
            get { return _Skin; }
            set
            {
                this.OnPropertyValueChange(_.Skin, _Skin, value);
                this._Skin = value;
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
				_.User_Id};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.User_Id,
				_.Language_Key,
				_.Skin,
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
				this._User_Id,
				this._Language_Key,
				this._Skin,
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
            this._User_Id = DataUtils.ConvertValue<Guid>(reader["user_id"]);
            this._Language_Key = DataUtils.ConvertValue<string>(reader["language_key"]);
            this._Skin = DataUtils.ConvertValue<string>(reader["skin"]);
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
            this._User_Id = DataUtils.ConvertValue<Guid>(row["user_id"]);
            this._Language_Key = DataUtils.ConvertValue<string>(row["language_key"]);
            this._Skin = DataUtils.ConvertValue<string>(row["skin"]);
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
            public readonly static Field All = new Field("*", "ngf_preference");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field User_Id = new Field("user_id", "ngf_preference", DbType.Guid, 16, "user_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Language_Key = new Field("language_key", "ngf_preference", DbType.String, 400, "language_key");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Skin = new Field("skin", "ngf_preference", DbType.String, 400, "skin");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_At = new Field("created_at", "ngf_preference", DbType.DateTime, 8, "created_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_By = new Field("created_by", "ngf_preference", DbType.String, 100, "created_by");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_At = new Field("modified_at", "ngf_preference", DbType.DateTime, 8, "modified_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_By = new Field("modified_by", "ngf_preference", DbType.String, 100, "modified_by");
        }
        #endregion


    }
}

