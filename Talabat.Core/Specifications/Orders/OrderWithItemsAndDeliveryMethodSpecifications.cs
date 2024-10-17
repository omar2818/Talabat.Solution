﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Orders
{
    public class OrderWithItemsAndDeliveryMethodSpecifications : BaseSpecifications<Order>
    {
        public OrderWithItemsAndDeliveryMethodSpecifications()
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.ShipToAddress);
            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderWithItemsAndDeliveryMethodSpecifications(string buyerEmail)
            :base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);

            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderWithItemsAndDeliveryMethodSpecifications(int orderId, string buyerEmail)
            : base(O => (O.BuyerEmail == buyerEmail)&&(O.Id == orderId))
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);
        }
    }
}
