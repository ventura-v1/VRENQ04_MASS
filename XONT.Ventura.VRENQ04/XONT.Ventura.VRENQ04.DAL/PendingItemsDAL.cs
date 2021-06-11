using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XONT.Common.Data;
using XONT.Common.Message;
using System.Data;
using XONT.Ventura.Common.Prompt;
using XONT.Ventura.Common.ConvertDateTime;

namespace XONT.Ventura.VRENQ04
{
    public class PendingItemsDAL
    {
        private readonly CommonDBService _dbService;
        private DataTable dt;
        StringBuilder sqlBuilder;
        ParameterSet paramSet;
        List<SPParameter> spParametersList;

        public PendingItemsDAL()
        {
            _dbService = new CommonDBService();
        }

        //public DataTable GetPendingPOItems(string businessUnit, string productcode, PendingItemDomain pendingItems, ref MessageSet msg)
        public List<PendingItemDomain> GetPendingPOItems(string businessUnit, string productcode, PendingItemDomain pendingItems, ref MessageSet msg)
        {
            List<PendingItemDomain> listDomain = new List<PendingItemDomain>(); 

            try
            {
                //_dbService.StartService();
                dt = new DataTable();
                paramSet = new ParameterSet();
                spParametersList = new List<SPParameter>();

                paramSet.SetSPParameterList(spParametersList, "BusinessUnit", businessUnit, "");
                paramSet.SetSPParameterList(spParametersList, "ProductCode", productcode, "");
                paramSet.SetSPParameterList(spParametersList, "ProductClassCount", pendingItems.LstProductClassification.Count,
                                            "");
                paramSet.SetSPParameterList(spParametersList, "ProductClassification", pendingItems.ProductClassificationCode,
                                            "");
                paramSet.SetSPParameterList(spParametersList, "ProductClassificationCode", pendingItems.ProductClassification,
                                            "");

                _dbService.StartService();
                dt = _dbService.FillDataTable(CommonVar.DBConName.UserDB, "[RD].[usp_VRENQ04GetReportData]", spParametersList);
                
                //dt = _dbService.FillDataTable(com.ToString());

                foreach (DataRow row in dt.Rows)
                {
                    pendingItems = new PendingItemDomain();

                    pendingItems.ItemCode = row["ItemCode"].ToString();
                    pendingItems.Description = row["Description"].ToString();
                    pendingItems.OrderQty = decimal.Parse(row["OrderQty"].ToString());
                    pendingItems.DeliveryDate = ConvertDateTime.DisplayDateTime(Convert.ToDateTime(row["DeliveryDate"].ToString()));
                    pendingItems.OrderDate = ConvertDateTime.DisplayDateTime(Convert.ToDateTime(row["OrderDate"].ToString()));
                    pendingItems.Supplier = row["Supplier"].ToString();
                    pendingItems.PONo = row["PurchaseCategoryCode"].ToString() + @"\" + int.Parse(row["OrderNumber"].ToString());
                    //pendingItems.OrderNumber = Convert.ToInt32(row["OrderNumber"].ToString());

                    listDomain.Add(pendingItems);
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetPendingPOItems", "XONT.Ventura.VRENQ04.DAL");
            }
            finally
            {
                _dbService.CloseService();
            }
            return listDomain;
            //return dt;
        }

        public DataTable GetProductClassifications(string buisnessUnit, ref MessageSet message)
        {
            sqlBuilder = new StringBuilder();
            string sql;
            message = null;
            try
            {
                _dbService.StartService();

                sqlBuilder.Append(" SELECT RTRIM(ProdClassifications.MasterGroup) as ProdClassification, RTRIM(ProdClassifications.GroupDescription) as ProdClassificationDes ");
                sqlBuilder.Append(" FROM XA.MasterDefinition as ProdClassifications ");
                sqlBuilder.Append(" WHERE ProdClassifications.BusinessUnit ='" + buisnessUnit + "' AND ProdClassifications.GroupType='02' ");

                sql = sqlBuilder.ToString();
                dt = _dbService.FillDataTable(sql);

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetProductClassifications", "XONT.Ventura.VRENQ04.DAL");
            }
            finally
            {
                _dbService.CloseService();
            }

            return dt;
        }

        public DataTable GetProductCode( string buisnessUnit, PendingItemDomain info, ref MessageSet msg)
        {
            var dTable = new DataTable("Descriptions");
            try
            {
                msg = null;
                var sql = new StringBuilder();

                sql.Append(
                    " Select Product.ProductCode as MasterGroupValue,ISNULL(Product.Description,'') as MasterGroupValueDescription");
                
                    sql.Append(" from RD.Product As Product");
              
                    sql.Append(" where Product.BusinessUnit='" + buisnessUnit + "' AND (Product.ProductCode = '" +
                           info.ProductCode.Trim() + "')");
                _dbService.StartService();

                dTable = _dbService.FillDataTable(CommonVar.DBConName.UserDB, sql.ToString());
                _dbService.CloseService();
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetProductCode", "XONT.Ventura.VRENQ04.DAL.dll");
            }

            return dTable;
        }
    }
}
