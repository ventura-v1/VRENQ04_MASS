using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using XONT.Ventura.Common;
using XONT.Common.Data;
using System.Xml.Linq;
using XONT.Ventura.AppConsole;
using XONT.Common.Message;
using XONT.Ventura.VRENQ04;
using XONT.Ventura.Common.Prompt;

namespace XONT.Ventura.VRENQ04
{
    public partial class VRENQ04 : CustomPage
    {
        private User _user;
        private MessageSet _message;
        private PendingItemBLL _POItemsBLL;
        private List<ProductClassification> productClassificationList;
        private PendingItemDomain pendingItems;
        private List<PendingItemDomain> listDomain;

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                //var user = new User { UserName = "xontadmin", BusinessUnit = "MASS", UserLevelGroup = "USER" };
                //Session["Main_LoginUser"] = user;

                _user = (User)Session["Main_LoginUser"];

                clsProductClassification.BusinessUnit = _user.BusinessUnit;
                clsProductClassification.ClassificationType = "02";
                clsProductClassification.UserName = _user.UserName;

            }
            catch (Exception ex)
            {
                _message = MessageCreate.CreateErrorMessage(0, ex, "Page_Init",
                                                         "XONT.Ventura.VRENQ04.Web");
                Session["Error"] = _message;
                MessageDisplay.Dispaly(this);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (User)Session["Main_LoginUser"];

            _POItemsBLL = new PendingItemBLL();
            if (!IsPostBack)
            {
                #region Add Field Level Authentication

                var feildLevelAuthenticBll = new FeildLevelAuthenticBLL();
                feildLevelAuthenticBll.AddAuthentic(this, _user, "VRENQ04", "VRENQ04");

                #endregion

                #region for GridView Size

                IGridViewControlManager grdViewControl = new GridViewControlManager();
                MasterControl masterControlData = new MasterControl();
                masterControlData = grdViewControl.GetMasterControlData(_user.BusinessUnit, "VRENQ04");

                if (masterControlData != null)
                {
                    if (masterControlData.AllowPaging == "1")
                    {
                        grvPendigItems.AllowPaging = true;
                        grvPendigItems.PageSize = masterControlData.PageSize;
                        //grvPendigItems.PageSize = 100;
                    }
                    else
                    {
                        grvPendigItems.AllowPaging = false;
                    }
                }
                else
                {
                    grvPendigItems.AllowPaging = false;
                   
                }

                #endregion

                var validateInfo = (PendingItemDomain)Session["VRENQ04_ValidationInfo"];
                if (validateInfo != null)
                {
                    txtProduct.Text = validateInfo.ProductCode;
                    txtProductDis.Text = validateInfo.ProductDis;
                    clsProductClassification.SelectedClassifications = pendingItems.ProductClassifications;
                }
               
            }
        }

        //private bool ValidateInputs()
        //{
        //    const bool isValid = true;
        //    try
        //    {

        //        if (!string.IsNullOrEmpty(txtProduct.Text.Trim()))
        //        {
        //            //pendingItems.UserName = _user.UserName.Trim();
        //            pendingItems.ProductCode = txtProduct.Text.Trim();
        //            DataTable product = _POItemsBLL.GetProductCode(_user.BusinessUnit, pendingItems, ref _message);
        //            if (product.Rows.Count > 0)
        //            {
        //                txtProduct.Text = product.Rows[0]["MasterGroupValue"].ToString();
        //                txtProductDis.Text = product.Rows[0]["MasterGroupValueDescription"].ToString();
        //            }
        //            else
        //            {
        //                //cusValCommon.ErrorMessage = "Invalid ProductCode";
        //                //cusValCommon.IsValid = false;
        //                txtProductDis.Text = "";
        //                return false;
        //            }
        //        }
        //        //else
        //        //{
        //        //    txtProductDis.Text = "";
                    
        //        //}

        //    }
        //    catch (Exception exception)
        //    {
        //        _message = MessageCreate.CreateErrorMessage(0, exception, "ValidateInputs",
        //                                                "XONT.Ventura.VRENQ04.Web.Dll");
        //        Session["Error"] = _message;
        //        MessageDisplay.Dispaly(btnExit);
        //    }
        //    return isValid;
        //}

        protected void txtProduct_TextChanged(object sender, EventArgs e)
        {
            //if (txtProduct.Text.Trim() == "")
            //{
            //    txtProductDis.Text = "";
            //}

            try
            {
                pendingItems = new PendingItemDomain();

                if (!string.IsNullOrEmpty(txtProduct.Text.Trim()))
                {

                    //pendingItems.UserName = _user.UserName.Trim();
                    pendingItems.ProductCode = txtProduct.Text.Trim();
                    DataTable product = _POItemsBLL.GetProductCode(_user.BusinessUnit, pendingItems, ref _message);
                    if (product.Rows.Count > 0)
                    {
                        txtProduct.Text = product.Rows[0]["MasterGroupValue"].ToString();
                        txtProductDis.Text = product.Rows[0]["MasterGroupValueDescription"].ToString();
                    }
                    else
                    {
                        //cusValCommon.ErrorMessage = "Invalid ProductCode";
                        //cusValCommon.IsValid = false;
                        txtProductDis.Text = "";
                    }
                }
                else
                    txtProductDis.Text = "";

            }
            catch (Exception exception)
            {
                _message = MessageCreate.CreateErrorMessage(0, exception, "txtProduct_TextChanged",
                                                        "XONT.Ventura.VRENQ04.Web.Dll");
                Session["Error"] = _message;
                MessageDisplay.Dispaly(btnExit);
            }

        }

