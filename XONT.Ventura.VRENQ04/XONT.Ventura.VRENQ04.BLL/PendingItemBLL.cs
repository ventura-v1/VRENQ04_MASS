using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XONT.Common.Message;
using System.Data;
using XONT.Ventura.Common.Prompt;

namespace XONT.Ventura.VRENQ04
{
    public class PendingItemBLL
    {
        private readonly PendingItemsDAL _dal;

        public PendingItemBLL() 
        {
            _dal = new PendingItemsDAL();
        
        }

        public List<PendingItemDomain> GetPendingPOItems(string businessUnit, string productcode, PendingItemDomain pendingItems, ref MessageSet msg)
        {
            return _dal.GetPendingPOItems(businessUnit, productcode, pendingItems,  ref msg);
        }

        public DataTable GetProductClassifications(string buisnessUnit, ref MessageSet message)
        {
            return _dal.GetProductClassifications(buisnessUnit, ref message);
        }

        public DataTable GetProductCode(string buisnessUnit, PendingItemDomain info, ref MessageSet msg)
        {
            return _dal.GetProductCode(buisnessUnit, info, ref msg);
        }
    }
}
