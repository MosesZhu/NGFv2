//------------------------------------------------------------------------------
// File Name   : Mc_Role_Function.cs
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
    /// 实体类Mc_Role_Function
    /// </summary>
    [Serializable]
    public class Mc_Role_Function : ITS.Data.EntityBase
    {
        public Mc_Role_Function() : base("mc_role_function") { }

        #region Model
        private Guid _Role_Id;
        private Guid _Function_Id;
        private DateTime? _Created_At;
        private string _Created_By;
        private DateTime? _Modified_At;
        private string _Modified_By;
        /// <summary>
        /// 
        /// </summary>
        public Guid Role_Id
        {
            get { return _Role_Id; }
            set
            {
                this.OnPropertyValueChange(_.Role_Id, _Role_Id, value);
                this._Role_Id = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Guid Function_Id
        {
            get { return _Function_Id; }
            set
            {
                this.OnPropertyValueChange(_.Function_Id, _Function_Id, value);
                this._Function_Id = value;
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
				_.Role_Id,
				_.Function_Id};
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
				_.Role_Id,
				_.Function_Id,
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
				this._Role_Id,
				this._Function_Id,
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
            this._Role_Id = DataUtils.ConvertValue<Guid>(reader["role_id"]);
            this._Function_Id = DataUtils.ConvertValue<Guid>(reader["function_id"]);
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
            this._Role_Id = DataUtils.ConvertValue<Guid>(row["role_id"]);
            this._Function_Id = DataUtils.ConvertValue<Guid>(row["function_id"]);
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
            public readonly static Field All = new Field("*", "mc_role_function");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Role_Id = new Field("role_id", "mc_role_function", DbType.Guid, 16, "role_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Function_Id = new Field("function_id", "mc_role_function", DbType.Guid, 16, "function_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_At = new Field("created_at", "mc_role_function", DbType.DateTime, 8, "created_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Created_By = new Field("created_by", "mc_role_function", DbType.String, 100, "created_by");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_At = new Field("modified_at", "mc_role_function", DbType.DateTime, 8, "modified_at");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Modified_By = new Field("modified_by", "mc_role_function", DbType.String, 100, "modified_by");
        }
        #endregion


    }
}

