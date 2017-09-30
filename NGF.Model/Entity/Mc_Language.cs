//------------------------------------------------------------------------------
// File Name   : Ngf_Language.cs
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
    /// 实体类Ngf_Language
    /// </summary>
    [Serializable]
    public class Ngf_Language : ITS.Data.EntityBase
    {
        public Ngf_Language() : base("ngf_language") { }

        #region Model
        private string _Language_Key;
        private string _Zh_Cn;
        private string _Zh_Tw;
        private string _En_Us;
        private DateTime? _Createed_At;
        private string _Created_By;
        private DateTime? _Modified_At;
        private string _Modified_By;
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
        public string Zh_Cn
        {
            get { return _Zh_Cn; }
            set
            {
                this.OnPropertyValueChange(_.Zh_Cn, _Zh_Cn, value);
                this._Zh_Cn = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Zh_Tw
        {
            get { return _Zh_Tw; }
            set
            {
                this.OnPropertyValueChange(_.Zh_Tw, _Zh_Tw, value);
                this._Zh_Tw = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string En_Us
        {
            get { return _En_Us; }
            set
            {
                this.OnPropertyValueChange(_.En_Us, _En_Us, value);
                this._En_Us = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Createed_At
        {
            get { return _Createed_At; }
            set
            {
                this.OnPropertyValueChange(_.Createed_At, _Createed_At, value);
                this._Createed_At = value;
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
				_.Language_Key};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.Language_Key,
				_.Zh_Cn,
				_.Zh_Tw,
				_.En_Us,
				_.Createed_At,
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
				this._Language_Key,
				this._Zh_Cn,
				this._Zh_Tw,
				this._En_Us,
				this._Createed_At,
				this._Created_By,
				this._Modified_At,
				this._Modified_By};
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(IDataReader reader)
        {
            this._Language_Key = DataUtils.ConvertValue<string>(reader["language_key"]);
            this._Zh_Cn = DataUtils.ConvertValue<string>(reader["zh_cn"]);
            this._Zh_Tw = DataUtils.ConvertValue<string>(reader["zh_tw"]);
            this._En_Us = DataUtils.ConvertValue<string>(reader["en_us"]);
            this._Createed_At = DataUtils.ConvertValue<DateTime?>(reader["createed_at"]);
            this._Created_By = DataUtils.ConvertValue<string>(reader["created_by"]);
            this._Modified_At = DataUtils.ConvertValue<DateTime?>(reader["modified_at"]);
            this._Modified_By = DataUtils.ConvertValue<string>(reader["modified_by"]);
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(DataRow row)
        {
            this._Language_Key = DataUtils.ConvertValue<string>(row["language_key"]);
            this._Zh_Cn = DataUtils.ConvertValue<string>(row["zh_cn"]);
            this._Zh_Tw = DataUtils.ConvertValue<string>(row["zh_tw"]);
            this._En_Us = DataUtils.ConvertValue<string>(row["en_us"]);
            this._Createed_At = DataUtils.ConvertValue<DateTime?>(row["createed_at"]);
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
            public readonly static Field All = new Field("*", "ngf_language");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Language_Key = new Field("language_key", "ngf_language", DbType.String, 400, "language_key");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Zh_Cn = new Field("zh_cn", "ngf_language", DbType.String, -1, "zh_cn");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Zh_Tw = new Field("zh_tw", "ngf_language", DbType.String, -1, "zh_tw");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field En_Us = new Field("en_us", "ngf_language", DbType.String, -1, "en_us");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Createed_At = new Field("createed_at", "ngf_language", DbType.DateTime, 8, "createed_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_By = new Field("created_by", "ngf_language", DbType.String, 100, "created_by");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_At = new Field("modified_at", "ngf_language", DbType.DateTime, 8, "modified_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_By = new Field("modified_by", "ngf_language", DbType.String, 100, "modified_by");
        }
        #endregion


    }
}

