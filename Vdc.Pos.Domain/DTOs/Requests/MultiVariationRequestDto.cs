﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class MultiVariationRequestDto
    {
        public Guid ParentCategoryId { get; set; }
        public string[] Names { get; set; }
    }
}