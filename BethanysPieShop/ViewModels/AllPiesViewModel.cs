﻿using BethanysPieShop.Models;

namespace BethanysPieShop.ViewModels
{
	public class AllPiesViewModel
	{
        public AllPiesViewModel(IEnumerable<Pie> pies, List<Category> categories, string? currentCategory)
        {
            Pies = pies;
            Categories = categories;
            CurrentCategory = currentCategory;
        }

        public int CategoryId { get; set; }
        public List<Category> Categories { get; }
        public IEnumerable<Pie> Pies { get; }
        public string? CurrentCategory { get; }
    }
}
