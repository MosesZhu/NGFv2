//------------------------------------------------------------------------------
// File Name   : Item.cs
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
    /// 实体类Item
    /// </summary>
    [Serializable]
    public class Item : ITS.Data.EntityBase
    {
        public Item() : base("Item") { }

        #region Model
        private Guid _Id;
        private string _Item_No;
        private string _Description;
        private string _Item_Type_Id;
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
        public string Item_No
        {
            get { return _Item_No; }
            set
            {
                this.OnPropertyValueChange(_.Item_No, _Item_No, value);
                this._Item_No = value;
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
        public string Item_Type_Id
        {
            get { return _Item_Type_Id; }
            set
            {
                this.OnPropertyValueChange(_.Item_Type_Id, _Item_Type_Id, value);
                this._Item_Type_Id = value;
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
				_.Item_No,
				_.Description,
				_.Item_Type_Id};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
				this._Id,
				this._Item_No,
				this._Description,
				this._Item_Type_Id};
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(IDataReader reader)
        {
            this._Id = DataUtils.ConvertValue<Guid>(reader["id"]);
            this._Item_No = DataUtils.ConvertValue<string>(reader["item_no"]);
            this._Description = DataUtils.ConvertValue<string>(reader["description"]);
            this._Item_Type_Id = DataUtils.ConvertValue<string>(reader["item_type_id"]);
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(DataRow row)
        {
            this._Id = DataUtils.ConvertValue<Guid>(row["id"]);
            this._Item_No = DataUtils.ConvertValue<string>(row["item_no"]);
            this._Description = DataUtils.ConvertValue<string>(row["description"]);
            this._Item_Type_Id = DataUtils.ConvertValue<string>(row["item_type_id"]);
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
            public readonly static Field All = new Field("*", "Item");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Id = new Field("id", "Item", DbType.Guid, 16, "id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Item_No = new Field("item_no", "Item", DbType.String, 100, "item_no");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Description = new Field("description", "Item", DbType.String, 1000, "description");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Item_Type_Id = new Field("item_type_id", "Item", DbType.String, 100, "item_type_id");
        }
        #endregion


    }
}

