//------------------------------------------------------------------------------
// File Name   : Mc_System.cs
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
    /// 实体类Mc_System
    /// </summary>
    [Serializable]
    public class Mc_System : ITS.Data.EntityBase
    {
        public Mc_System() : base("mc_system") { }

        #region Model
        private Guid _Id;
        private string _Code;
        private string _Description;
        private string _Product_Id;
        private string _Domain_Id;
        private string _Language_Key;
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
        public string Description
        {
            get { return _Description; }
            set
            {
                this.OnPropertyValueChange(_.Description, _Description, value);
                this._Description = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Product_Id
        {
            get { return _Product_Id; }
            set
            {
                this.OnPropertyValueChange(_.Product_Id, _Product_Id, value);
                this._Product_Id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Domain_Id
        {
            get { return _Domain_Id; }
            set
            {
                this.OnPropertyValueChange(_.Domain_Id, _Domain_Id, value);
                this._Domain_Id = value;
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
				_.Description,
				_.Product_Id,
				_.Domain_Id,
				_.Language_Key,
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
				this._Description,
				this._Product_Id,
				this._Domain_Id,
				this._Language_Key,
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
            this._Description = DataUtils.ConvertValue<string>(reader["description"]);
            this._Product_Id = DataUtils.ConvertValue<string>(reader["product_id"]);
            this._Domain_Id = DataUtils.ConvertValue<string>(reader["domain_id"]);
            this._Language_Key = DataUtils.ConvertValue<string>(reader["language_key"]);
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
            this._Description = DataUtils.ConvertValue<string>(row["description"]);
            this._Product_Id = DataUtils.ConvertValue<string>(row["product_id"]);
            this._Domain_Id = DataUtils.ConvertValue<string>(row["domain_id"]);
            this._Language_Key = DataUtils.ConvertValue<string>(row["language_key"]);
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
            public readonly static Field All = new Field("*", "mc_system");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Id = new Field("id", "mc_system", DbType.Guid, 16, "id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Code = new Field("code", "mc_system", DbType.String, 100, "code");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Description = new Field("description", "mc_system", DbType.String, 500, "description");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Product_Id = new Field("product_id", "mc_system", DbType.String, 300, "product_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Domain_Id = new Field("domain_id", "mc_system", DbType.String, 300, "domain_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Language_Key = new Field("language_key", "mc_system", DbType.String, 400, "language_key");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_At = new Field("created_at", "mc_system", DbType.DateTime, 8, "created_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_By = new Field("created_by", "mc_system", DbType.String, 100, "created_by");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_At = new Field("modified_at", "mc_system", DbType.DateTime, 8, "modified_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_By = new Field("modified_by", "mc_system", DbType.String, 100, "modified_by");
        }
        #endregion


    }
}

