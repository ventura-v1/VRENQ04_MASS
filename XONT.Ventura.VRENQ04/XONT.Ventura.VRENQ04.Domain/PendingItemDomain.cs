using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XONT.Ventura.Common.Prompt;

namespace XONT.Ventura.VRENQ04
{
    [Serializable]
    public class PendingItemDomain
    {
        public string TerritoryCode { set; get; }
        public string ProductCode { set; get; }
        public string ProductDis { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public decimal OrderQty { get; set; }
        public string DeliveryDate { get; set; }
        public string OrderDate { get; set; }
        public string Supplier { get; set; }
        public string PurchaseCategoryCode { get; set; }
        public int OrderNumber { get; set; }

        public string PONo { get; set; }

        public List<ClassificationRecord> ProductClassifications { get; set; }
        public List<PromptParameter> LstProductClassification { get; set; }

        public string ProductClassification
        {
            get
            {
                string retrunval = "";
                if ((LstProductClassification != null) && (LstProductClassification.Count > 0))
                {
                    foreach (PromptParameter p in LstProductClassification)
                    {
                        if (p.ParameterValue.ToString() != "")
                        {
                            retrunval = retrunval + "," + p.ParameterValue;
                        }
                    }

                    if (retrunval != "")
                    {
                        retrunval = retrunval.Substring(1);
                    }
                }

                return retrunval;
            }
        }

        public string ProductClassificationCode
        {
            get
            {
                string retrunval = "";
                if ((LstProductClassification != null) && (LstProductClassification.Count > 0))
                {
                    foreach (PromptParameter p in LstProductClassification)
                    {
                        if (p.ParameterValue.ToString() != "")
                        {
                            retrunval = retrunval + "," + p.ParameterCode;
                        }
                    }

                    if (retrunval != "")
                    {
                        retrunval = retrunval.Substring(1);
                    }
                }

                return retrunval;
            }
        }
    }
}
