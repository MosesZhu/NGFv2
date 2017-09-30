//------------------------------------------------------------------------------
// File Name   : Itemtype.cs
// Creator     : Moses.Zhu
// Create Date : 2017-03-09
// Description : 此代码由工具生成，请不要人为更改代码，如果重新生成代码后，这些更改将会丢失。
// Copyright (C) 2017 Qisda Corporation. All rights reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ITS.Data;
using ITS.Data.Common;

namespace NGF.Demo.Model.Entity
{

    /// <summary>
    /// 实体类Itemtype
    /// </summary>
    [Serializable]
    public class Itemtype : ITS.Data.EntityBase
    {
        public Itemtype() : base("ItemType") { }

        #region Model
        private Guid _Id;
        private string _Name;
        private string _Description;
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
        public string Name
        {
            get { return _Name; }
            set
            {
                this.OnPropertyValueChange(_.Name, _Name, value);
                this._Name = value;
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
				_.Name,
				_.Description};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._Id,
				this._Name,
				this._Description};
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(IDataReader reader)
        {
            this._Id = DataUtils.ConvertValue<Guid>(reader["id"]);
            this._Name = DataUtils.ConvertValue<string>(reader["name"]);
            this._Description = DataUtils.ConvertValue<string>(reader["description"]);
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(DataRow row)
        {
            this._Id = DataUtils.ConvertValue<Guid>(row["id"]);
            this._Name = DataUtils.ConvertValue<string>(row["name"]);
            this._Description = DataUtils.ConvertValue<string>(row["description"]);
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
            public readonly static Field All = new Field("*", "ItemType");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Id = new Field("id", "ItemType", DbType.Guid, 16, "id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Name = new Field("name", "ItemType", DbType.String, 200, "name");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Description = new Field("description", "ItemType", DbType.String, 1000, "description");
        }
        #endregion


    }
}

