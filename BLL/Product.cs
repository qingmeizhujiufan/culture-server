using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Product
    {
        private Common commonBll = null;
        public Product()
        {
            this.commonBll = new Common();
        }

        //获取产品列表信息
        public DataTable GetProductList()
        {
            string strSql = @"select    id,
                                        ISNULL(name, '') as name,
                                        ISNULL(unit, '') as unit,
                                        price,
                                        ISNULL(type, '') as type,
                                        ISNULL(detail, '') as detail,
                                        ISNULL(structuralSection, '') as structuralSection,
                                        ISNULL(hardware, '') as hardware,
                                        ISNULL(sealant, '') as sealant,
                                        ISNULL(status, 0) as status,
                                        ISNULL(attaches, '') as attaches,
                                        ISNULL(coverAttaches, '') as coverAttaches,
                                        CONVERT(varchar(19) , create_time, 120 ) as create_time
                                        from dbo.wxop_product
                                        order by create_time desc";
            strSql = string.Format(strSql);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //获取产品基本信息
        public DataTable GetProductBaseInfo(string id)
        {
            string strSql = @"select    id,
                                        ISNULL(name, '') as name,
                                        ISNULL(unit, '') as unit,
                                        Convert(decimal(18,2), price) as price,
                                        ISNULL(type, '') as type,
                                        ISNULL(detail, '') as detail,
                                        ISNULL(structuralSection, '') as structuralSection,
                                        ISNULL(hardware, '') as hardware,
                                        ISNULL(sealant, '') as sealant,
                                        ISNULL(status, 0) as status,
                                        ISNULL(attaches, '') as attaches,
                                        ISNULL(coverAttaches, '') as coverAttaches,
                                        CONVERT(varchar(19) , create_time, 120 ) as create_time
                                    from dbo.wxop_product
                                    where id = '{0}'
                                    order by create_time desc";
            strSql = string.Format(strSql, id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //新增产品
        public bool AddProduct(string name, string unit, double price, string type, string detail, string attaches, string coverAttaches, string structuralSection, string hardware, string sealant)
        {
            string str = @"insert into dbo.wxop_product (name, unit, price, type, detail, attaches, coverAttaches, structuralSection, hardware, sealant) 
                            values ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
            str = string.Format(str, name, unit, price, type, detail, attaches, coverAttaches, structuralSection, hardware, sealant);

            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //更新产品
        public bool EditProduct(string name, string unit, double price, string type, string detail, string attaches, string coverAttaches, string structuralSection, string hardware, string sealant, string id)
        {
            string str = @"update dbo.wxop_product 
                                  set 
                                    name='{0}', 
                                    unit='{1}', 
                                    price={2}, 
                                    type='{3}', 
                                    detail='{4}', 
                                    attaches='{5}', 
                                    coverAttaches='{6}' ,
                                    structuralSection='{7}',
                                    hardware='{8}',
                                    sealant='{9}'
                            where id='{10}'";
            str = string.Format(str, name, unit, price, type, detail, attaches, coverAttaches, structuralSection, hardware, sealant, id);
            CommonTool.WriteLog.Write(str);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除商品
        public bool DelProduct(string id)
        {
            string str = @"delete from dbo.wxop_product where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    
        //获取品牌列表   1）门窗品牌   2）五金配件品牌    3）密封胶品牌
        public DataTable GetBrandList(int type)
        {
            string strSql = @"select    id,
                                        ISNULL(name, '') as name,
                                        ISNULL(type, '') as type,
                                        CONVERT(varchar(19) , create_time, 120 ) as create_time
                                        from dbo.wxop_brand
                                        where type = '{0}'
                                        order by create_time desc";
            strSql = string.Format(strSql, type);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        //添加品牌
        public bool AddBrand(int type, string name)
        {
            string str = @"insert into dbo.wxop_brand (name, type) 
                            values ('{0}', '{1}')";
            str = string.Format(str, name, type);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

        //删除品牌
        public bool DelBrand(string id)
        {
            string str = @"delete from dbo.wxop_brand where id='{0}'";
            str = string.Format(str, id);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }
    }
}
