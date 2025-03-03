﻿using System;

namespace BUILD_WEEK_4_TEAM_7.Models {
    public class Cart {
        public Guid IdCart {
            get; set;
        }

        public Guid IdProduct {
            get; set;
        }

        public string ProductName {
            get; set;
        }

        public decimal Price {
            get; set;
        }

        public string ImageURL {
            get; set;
        }
    }
}
