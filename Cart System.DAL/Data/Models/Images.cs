﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart_System.DAL
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }=string.Empty;
        public Product Product { get; set; }
    }
}
