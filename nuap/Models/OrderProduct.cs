using System;
using System.Collections.Generic;
using System.Text;

namespace nuap.Models
{
    class OrderProduct
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public int quantity { get; set; }
        public int category_id { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public int is_featured { get; set; }
        public int stock { get; set; }
        public int weight { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int purchase_price { get; set; }
        public int sale_price { get; set; }
        public int special_price { get; set; }
        public int has_special_price { get; set; }
        public string image { get; set; }
        public int position { get; set; }
    }
}
