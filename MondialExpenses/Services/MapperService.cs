﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MondialExpenses.Models;
using MondialExpenses.ViewModels;

namespace MondialExpenses.Services
{
    public class MapperService : Profile
    {
        public MapperService()
        {
            CreateMap<Cashier, CreateCashierVM>().ReverseMap();
            CreateMap<Expense, ExpenseVM>().ReverseMap();
            CreateMap<IdentityUser, EditUserVM>().ReverseMap();
        }
    }
}