        protected void btnProduct_Click(object sender, EventArgs e)
        {
            try
            {
                var promptParameters = new List<PromptParameter>();
                var headings = new List<string> { "ProductCode", "Description" };
                var fieldNames = new List<string> { "ProductCode", "Description" };
                mptProduct.PromptTitle = "Product Code";

                //mptProduct.ShowMasterClassificationPopup(_user.BusinessUnit, _user.UserName, "02", promptParameters,
                //                                         headings, fieldNames);
                mptProduct.ShowMasterClassificationPopup(_user.BusinessUnit, _user.UserName, "02", promptParameters, 
                                                            StatusFlag.All, headings, fieldNames);
            }
            catch (Exception exception)
            {
                _message = MessageCreate.CreateErrorMessage(0, exception, "btnProduct_Click", "XONT.Ventura.VRENQ04.Web.dll");
                Session["Error"] = _message;
                MessageDisplay.Dispaly(this);
            }
        }

        protected void mptProduct_Cancelled(object sender, PromptResultsEventArgs e)
        {
            txtProduct.Text = "";
            txtProductDis.Text = "";
        }

        protected void mptProduct_ItemSelected(object sender, PromptResultsEventArgs e)
        {
            txtProduct.Text = e.SelectedValue("ProductCode").ToString();
            txtProductDis.Text = e.SelectedValue("Description").ToString();
 
            if (txtProduct.Enabled)
            {
                txtProductDis.Focus();
            }

        }
        protected void GetGridViewData()
        {
            PendingItemDomain pendingItems = new PendingItemDomain();

            List<PendingItemDomain> listDomain = new List<PendingItemDomain>();

            //if (!ValidateInputs())
            //{
            //    //grvPendigItems.DataSource = null;
            //    //grvPendigItems.DataBind();
            //    return;
            //}
                
            //RegularExpressionValidator1.Validate();
            //if (BaseValidator.)

            if (!string.IsNullOrEmpty(txtProduct.Text))
                pendingItems.ProductCode = txtProduct.Text;

            _message = null;
            string productcode = txtProduct.Text;
            List<PromptParameter> selectioncriteria = clsProductClassification.SelectedCriteria;
            pendingItems.ProductClassifications = clsProductClassification.SelectedClassifications;
            pendingItems.LstProductClassification = clsProductClassification.SelectedCriteria;

            listDomain = _POItemsBLL.GetPendingPOItems(_user.BusinessUnit, productcode, pendingItems, ref _message);

            ViewState["POItems"] = listDomain;

            grvPendigItems.DataSource = listDomain;
            grvPendigItems.DataBind();

        }
        protected void btnList_Click(object sender, EventArgs e)
        {

            GetGridViewData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseTab();
        }

        protected void grvPendigItems_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            try
            {
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                listDomain = (List<PendingItemDomain>)ViewState["POItems"];
                if (listDomain != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        listDomain = listDomain.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        listDomain = listDomain.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }
                grvPendigItems.DataSource = listDomain;
                grvPendigItems.DataBind();
            }
            catch (Exception ex)
            {
                _message = MessageCreate.CreateErrorMessage(0, ex, "grvPendigItems_Sorting",
                                                        "XONT.Ventura.VRENQ04.Web");
                Session["Error"] = _message;
                MessageDisplay.Dispaly(this);
                return;
            }
        }

        protected void grvPendigItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                grvPendigItems.SelectedIndex = -1;
                grvPendigItems.PageIndex = e.NewPageIndex;
                //grvPendigItems.DataBind();
                List<PendingItemDomain> listDomain = new List<PendingItemDomain>();
                listDomain = (List<PendingItemDomain>)ViewState["POItems"];
                grvPendigItems.DataSource = listDomain;
                grvPendigItems.DataBind();
            }
            catch (Exception ex)
            {
                MessageSet message = MessageCreate.CreateErrorMessage(0, ex, "grvPendigItems_PageIndexChanging", "XONT.Ventura.VRENQ04.WEB.dll");
                if (message != null)
                {
                    Session["Error"] = message;
                    MessageDisplay.Dispaly(this);
                }
            }
        }

        protected void grvPendigItems_RowCreated(object sender, GridViewRowEventArgs e)
        {
           
        }
    }
}
