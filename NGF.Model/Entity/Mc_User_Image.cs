//------------------------------------------------------------------------------
// File Name   : Ngf_User_Image.cs
// Creator     : Moses.Zhu
// Create Date : 2017-05-05
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
    /// 实体类Ngf_User_Image
    /// </summary>
    [Serializable]
    public class Ngf_User_Image : ITS.Data.EntityBase
    {
        public Ngf_User_Image() : base("ngf_user_image") { }

        #region Model
        private string _User_Id;
        private string _Image;
        /// <summary>
        /// 
        /// </summary>
        public string User_Id
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
        public string Image
        {
            get { return _Image; }
            set
            {
                this.OnPropertyValueChange(_.Image, _Image, value);
                this._Image = value;
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
                _.Image};
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._User_Id,
                this._Image};
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(IDataReader reader)
        {
            this._User_Id = DataUtils.ConvertValue<string>(reader["user_id"]);
            this._Image = DataUtils.ConvertValue<string>(reader["image"]);
        }
        /// <summary>
        /// 给当前实体赋值
        /// </summary>
        public override void SetPropertyValues(DataRow row)
        {
            this._User_Id = DataUtils.ConvertValue<string>(row["user_id"]);
            this._Image = DataUtils.ConvertValue<string>(row["image"]);
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
            public readonly static Field All = new Field("*", "ngf_user_image");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field User_Id = new Field("user_id", "ngf_user_image", DbType.String, 200, "user_id");
            /// <summary>
            /// 
            /// </summary>
            public readonly static Field Image = new Field("image", "ngf_user_image", DbType.String, -1, "image");
        }
        #endregion


    }
}

